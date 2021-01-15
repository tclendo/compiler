using System;
using System.Collections.Generic;
using Forte.CodeAnalysis.Syntax;

namespace Forte.CodeAnalysis.Binding
{
    internal sealed class Binder {

        /*
            Binder class

            Binds a SyntaxKind object to its bound expression form, so that it's clearer
            what happens to the operands in the SyntaxKind when it's operated on by the
            operators.
        */

        private readonly List<string> _diagnostics = new List<string>();
        public IEnumerable<string> Diagnostics => _diagnostics;

        public BoundExpression BindExpression(ExpressionSyntax syntax) 
        {

            // Casts ExpressionSyntax to a certain Bound Expression based on what kind
            // of expression it is.

            switch (syntax.Kind) 
            {
                // if it's a literal expression, it should just return a bound expression of itself.
                case SyntaxKind.LiteralExpression:
                    return BindLiteralExpression((LiteralExpressionSyntax)syntax);
                // if it's a unary expression, it should return a bound expression of an operand and an operator
                case SyntaxKind.UnaryExpression:
                    return BindUnaryExpression((UnaryExpressionSyntax)syntax);
                // if it's a binary expression, it should return a bound expression of a left, right and an operator
                case SyntaxKind.BinaryExpression:
                    return BindBinaryExpression((BinaryExpressionSyntax)syntax);
                // if it's some other kind of expression, throw an exception
                default:
                    throw new Exception($"Unexpected syntax{syntax.Kind}");
            }
        }

        private BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax)

            /*
                Binder.BindLiteralExpression

                Binds a LiteralExpressionSytnax to its bound form.
            */

        {
            var value = syntax.Value ?? 0; // if null, value = 0
            return new BoundLiteralExpression(value);
        }

        private BoundExpression BindUnaryExpression(UnaryExpressionSyntax syntax)

            /*
                Binder.BindUnaryExpression

                Binds a UnaryExpressionSyntax to its bound form.
            */

        {
            var boundOperand = BindExpression(syntax.Operand);
            var boundOperator = BoundUnaryOperator.Bind(syntax.OperatorToken.Kind, boundOperand.Type);

            if (boundOperator == null) {

                _diagnostics.Add($"Unary operator '{syntax.OperatorToken.Text}' is not defined for type {boundOperand.Type}");
                return boundOperand;
            }

            return new BoundUnaryExpression(boundOperator, boundOperand);
        }

        private BoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax)

            /*
                Binder.BindBinaryExpression

                Binds a BindBinaryExpression to its bound form.
            */
        {
            var boundLeft = BindExpression(syntax.Left);
            var boundRight = BindExpression(syntax.Right);
            var boundOperator = BoundBinaryOperator.Bind(syntax.OperatorToken.Kind, boundLeft.Type, boundRight.Type);

            if (boundOperator == null) {

                _diagnostics.Add($"Binary operator '{syntax.OperatorToken.Text}' is not defined for type {boundLeft.Type} and {boundRight.Type}");
                return boundLeft;
            }

            return new BoundBinaryExpression(boundLeft, boundOperator, boundRight);
        }
    }
}