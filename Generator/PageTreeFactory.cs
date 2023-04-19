using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using StaticSharpProjectMapGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace StaticSharpProjectMapGenerator
{
    public class PageTreeFactory
    {
        public ProjectMap CreatePageTree(Compilation compilation) // TODO: review ProjectMap vs PageTree
        {
            //// finding root (by protonode)

            var protonode = compilation.GetSymbolsWithName("ProtoNode").SingleOrDefault();
            if (protonode == null) {
                throw new Exception("ProtoNode not found or multiple ProtoNode's"); // TODO: notify user of exceptions
            }

            var rootNamespace = protonode.ContainingNamespace;
            // TODO: relative? partial?
            var rootFilePath = protonode.DeclaringSyntaxReferences.First().GetSyntax().SyntaxTree.FilePath;
            var pathToRoot = Path.GetDirectoryName(Path.GetDirectoryName(rootFilePath));  // TODO: remove this from others files names?

            var rootContainingNamespaces = new List<string>();
            if (!rootNamespace.IsGlobalNamespace) {
                var currentOuterNamespace = rootNamespace.ContainingNamespace;
                while (!currentOuterNamespace.IsGlobalNamespace) {
                    rootContainingNamespaces.Add(currentOuterNamespace.Name);
                    currentOuterNamespace = currentOuterNamespace.ContainingNamespace;
                }
            }

            rootContainingNamespaces.Reverse();
            var rootContainingNamespaceString = string.Join(".", rootContainingNamespaces);
            var projectMap = new ProjectMap(compilation.AssemblyName, rootNamespace.Name, pathToRoot, rootContainingNamespaceString);            

            //// find representatives
            var allSymbols = compilation.GetSymbolsWithName(_ => true);
            var typeSymbols = allSymbols.OfType<INamedTypeSymbol>(); // TODO: optimization possible - only visit rootNamespace descendents
            var pageSymbols = typeSymbols.Where(_ => _.GetAttributes()
                .Any(__ => __.AttributeClass.ConstructedFrom.ToString() == "StaticSharp.RepresentativeAttribute"));
            ///

            // construct tree
            foreach (var pageSymbol in pageSymbols) {
                var currentNamespace = pageSymbol.ContainingNamespace;

                IEnumerable<string> pagePathSegments = new List<string>();

                while (!SymbolEqualityComparer.Default.Equals(currentNamespace, rootNamespace)) {
                    pagePathSegments = pagePathSegments.Prepend(currentNamespace.Name);

                    if (string.IsNullOrEmpty(currentNamespace.Name) && !currentNamespace.IsGlobalNamespace) {
                        // TODO: realy strange hack: this code never executes,
                        // though without it Name=="" in a specific case: when appending chars to it's end without saving
                        Console.WriteLine(JsonSerializer.Serialize(currentNamespace));
                    }

                    currentNamespace = currentNamespace.ContainingNamespace;

                    if (currentNamespace == null) {
                        break;
                    }
                }
                
                if (currentNamespace == null) {
                    // TODO: notify user
                    SimpleLogger.Log($"WARNING: Page not under root. Page type: {pageSymbol.Name}");
                    break;
                }

                pagePathSegments = pagePathSegments.Prepend(rootNamespace.Name);

                // TODO: relative? partial?
                var filePath = pageSymbol.DeclaringSyntaxReferences.First().GetSyntax().SyntaxTree.FilePath;

                if (string.IsNullOrEmpty(pageSymbol.Name)) {
                    // TODO: realy strange hack: this code never executes,
                    // though without it Name=="" in a specific case: when appending chars to it's end without saving
                    Console.WriteLine(JsonSerializer.Serialize(pageSymbol));
                }

                var page = projectMap.GetOrCreatePageByPath(pagePathSegments);
                page.Pages.Add( new PageMap {
                    Name = pageSymbol.Name,
                    FilePath = filePath,
                    PageCsDescription = CreatePageCsDescription(pageSymbol)
                });
            }

            return projectMap;
        }

        protected PageCsDescription CreatePageCsDescription(INamedTypeSymbol pageSymbol)
        {

            var classSyntaxReference = pageSymbol.DeclaringSyntaxReferences.First();

            var fileSyntaxNode = classSyntaxReference.SyntaxTree.GetRoot();

            var classSyntaxNode = classSyntaxReference.GetSyntax();
            var className = classSyntaxNode.ChildTokens().First(_ => _.RawKind == 8508); // TODO: 8508?!!! == IdentifierToken?
            //declarationSyntaxNode.GetAnnotatedTokens("")


            var exclussiveWrapper = GetExclusiveWrapper(classSyntaxNode);

            var result = new PageCsDescription {
                ClassDefinition = classSyntaxNode.ToFileTextRange(),
                ClassName = className.ToFileTextRange(),
                ProposedDefinitionLine = fileSyntaxNode.LineSpan().EndLinePosition.Line + 1,
                ProposedDefinitionColumn = 0,

                FileScopedNamespace = null,

                ExclusiveNamespaceWrapper = exclussiveWrapper.ToFileTextRange()
            };

            return result;
        }

        protected SyntaxNode GetExclusiveWrapper(SyntaxNode target) {
            var sibblings = target.Parent.ChildNodes();
            if (sibblings.Count() == 2 ) { // namespace Idendificator and self // TODO: exclude usings
                return GetExclusiveWrapper(target.Parent);
            } else {
                return target;
            }
        }
    }       

    

}
