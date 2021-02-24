using System;
using Forte.CodeAnalysis.Syntax;

namespace Forte.CodeAnalysis.Binding
{
    internal sealed class BoundUnaryOperator {

        /*
            BoundUnaryOperator class

            Binds the results of unary operators between the operand and its operator to a type so that we can
            actually show how unary operations affect the operand.
        */

        private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, Type operandType)
            : this(syntaxKind, kind, operandType, operandType)
        {

        }
    
        // constructor
        private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, Type operandType, Type resultType) {
            SyntaxKind = syntaxKind;
            Kind = kind;
            OperandType = operandType;
            Type = resultType;
        }

        public SyntaxKind SyntaxKind { get; }
        public BoundUnaryOperatorKind Kind { get; }
        public Type OperandType { get; }
        public Type Type { get; }

        // here is our list of unary operators
        private static BoundUnaryOperator[] _operators =
        {
            new BoundUnaryOperator(SyntaxKind.BangToken, BoundUnaryOperatorKind.LogicalNegation, typeof(bool)),

            new BoundUnaryOperator(SyntaxKind.PlusToken, BoundUnaryOperatorKind.Identity, typeof(int)),
            new BoundUnaryOperator(SyntaxKind.MinusToken, BoundUnaryOperatorKind.Negation, typeof(int)),

            new BoundUnaryOperator(SyntaxKind.TildeToken, BoundUnaryOperatorKind.OnesCompliment, typeof(int)),
        };

        public static BoundUnaryOperator Bind(SyntaxKind SyntaxKind, Type operandType) {
            
            /*
                BoundUnaryOperator.Bind

                Binds a given SyntaxKind and it's operand type, to an operator within
                our unary operator list. Returns the bound operator when it is finalized,
                else null.
            */

            foreach (var op in _operators) {

                if (op.SyntaxKind == SyntaxKind && op.OperandType == operandType) {

                    return op;
                }
            }

            return null;
        }
    }
}