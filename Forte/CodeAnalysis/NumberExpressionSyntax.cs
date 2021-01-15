using System.Collections.Generic;

namespace Forte.CodeAnalysis
{
    public sealed class LiteralExpressionSyntax : ExpressionSyntax {

        /*
            LiteralExpressionSyntax

            A specified syntax expression node for a number token.
        */

        public LiteralExpressionSyntax(SyntaxToken literalToken) {

            LiteralToken = literalToken;
        }

        public override SyntaxKind Kind => SyntaxKind.LiteralExpression;
        public SyntaxToken LiteralToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren() {

            yield return LiteralToken;
        }

    }
}