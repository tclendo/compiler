namespace Forte.CodeAnalysis.Binding
{
    internal enum BoundNodeKind {

        // statements
        BlockStatement,
        ExpressionStatement,
        
        // expressions
        LiteralExpression,
        VariableExpression,
        AssignmentExpression,
        UnaryExpression,
        BinaryExpression,
    }
}