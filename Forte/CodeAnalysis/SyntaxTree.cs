using System.Collections.Generic;
using System.Linq;

namespace Forte.CodeAnalysis
{
    sealed class SyntaxTree {

        /*
            Our syntax tree class

            This will hold our parsed syntax tree for output,
            along with any diagnostics data, the root, and eof token.
        */

        public SyntaxTree(IEnumerable<string> diagnostics, ExpressionSyntax root, SyntaxToken endOfFileToken) {

            /*
                SyntaxTree constructor
            */

            Diagnostics = diagnostics.ToArray();
            Root = root;
            EndOfFileToken = endOfFileToken;
        }

        public IReadOnlyList<string> Diagnostics { get; }
        public ExpressionSyntax Root { get; }
        public SyntaxToken EndOfFileToken { get; }

        public static SyntaxTree ParseTree(string text) {

            /*
                ParseTree

                Creates an instance of the parser class to generate a syntax tree
            */

            var parser = new Parser(text);
            return parser.Parse();
        }
    }
}