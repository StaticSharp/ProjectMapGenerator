using AngleSharp.Dom;
using StaticSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharpProjectMap.TestTarget.Root
{
    public class ProtoNode : StaticSharp.Tree.Node
    {
        public override StaticSharp.Tree.Node Root => throw new NotImplementedException();

        public override StaticSharp.Tree.Node Parent => throw new NotImplementedException();

        public override Page? Representative => throw new NotImplementedException();

        public override IEnumerable<StaticSharp.Tree.Node> Children => throw new NotImplementedException();

        public override string[] Path => throw new NotImplementedException();

        public override string Name => throw new NotImplementedException();
    }
}
