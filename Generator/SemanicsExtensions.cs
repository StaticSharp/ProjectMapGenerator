using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaticSharpProjectMapGenerator
{
    public static class SemanicsExtensions
    {
        // TODO: not in use
        public static string GetFullNamespaceString(this INamedTypeSymbol _this)
        {
            var nsNames = new List<string>();

            var currentNs = _this.ContainingNamespace;
            while(!currentNs.IsGlobalNamespace)
            {
                nsNames.Add(currentNs.Name);
                currentNs = currentNs.ContainingNamespace;
            }

            return string.Join(".", nsNames);
        }

        public static bool IsDescendantOf(this INamedTypeSymbol typeSymbol, INamedTypeSymbol ancestorCandidate) =>
            typeSymbol.BaseType != null &&
            (SymbolEqualityComparer.Default.Equals(typeSymbol.BaseType, ancestorCandidate)
            || typeSymbol.BaseType.IsDescendantOf(ancestorCandidate));
    }
}
