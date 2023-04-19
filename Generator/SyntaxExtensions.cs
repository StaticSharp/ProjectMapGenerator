using Microsoft.CodeAnalysis;
using StaticSharpProjectMapGenerator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StaticSharpProjectMapGenerator
{
    public static class SyntaxExtensions
    {
        public static FileLinePositionSpan LineSpan(this SyntaxNode node) => node.SyntaxTree.GetLineSpan(node.Span);

        public static FileTextRange ToFileTextRange(this SyntaxNode node)
        {
            var lineSpan = node.LineSpan();
            return new FileTextRange {
                Start = node.Span.Start + 1, // TODO: +1  !!!!!
                StartLine = lineSpan.StartLinePosition.Line,
                StartColumn = lineSpan.StartLinePosition.Character,
                End = node.Span.End + 1,
                EndLine = lineSpan.EndLinePosition.Line,
                EndColumn = lineSpan.EndLinePosition.Character
            };
        }


        public static FileLinePositionSpan LineSpan(this SyntaxReference node) => node.SyntaxTree.GetLineSpan(node.Span);

        public static FileTextRange ToFileTextRange(this SyntaxReference node)
        {
            var lineSpan = node.LineSpan();
            return new FileTextRange {
                Start = node.Span.Start + 1,
                StartLine = lineSpan.StartLinePosition.Line,
                StartColumn = lineSpan.StartLinePosition.Character,
                End = node.Span.End + 1,
                EndLine = lineSpan.EndLinePosition.Line,
                EndColumn = lineSpan.EndLinePosition.Character
            };
        }


        public static FileLinePositionSpan LineSpan(this SyntaxToken node) => node.SyntaxTree.GetLineSpan(node.Span);

        public static FileTextRange ToFileTextRange(this SyntaxToken node)
        {
            var lineSpan = node.LineSpan();
            return new FileTextRange {
                Start = node.Span.Start + 1,
                StartLine = lineSpan.StartLinePosition.Line,
                StartColumn = lineSpan.StartLinePosition.Character,
                End = node.Span.End + 1,
                EndLine = lineSpan.EndLinePosition.Line,
                EndColumn = lineSpan.EndLinePosition.Character
            };
        }

    }
}
