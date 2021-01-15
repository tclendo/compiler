using System;
using Forte.CodeAnalysis.Binding;
using Forte.CodeAnalysis.Syntax;

namespace Forte.CodeAnalysis
{
    internal sealed class Evaluator {

        /*
            Our Evaluator class

            This class contains functions that perform mathematical operations on our parse tree
            given a root node of that parse tree. 
        */

        public readonly BoundExpression _root;

        public Evaluator(BoundExpression root) {

            /*
                Constructor for our evaluator class
                Initialize the root node.
            */

            _root = root;
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

            if (node is BoundUnaryExpression u) {

                var operand = (int) EvaluateExpression(u.Operand);

                switch (u.OperatorKind)
                {
                    case BoundUnaryOperatorKind.Identity:
                        return operand;
                    case BoundUnaryOperatorKind.Negation:
                        return -operand;
                    default:
                        throw new Exception($"Unexpected unary operator {u.OperatorKind}");
                }
            }

            // if the node is a binary expression syntax node, recursively evaluate the left and right operands
            if (node is BoundBinaryExpression b) {

                var left = (int) EvaluateExpression(b.Left);
                var right = (int) EvaluateExpression(b.Right);

                switch (b.OperatorKind)
                {
                    case BoundBinaryOperatorKind.Addition:
                        return left + right;
                    case BoundBinaryOperatorKind.Subtraction:
                        return left - right;
                    case BoundBinaryOperatorKind.Multiplication:
                        return left * right;
                    case BoundBinaryOperatorKind.Division:
                        return left / right;
                    default:
                        throw new Exception($"Unexpected binary operator {b.OperatorKind}");
                }
            }

            // if all cases failed, throw an unexpected node exception
            throw new Exception($"Unexpected node {node.Kind}");
        }
    }
}