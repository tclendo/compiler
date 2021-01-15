using System;

namespace Forte.CodeAnalysis.Binding
{
    internal sealed class BoundLiteralExpression : BoundExpression {

        /*
            Our BoundLiteralExpression class

            Binds a literal expression to its own value.
        */

        public BoundLiteralExpression(object value) {
            Value = value;
        }

        public override BoundNodeKind Kind => BoundNodeKind.LiteralExpression;
        public override Type Type => Value.GetType();
        public object Value { get; }
    }
}