using System;

namespace Forte.CodeAnalysis.Binding
{
    internal sealed class BoundBinaryExpression : BoundExpression 
        
        /*
            Our BoundBinaryExpression class

            Contains an instance of a bound binary expression containing a left
            and right expression, as well as it's operator. Also contains a type
            that results from the operator itself.
        */
    {

        public BoundBinaryExpression(BoundExpression left, BoundBinaryOperator op, BoundExpression right) {
            Left = left;
            Op = op;
            Right = right;
        }

        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        public override Type Type => Op.Type;

        public BoundExpression Left { get; }
        public BoundBinaryOperator Op { get; }
        public BoundExpression Right { get; }
    }
}