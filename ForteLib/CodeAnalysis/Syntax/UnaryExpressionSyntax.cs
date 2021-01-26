using System.Collections.Generic;

namespace Forte.CodeAnalysis.Syntax
{
    public sealed class UnaryExpressionSyntax : ExpressionSyntax {

        /*
            UnaryExpressionSyntax

            This is a specified syntax ExpressionSyntax node class that contains information
            for binary expressions (i.e. a + b). Contains the left and right operands, as well
            as the operatorToken.
        */

        public UnaryExpressionSyntax(SyntaxToken operatorToken, ExpressionSyntax operand) {

            OperatorToken = operatorToken;
            Operand = operand;
        }

        public override SyntaxKind Kind => SyntaxKind.UnaryExpression;
        public SyntaxToken OperatorToken { get; }
        public ExpressionSyntax Operand { get; }
    }
}