using AngleSharp.Dom;
using StaticSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharpProjectMap.TestTarget.Root
{
    public enum Language
    {
        En,
        Ru,
        Sp
    }

    public abstract class BaseProtoNode<T2, T> : MultilanguageProtoNode<T> where T: struct, Enum
    {
        protected BaseProtoNode(T language) : base(language)
        {
        }
    }

    public abstract class ProtoNode : BaseProtoNode<int, Language> //StaticSharp.Tree.Node //
    {
        protected ProtoNode(Language language) : base(language)
        {
        }
    }
}
