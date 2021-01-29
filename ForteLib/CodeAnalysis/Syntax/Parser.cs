using System.Collections.Generic;
using System.Collections.Immutable;
using Forte.CodeAnalysis.Text;

namespace Forte.CodeAnalysis.Syntax
{

    internal sealed class Parser
    {

        /*
            Our parser class

            Contains helper functions to convert our tokenized input into a parse tree.
            Working on a recursive descent parser

            Read tokens -> Create parse tree
        */

        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();
        private readonly SourceText _text;

        private readonly ImmutableArray<SyntaxToken> _tokens;

        private int _position;

        public Parser(SourceText text)
        {

            var tokens = new List<SyntaxToken>();
            var lexer = new Lexer(text);

            SyntaxToken token;

            do {

                token = lexer.Lex();

                if (token.Kind != SyntaxKind.WhitespaceToken &&
                    token.Kind != SyntaxKind.BadToken) {

                        tokens.Add(token);
                    }

            } while (token.Kind != SyntaxKind.EndOfFileToken);
            
            _text = text;
            _tokens = tokens.ToImmutableArray();
            _diagnostics.AddRange(lexer.Diagnostics);
        }

        public DiagnosticBag Diagnostics => _diagnostics;

        private SyntaxToken Peek(int offset)
        {

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

        private SyntaxToken NextToken()
        {

            /*
                NextToken

                Increase the position of 
            */

            var current = Current;
            _position++;
            return current;
        }

        private SyntaxToken MatchToken(SyntaxKind kind)
        {

            /*
                MatchToken

                Check to see if the current token matches the input token
            */

            if (Current.Kind == kind) {

                return NextToken();
            }

            // add an error message to diagnostics if it's a different token than expected
            _diagnostics.ReportUnexpectedToken(Current.Span, Current.Kind, kind);
            
            // return 
            return new SyntaxToken(kind, Current.Position, null, null);
        }

        public CompilationUnitSyntax ParseCompilationUnit()
        {
            
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
            return new CompilationUnitSyntax(expression, endOfFileToken);
        }

        private ExpressionSyntax ParseExpression()
        {

            return ParseAssignmentExpression();
        }

        private ExpressionSyntax ParseAssignmentExpression()
        {

            if (Peek(0).Kind == SyntaxKind.IdentifierToken &&
                Peek(1).Kind == SyntaxKind.EqualsToken)
            {
                var identifierToken = NextToken();
                var operatorToken = NextToken();
                var right = ParseAssignmentExpression();
                return new AssignmentExpressionSyntax(identifierToken, operatorToken, right);

            }

            return ParseBinaryExpression();
        }

        private ExpressionSyntax ParseBinaryExpression(int parentPrecedence = 0)
        {

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
                var operand = ParseBinaryExpression(unaryOperatorPrecedence);
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
                var right = ParseBinaryExpression(precedence);
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
                    return ParseParenthesizedExpression();
                case SyntaxKind.FalseKeyword:
                case SyntaxKind.TrueKeyword:
                    return ParseBooleanLiteral();
                case SyntaxKind.NumberToken:
                    return ParseNumberLiteral();
                case SyntaxKind.IdentifierToken:
                default:
                    return ParseNameExpression();
            }
        }

        private ExpressionSyntax ParseParenthesizedExpression()
        {
            var left = MatchToken(SyntaxKind.OpenParenthesisToken);
            var expression = ParseExpression();
            var right = MatchToken(SyntaxKind.CloseParenthesisToken);
            return new ParenthesizedExpressionSyntax(left, expression, right);
        }

        private ExpressionSyntax ParseBooleanLiteral()
        {
            var isTrue = Current.Kind == SyntaxKind.TrueKeyword;
            var keywordToken = isTrue ? MatchToken(SyntaxKind.TrueKeyword) : MatchToken(SyntaxKind.FalseKeyword);
            return new LiteralExpressionSyntax(keywordToken, isTrue);
        }

        private ExpressionSyntax ParseNameExpression()
        {
            var identifierToken = MatchToken(SyntaxKind.IdentifierToken);
            return new NameExpressionSyntax(identifierToken);
        }
        
        private ExpressionSyntax ParseNumberLiteral()
        {
            var numberToken = MatchToken(SyntaxKind.NumberToken);
            return new LiteralExpressionSyntax(numberToken);
        }
    }
}