namespace Forte.CodeAnalysis.Binding
{
    internal enum BoundBinaryOperatorKind {

        // List of possible bound binary operator kinds
        
        Addition,
        Subtraction,
        Multiplication,
        Division,
        LogicalAnd,
        LogicalOr,
        BitwiseAnd,
        BitwiseOr,
        BitwiseXor,
        Equals,
        NotEquals,
        Less,
        LessOrEquals,
        Greater,
        GreaterOrEquals,
    }
}