using System.Collections.Generic;

namespace Forte.CodeAnalysis.Syntax
{

    internal sealed class Parser {

        /*
            Our parser class

            Contains helper functions to convert our tokenized input into a parse tree.
            Working on a recursive descent parser

            Read tokens -> Create parse tree
        */

        private readonly SyntaxToken[] _tokens;
        private int _position;
        private List<string> _diagnostics = new List<string>();

        public Parser(string text) {

            /*
                Parser constructor

                Initializes the list of _tokens and _diagnostics
            */

            var tokens = new List<SyntaxToken>();
            var lexer = new NextTokener(text);

            SyntaxToken token;

            // go through the text until end of file token to create a list of tokens
            do {

                // get the next token from our lexer
                token = lexer.NextToken();

                // as long as the token is good, add it to our tokens list
                if (token.Kind != SyntaxKind.WhitespaceToken &&
                    token.Kind != SyntaxKind.BadToken) {

                        tokens.Add(token);
                    }

            } while (token.Kind != SyntaxKind.EndOfFileToken);

            // copy our tokens array to our instance variable _tokens
            _tokens = tokens.ToArray();
            // add any diagnostics info to our diagnostics
            _diagnostics.AddRange(lexer.Diagnostics);
        }

        public IEnumerable<string> Diagnostics => _diagnostics;

        private SyntaxToken Peek(int offset) {

            /*
                Peek

                Returns a token from the token list by accessing it's _position + an offset
            */

            var index = _position + offset;

            // if the index is equal to or more than the token list length, return the last token in the list
            if (index >= _tokens.Length) {

                return _tokens[_tokens.Length - 1];
            }

            // otherwise, return the token at the index
            return _tokens[index];
        }

        private SyntaxToken Current => Peek(0);

        private SyntaxToken NextToken() {

            /*
                NextToken

                Increase the position of 
            */

            var current = Current;
            _position++;
            return current;
        }

        private SyntaxToken MatchToken(SyntaxKind kind) {

            /*
                MatchToken

                Check to see if the current token matches the input token
            */

            if (Current.Kind == kind) {

                return NextToken();
            }

            // add an error message to diagnostics if it's a different token than expected
            _diagnostics.Add($"ERROR: Unexpected token <{Current.Kind}>, expected <{kind}>");
            
            // return 
            return new SyntaxToken(kind, Current.Position, null, null);
        }

        public SyntaxTree Parse() {
            
            /*
                Parse

                Main functon to parse our input, takes into account
                some order of operations thanks to helper parse functions.

                The function ParseTerm and it's helper functions are structured in a way that maintains
                order of operation and hierarchy of operators.

                Returns a SyntaxTree

                CHANGED AFTER EPISODE 2
            */

            var expression = ParseExpression();
            var endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);
            return new SyntaxTree(_diagnostics, expression, endOfFileToken);
        }
        
        private ExpressionSyntax ParseExpression(int parentPrecedence = 0) {

            ExpressionSyntax left;

            var unaryOperatorPrecedence = Current.Kind.GetUnaryOperatorPrecedence();
            if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence) {

                var operatorToken = NextToken();
                var operand = ParseExpression(unaryOperatorPrecedence);
                left = new UnaryExpressionSyntax(operatorToken, operand);

            } else {

                left = ParsePrimaryExpression();
            }

            while (true) {

                var precedence = Current.Kind.GetBinaryOperatorPrecedence();
                if (precedence == 0 || precedence <= parentPrecedence) {

                    break;
                }

                var operatorToken = NextToken();
                var right = ParseExpression(precedence);
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax ParsePrimaryExpression() {

            /*
                ParsePrimaryExpression

                Checks if the current kind has parenthesis, and then creates a
                left/expression/right syntaxnode within the parenthesis semi-recursively.

                Otherwise, it should MatchToken a number input, since we have gone through
                ParseTerm, and ParseFactor for operands, otherwise it will add to
                diagnostics that it expected a number.
            */

            if (Current.Kind == SyntaxKind.OpenParenthesisToken) {

                var left = NextToken();
                var expression = ParseExpression();
                var right = MatchToken(SyntaxKind.CloseParenthesisToken);
                return new ParenthesizedExpressionSyntax(left, expression, right);
            }
            var literalToken = MatchToken(SyntaxKind.LiteralToken);
            return new LiteralExpressionSyntax(literalToken);
        }
    }
}