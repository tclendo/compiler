using System;

namespace Forte.CodeAnalysis.Binding
{
    internal sealed class BoundUnaryExpression : BoundExpression {

        /*
            BoundUnaryExpression class

            Constructs a bound unary expression instance that contains the operator,
            operand. Also contains a type that results from the operator.
        */

        public BoundUnaryExpression(BoundUnaryOperator op, BoundExpression operand) {
            Op = op;
            Operand = operand;
        }

        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        public override Type Type => Op.Type;

        public BoundUnaryOperator Op { get; }
        public BoundExpression Operand { get; }
    }
}