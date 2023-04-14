using System;
using System.Collections.Generic;
using System.Text;

namespace StaticSharpProjectMapGenerator.Models
{
    public class PageMap
    {
        public PageMap(string name, IEnumerable<string> pathSegments)
        {
            Name = name;
            PathSegments = pathSegments;
        }

        public string Name { get; }

        public IEnumerable<string> PathSegments { get; } // TODO: not in use now, remove? Constructed in TS from page names

        public List<PageMap> ChildPages { get; set;} = new List<PageMap>();

        public string Debug {get; set;}

        public List<RepresentativeMap> Representatives { get; set; } = new List<RepresentativeMap>();

        //public List<string> ResourceFiles { get; set; } = new List<string>();
    }
}
