using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly Dictionary<VariableSymbol, object> _variables;
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();

        public Binder(Dictionary<VariableSymbol, object> variables) {

            _variables = variables;
        }

        public DiagnosticBag Diagnostics => _diagnostics;

        public BoundExpression BindExpression(ExpressionSyntax syntax) 
        {

            // Casts ExpressionSyntax to a certain Bound Expression based on what kind
            // of expression it is.

            switch (syntax.Kind) 
            {
                // if it's a parenthesized expression, return a parenthesized expression
                case SyntaxKind.ParenthesizedExpressionSyntax:
                    return BindParenthesizedExpression((ParenthesizedExpressionSyntax)syntax);                
                // if it's a literal expression, it should just return a bound expression of itself.
                case SyntaxKind.LiteralExpression:
                    return BindLiteralExpression((LiteralExpressionSyntax)syntax);
                case SyntaxKind.NameExpression:
                    return BindNameExpression((NameExpressionSyntax)syntax);
                case SyntaxKind.AssignmentExpression:
                    return BindAssignmentExpression((AssignmentExpressionSyntax)syntax);
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

        private BoundExpression BindParenthesizedExpression(ParenthesizedExpressionSyntax syntax)
        {
            return BindExpression(syntax.Expression);
        }

        private BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax) {

            /*
                Binder.BindLiteralExpression

                Binds a LiteralExpressionSytnax to its bound form.
            */
        
            var value = syntax.Value ?? 0; // if null, value = 0
            return new BoundLiteralExpression(value);
        }

        private BoundExpression BindNameExpression(NameExpressionSyntax syntax)
        {
            var name = syntax.IdentifierToken.Text;

            var variable = _variables.Keys.FirstOrDefault(v => v.Name == name);
            
            if (variable == null) {

                _diagnostics.ReportUndefinedName(syntax.IdentifierToken.Span, name);
                return new BoundLiteralExpression(0); 
            }

            return new BoundVariableExpression(variable);
        }

        private BoundExpression BindAssignmentExpression(AssignmentExpressionSyntax syntax)
        {
            var name = syntax.IdentifierToken.Text;
            var boundExpression = BindExpression(syntax.Expression);
            
            var existingVariable = _variables.Keys.FirstOrDefault(v => v.Name == name);
            if (existingVariable != null) {

                _variables.Remove(existingVariable);
            }

            var variable = new VariableSymbol(name, boundExpression.Type);
            _variables[variable] = null;

            return new BoundAssignmentExpression(variable, boundExpression);
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

                _diagnostics.ReportUndefinedUnaryOperator(syntax.OperatorToken.Span, syntax.OperatorToken.Text, boundOperand.Type);
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

                _diagnostics.ReportUndefinedBinaryOperator(syntax.OperatorToken.Span, syntax.OperatorToken.Text, boundLeft.Type, boundRight.Type);
                return boundLeft;
            }

            return new BoundBinaryExpression(boundLeft, boundOperator, boundRight);
        }
    }
}