using System;
using System.Collections.Generic;
using System.Text;

namespace StaticSharpProjectMapGenerator.ContractModels
{
    public class PageMap
    {
        public string Name { get; set; }

        public string FilePath { get; set; }

        public PageCsDescription PageCsDescription { get; set; }
    }
}
