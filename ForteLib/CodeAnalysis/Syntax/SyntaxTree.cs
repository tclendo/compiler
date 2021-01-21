using System.Collections.Generic;
using System.Linq;

namespace Forte.CodeAnalysis.Syntax
{
    public sealed class SyntaxTree {

        /*
            Our syntax tree class

            This will hold our parsed syntax tree for output,
            along with any diagnostics data, the root, and eof token.
        */

        public SyntaxTree(IEnumerable<Diagnostic> diagnostics, ExpressionSyntax root, SyntaxToken endOfFileToken) {

            /*
                SyntaxTree constructor
            */

            Diagnostics = diagnostics.ToArray();
            Root = root;
            EndOfFileToken = endOfFileToken;
        }

        public IReadOnlyList<Diagnostic> Diagnostics { get; }
        public ExpressionSyntax Root { get; }
        public SyntaxToken EndOfFileToken { get; }

        public static SyntaxTree Parse(string text) {

            /*
                Parse

                Creates an instance of the parser class to generate a syntax tree
            */

            var parser = new Parser(text);
            return parser.Parse();
        }

        public static IEnumerable<SyntaxToken> ParseTokens(string text) {

            var lexer = new Lexer(text);
            while (true) {

                var token = lexer.Lex();
                if (token.Kind == SyntaxKind.EndOfFileToken) {

                    break;
                }

                yield return token;
            }
        }
    }
}