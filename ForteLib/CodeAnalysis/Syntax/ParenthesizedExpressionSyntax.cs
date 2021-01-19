using System.Collections.Generic;

namespace Forte.CodeAnalysis.Syntax
{
    public sealed class ParenthesizedExpressionSyntax : ExpressionSyntax {

        /*
            ParenthesizedExpressionSyntax

            A subclass of ExpressionSyntax that contains an open and closed parenthesis syntax token, as well as
            an expression syntax expression.
        */

        public ParenthesizedExpressionSyntax(SyntaxToken openParenthesisToken, ExpressionSyntax expression, SyntaxToken closedParenthesisToken) {

            OpenParenthesisToken = openParenthesisToken;
            Expression = expression;
            ClosedParenthesisToken = closedParenthesisToken;
        }

        public override SyntaxKind Kind => SyntaxKind.ParenthesizedExpressionSyntax;
        public SyntaxToken OpenParenthesisToken { get; }
        public ExpressionSyntax Expression { get; }
        public SyntaxToken ClosedParenthesisToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren() {

            yield return OpenParenthesisToken;
            yield return Expression;
            yield return ClosedParenthesisToken;
        }
    }
}