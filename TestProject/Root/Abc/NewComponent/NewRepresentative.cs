using StaticSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharpProjectMap.TestTarget.Root.Abc
{
    namespace NewComponent
    {
        [Representative]
        public class NewRepresentative
        {
            public SomeAdditionalClass SomeProp => new SomeAdditionalClass();
        }
    }

    public class SomeAdditionalClass
    {

    }
}
