﻿using StaticSharpProjectMapGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaticSharpProjectMapGenerator
{
    public static class PageMapExtensions
    {
        public static PageMap GetPageByPath(this ProjectMap _this, IEnumerable<string> pagePathSegments)
        {
            if (!pagePathSegments.Any() || _this.Root.Name != pagePathSegments.First()) {
                return null;
            }

            var currentPage = _this.Root;
            foreach (var pathSegment in pagePathSegments.Skip(1)) {
                currentPage = currentPage.ChildPages.FirstOrDefault(_ => _.Name == pathSegment);
                if (currentPage == null) {
                    return null;
                }
            }

            return currentPage;
        }

        // Returns last page in the path, new or existing
        public static PageMap GetOrCreatePageByPath(this ProjectMap _this, IEnumerable<string> pagePathSegments)
        {
            if (!pagePathSegments.Any()) {
                return null;
            }

            if (_this.Root.Name != pagePathSegments.First()) {
                throw new Exception("Unable to create page not under root");
            }

            var currentPage = _this.Root;
            foreach (var pathSegment in pagePathSegments.Skip(1)) {
                var nextPage = currentPage.ChildPages.FirstOrDefault(_ => _.Name == pathSegment);
                if (nextPage == null) {
                    nextPage = new PageMap(pathSegment, pagePathSegments);
                    currentPage.ChildPages.Add(nextPage);
                }

                currentPage = nextPage;
            }

            return currentPage;
        }
    }
}
