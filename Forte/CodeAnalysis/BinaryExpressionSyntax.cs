using System.Collections.Generic;

namespace Forte.CodeAnalysis
{
    sealed class BinaryExpressionSyntax : ExpressionSyntax {

        /*
            BinaryExpressionSyntax

            This is a specified syntax ExpressionSyntax node class that contains information
            for binary expressions (i.e. a + b). Contains the left and right operands, as well
            as the operatorToken.
        */

        public BinaryExpressionSyntax(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right) {

            Left = left;
            OperatorToken = operatorToken;
            Right = right;
        }

        public override SyntaxKind Kind => SyntaxKind.BinaryExpression;
        public ExpressionSyntax Left { get; }
        public SyntaxToken OperatorToken { get; }
        public ExpressionSyntax Right { get; }

        public override IEnumerable<SyntaxNode> GetChildren() {

            yield return Left;
            yield return OperatorToken;
            yield return Right;
        }
    }
}