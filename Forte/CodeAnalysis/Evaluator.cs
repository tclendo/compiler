using System;

namespace Forte.CodeAnalysis
{
    public sealed class Evaluator {

        /*
            Our Evaluator class

            This class contains functions that perform mathematical operations on our parse tree
            given a root node of that parse tree. 
        */

        public readonly ExpressionSyntax _root;


        public Evaluator(ExpressionSyntax root) {

            /*
                Constructor for our evaluator class
                Initialize the root node.
            */

            this._root = root;
        }

        public int Evaluate() {

            /*
                Evaluate

                Evaluate the entire tree and return a value.
            */

            return EvaluateExpression(_root);
        }

        private int EvaluateExpression(ExpressionSyntax node) {

            /*
                EvaluateExpression

                Returns an integer by recursively operating on nodes of the parse tree.

                LiteralExpressionSyntax
                BinaryExpressionSyntax
                ParenthesizedExpressionSyntax
            */

            // if the current node is just a number, return the integer value of it
            if (node is LiteralExpressionSyntax n) {

                return (int) n.LiteralToken.Value;
            }

            if (node is UnaryExpressionSyntax u) {

                var operand = EvaluateExpression(u.Operand);

                if (u.OperatorToken.Kind == SyntaxKind.PlusToken) {

                    return operand;
                } else if (u.OperatorToken.Kind == SyntaxKind.MinusToken) {

                    return -operand;
                } else {

                    throw new Exception($"Unexpected unary operator {u.OperatorToken.Kind}");
                }
            }
            // if the node is a binary expression syntax node, recursively evaluate the left and right operands
            if (node is BinaryExpressionSyntax b) {

                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);

                // addition
                if (b.OperatorToken.Kind == SyntaxKind.PlusToken) {
                    return left + right;
                // subtraction
                } else if (b.OperatorToken.Kind == SyntaxKind.MinusToken) {
                    return left - right;
                // multiplication
                } else if (b.OperatorToken.Kind == SyntaxKind.StarToken) {
                    return left * right;
                // division
                } else if (b.OperatorToken.Kind == SyntaxKind.SlashToken) {
                    return left / right;
                // binary operator not recognized
                } else {

                    throw new Exception($"Unexpected binary operator {b.OperatorToken.Kind}");
                }
            }

            // recursively call EvaluateExpression when we reach a parenthesized expression
            if (node is ParenthesizedExpressionSyntax p) {

                return EvaluateExpression(p.Expression);
            }

            // if all cases failed, throw an unexpected node exception
            throw new Exception($"Unexpected node {node.Kind}");
        }
    }
}