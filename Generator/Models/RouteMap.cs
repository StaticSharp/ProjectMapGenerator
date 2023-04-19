using System;
using System.Collections.Generic;
using System.Text;

namespace StaticSharpProjectMapGenerator.Models
{
    public class RouteMap
    {
        public RouteMap(string name/*, IEnumerable<string> pathSegments*/)
        {
            Name = name;
            //PathSegments = pathSegments;
        }

        public string Name { get; }

        //public IEnumerable<string> PathSegments { get; } // TODO: not in use now, remove? Constructed in TS from page names

        public List<RouteMap> ChildRoutes{ get; set;} = new List<RouteMap>();

        public string Debug {get; set;}

        public List<PageMap> Pages { get; set; } = new List<PageMap>();

        //public List<string> ResourceFiles { get; set; } = new List<string>();
    }
}
