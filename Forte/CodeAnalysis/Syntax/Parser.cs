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
            var lexer = new Lexer(text);

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
            */

            var expression = ParseExpression();
            var endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);
            return new SyntaxTree(_diagnostics, expression, endOfFileToken);
        }
        
        private ExpressionSyntax ParseExpression(int parentPrecedence = 0) {

            /*
                Parser.ParseExpression

                Helper function that parses our parse tree starting at root precedence of 0.

                Takes into account our definitions of order of operations (found in SyntaxFacts).
            */

            ExpressionSyntax left;

            // get the current unary operator precedence, if it doesn't equal 0, it does indeed
            // have a unary operator prescedence. However, only get the next token and parse
            // the expression if it has a higher precedence than its parent (otherwise, we want
            // to act on other operators first)
            var unaryOperatorPrecedence = Current.Kind.GetUnaryOperatorPrecedence();
            if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence) {

                var operatorToken = NextToken();
                var operand = ParseExpression(unaryOperatorPrecedence);
                left = new UnaryExpressionSyntax(operatorToken, operand);

            }
            // else, the current kind is not a unary operator, but rather a primary literal expression.
            else 
            {

                left = ParsePrimaryExpression();
            }

            // iterate through what should be binary operator tokens until the precedence is less significant
            // than its parent, thus returning an ExpressionSyntax of left. otherwise, put the operator token into a variable,
            // and call ParseExpression for the right sub tree, with the current operator's precedence as its parent precedence.
            while (true) {

                var precedence = Current.Kind.GetBinaryOperatorPrecedence();
                if (precedence == 0 || precedence <= parentPrecedence) {

                    break;
                }

                var operatorToken = NextToken();    // we call nexttoken because we want our parser to continuously parse input.
                var right = ParseExpression(precedence);
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax ParsePrimaryExpression() {

            /*
                ParsePrimaryExpression

                Checks if the current kind has parenthesis, and if it does, then creates a
                left/expression/right syntaxnode within the parenthesis semi-recursively.

                Otherwise, it should MatchToken a literal (number, true, or false so far),
                since we have gone through ParseTerm, and ParseFactor for operands,
            */

            switch (Current.Kind)
            {
                // todo: change how parentheses work i think?
                case SyntaxKind.OpenParenthesisToken:
                {
                    var left = NextToken();
                    var expression = ParseExpression();
                    var right = MatchToken(SyntaxKind.CloseParenthesisToken);
                    return new ParenthesizedExpressionSyntax(left, expression, right);
                }

                // if the cases are true or false keywords, their value is true or false
                case SyntaxKind.FalseKeyword:
                case SyntaxKind.TrueKeyword:
                {
                    var keywordToken = NextToken();
                    var value = keywordToken.Kind == SyntaxKind.TrueKeyword;
                    return new LiteralExpressionSyntax(keywordToken, value);
                }

                // default case is that the literal has a number value, and is a number token.
                // we still check that it is, however.
                default: 
                {
                    var numberToken = MatchToken(SyntaxKind.LiteralToken);
                    return new LiteralExpressionSyntax(numberToken);
                }
            }
        }
    }
}