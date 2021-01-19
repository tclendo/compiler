using System.Collections.Generic;
using System.Linq;

namespace Forte.CodeAnalysis.Syntax
{
    public class SyntaxToken : SyntaxNode {
        
        /*
            Our syntax token class

            Stores data from input text into tokens.
        */
        
        public SyntaxToken(SyntaxKind kind, int position, string text, object value) {

            Kind = kind;
            Position = position;
            Text = text;
            Value = value;
        }

        public override SyntaxKind Kind { get; }
        public int Position { get; }
        public string Text { get; }
        public object Value { get; }
        public TextSpan Span => new TextSpan(Position, Text.Length);

        public override IEnumerable<SyntaxNode> GetChildren() {
            return Enumerable.Empty<SyntaxNode>();
        }
    }
}