using System.Collections.Generic;

namespace Forte.CodeAnalysis
{
    sealed class NumberExpressionSyntax : ExpressionSyntax {

        /*
            NumberExpressionSyntax

            A specified syntax expression node for a number token.
        */

        public NumberExpressionSyntax(SyntaxToken numberToken) {

            NumberToken = numberToken;
        }

        public override SyntaxKind Kind => SyntaxKind.NumberExpression;
        public SyntaxToken NumberToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren() {

            yield return NumberToken;
        }

    }
}