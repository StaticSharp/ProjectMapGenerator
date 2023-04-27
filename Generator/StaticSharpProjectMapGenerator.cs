using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using StaticSharpProjectMapGenerator.Models;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;

namespace StaticSharpProjectMapGenerator
{
    [Microsoft.CodeAnalysis.Generator]
    public class StaticSharpProjectMapGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {

        }

        public void Execute(GeneratorExecutionContext context)
        {
            try {
                // TODO: figure out about AnalyzerConfigOptions
                var targetProjectPath = context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.projectdir", out var result) ? result : "";
                var projectMapFilePath = Path.Combine(targetProjectPath, "ProjectMap.json");

                var ssAnalyzer = new PagesFinder(context.Compilation);

                var pageTreeFactory = new PageTreeFactory(ssAnalyzer);
                var projectMap = pageTreeFactory.CreatePageTree(context.Compilation);

                // append base pages
                var pages = ssAnalyzer.GetBasePageDescendants();
                var basePages = pages.Where(_ => _.IsAbstract).ToList();
                basePages.Add(ssAnalyzer.GetPrimalPageSymbol());
                projectMap.PageTypes = basePages.Select(p => p.ToString()).ToList();

                // append languages
                // TODO: move to PageFinder?
                var protonode = context.Compilation.GetSymbolsWithName("ProtoNode").SingleOrDefault();
                var multilanguageProtonode = context.Compilation.GetTypesByMetadataName("StaticSharp.MultilanguageProtoNode`1").First(); // TODO: First()

                // TODO: indirect nesting, wrap, add checks
                var temp = SymbolEqualityComparer.Default.Equals(((INamedTypeSymbol)protonode).BaseType.ConstructedFrom, multilanguageProtonode);
                if (temp)
                {
                    var currentMultilanguageProtonode = ((INamedTypeSymbol)protonode).BaseType;
                    var languageEnum = currentMultilanguageProtonode.TypeArguments.First();
                    var languagesSyntaxNodes = languageEnum.DeclaringSyntaxReferences.First().GetSyntax().ChildNodes();

                    projectMap.Languages = languagesSyntaxNodes.Where(s => s.IsKind(SyntaxKind.EnumMemberDeclaration)).Select(s => s.ToString()).ToList();
                }
                else
                {
                    projectMap.Languages = new List<string> { "" };
                }

                var projectMapJson = JsonSerializer.Serialize(projectMap);

                var fileWriteSuccess = true;

                try {
                    File.WriteAllText(projectMapFilePath, projectMapJson);
                }
                catch (Exception ex) {
                    fileWriteSuccess = false;
                }


//                context.AddSource("Generated.cs", $@"
//public static class GeneratorDebug
//{{
//    public static string ProjectMapPath = ""{targetProjectPath.Replace("\\", "\\\\")}"";
//    public static bool WriteSuccess = {fileWriteSuccess.ToString().ToLower()};
//}}
//");
            }
            catch(Exception e) {
                SimpleLogger.Log($"EXCEPTION Type: {e.GetType()}");
                SimpleLogger.Log($"EXCEPTION Message: {e.Message}");
                SimpleLogger.Log($"EXCEPTION StackTrace: {e.StackTrace}");
            }
            finally {
                SimpleLogger.Flush();
            }
        }   
    }
}
//{Path.Combine(ProjectDirectory.GetPath(), "ProjectMap.json").Replace("\\", "\\\\" )}