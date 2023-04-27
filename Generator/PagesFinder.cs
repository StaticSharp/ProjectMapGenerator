using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaticSharpProjectMapGenerator
{
    public class PagesFinder
    {
        protected Compilation _compilation { get; set; }
        protected List<INamedTypeSymbol> _pageDescendants { get; set; } // TODO: maybe there is a sense to optimize List<>

        public PagesFinder(Compilation compilation) { 
            _compilation = compilation;
        }

        protected INamedTypeSymbol _basePageSymbol;

        public INamedTypeSymbol GetPrimalPageSymbol()
        {
            if (_basePageSymbol == null)
            {
                _basePageSymbol = _compilation.GetTypesByMetadataName("StaticSharp.Page").First(); // TODO: First()
            }

            return _basePageSymbol;
        }
           
        public List<INamedTypeSymbol> GetBasePageDescendants()
        {
            if (_pageDescendants == null)
            {

                var pagePossibleParents = new List<INamedTypeSymbol>();
                var allSymbols = _compilation.GetSymbolsWithName(_ => true);
                var typeSymbols = allSymbols.OfType<INamedTypeSymbol>();
                foreach (var typeSymbol in typeSymbols)
                {
                    if (typeSymbol.IsDescendantOf(GetPrimalPageSymbol()))
                    {
                        pagePossibleParents.Add(typeSymbol);
                    }
                }

                _pageDescendants = pagePossibleParents;
            }

            return _pageDescendants;
        }
    }
}
