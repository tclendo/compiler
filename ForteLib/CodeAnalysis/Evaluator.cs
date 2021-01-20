using System;
using System.Collections.Generic;
using Forte.CodeAnalysis.Binding;

namespace Forte.CodeAnalysis
{
    internal sealed class Evaluator {

        /*
            Our Evaluator class

            This class contains functions that perform mathematical operations on our parse tree
            given a root node of that parse tree. 
        */

        public readonly BoundExpression _root;
        private readonly Dictionary<VariableSymbol, object> _variables;

        public Evaluator(BoundExpression root, Dictionary<VariableSymbol, object> variables) {

            /*
                Constructor for our evaluator class
                Initialize the root node.
            */

            _root = root;
            _variables = variables;
        }

        public object Evaluate() {

            /*
                Evaluate

                Evaluate the entire tree and return a value.
            */

            return EvaluateExpression(_root);
        }

        private object EvaluateExpression(BoundExpression node) {

            /*
                EvaluateExpression

                Returns an integer by recursively operating on nodes of the parse tree.

                LiteralExpressionSyntax
                BinaryExpressionSyntax
                ParenthesizedExpressionSyntax
            */

            // if the current node is just a number, return the integer value of it
            if (node is BoundLiteralExpression n) {

                return n.Value;
            }

            if (node is BoundVariableExpression v) {

                return _variables[v.Variable];
            }

            if (node is BoundAssignmentExpression a) {

                var value = EvaluateExpression(a.Expression);
                _variables[a.Variable] = value;
                return value; 
            }

            if (node is BoundUnaryExpression u) {

                var operand = EvaluateExpression(u.Operand);

                switch (u.Op.Kind)
                {
                    case BoundUnaryOperatorKind.Identity:
                        return (int) operand;
                    case BoundUnaryOperatorKind.Negation:
                        return -(int) operand;
                    case BoundUnaryOperatorKind.LogicalNegation:
                        return !(bool) operand;
                    default:
                        throw new Exception($"Unexpected unary operator {u.Op}");
                }
            }

            // if the node is a binary expression syntax node, recursively evaluate the left and right operands
            if (node is BoundBinaryExpression b) {

                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);

                switch (b.Op.Kind)
                {
                    case BoundBinaryOperatorKind.Addition:
                        return (int) left + (int) right;
                    case BoundBinaryOperatorKind.Subtraction:
                        return (int) left - (int) right;
                    case BoundBinaryOperatorKind.Multiplication:
                        return (int) left * (int) right;
                    case BoundBinaryOperatorKind.Division:
                        return (int) left / (int) right;
                    case BoundBinaryOperatorKind.LogicalAnd:
                        return (bool) left && (bool) right;
                    case BoundBinaryOperatorKind.LogicalOr:
                        return (bool) left || (bool) right;
                    case BoundBinaryOperatorKind.Equals:
                        return Equals(left, right);
                    case BoundBinaryOperatorKind.NotEquals:
                        return !Equals(left, right);
                    default:
                        throw new Exception($"Unexpected binary operator {b.Op}");
                }
            }

            // if all cases failed, throw an unexpected node exception
            throw new Exception($"Unexpected node {node.Kind}");
        }
    }
}