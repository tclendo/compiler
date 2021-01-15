using System;
using Forte.CodeAnalysis.Syntax;

namespace Forte.CodeAnalysis.Binding
{
    internal sealed class BoundBinaryOperator {

        /*
            BoundBinaryOperator class

            Binds the results of binary operators between the 2 operands to a type so that we can
            actually show how binary operations affect 2 operands.
        */

        private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type type) 
            : this(syntaxKind, kind, type, type, type)
        {

        }

        private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type operandType, Type resultType) 
            : this(syntaxKind, kind, operandType, operandType, resultType)
        {

        }

        // constructors
        private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type leftType, Type rightType, Type resultType) {
            SyntaxKind = syntaxKind;
            Kind = kind;
            LeftType = leftType;
            RightType = rightType;
            Type = resultType;
        }

        public SyntaxKind SyntaxKind { get; }
        public BoundBinaryOperatorKind Kind { get; }
        public Type LeftType { get; }
        public Type RightType { get; }
        public Type OperandType { get; }
        public Type Type { get; }

        // this is our list of binary operators and information about how they act on their operands
        private static BoundBinaryOperator[] _operators =
        {
            new BoundBinaryOperator(SyntaxKind.PlusToken, BoundBinaryOperatorKind.Addition, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.MinusToken, BoundBinaryOperatorKind.Subtraction, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.StarToken, BoundBinaryOperatorKind.Multiplication, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.SlashToken, BoundBinaryOperatorKind.Division, typeof(int)),

            new BoundBinaryOperator(SyntaxKind.EqualsEqualsToken, BoundBinaryOperatorKind.Equals, typeof(int), typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.BangEqualsToken, BoundBinaryOperatorKind.NotEquals, typeof(int), typeof(bool)),

            new BoundBinaryOperator(SyntaxKind.AmpersandAmpersandToken, BoundBinaryOperatorKind.LogicalAnd, typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.PipePipeToken, BoundBinaryOperatorKind.LogicalOr, typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.EqualsEqualsToken, BoundBinaryOperatorKind.Equals, typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.BangEqualsToken, BoundBinaryOperatorKind.NotEquals, typeof(bool)),

        };

        public static BoundBinaryOperator Bind(SyntaxKind SyntaxKind, Type leftType, Type rightType) {

            /*
                Bind

                Binds a given SyntaxKind, left and right type to a certain binary operator
                in our _operators list. If they match a binary operator, return the bound
                version of it. 
            */
            
            foreach (var op in _operators) {

                if (op.SyntaxKind == SyntaxKind && op.LeftType == leftType && op.RightType == rightType) {

                    return op;
                }
            }

            return null;
        }
    }
}