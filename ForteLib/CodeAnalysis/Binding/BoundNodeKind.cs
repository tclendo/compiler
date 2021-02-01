namespace Forte.CodeAnalysis.Binding
{
    internal enum BoundNodeKind {

        // statements
        BlockStatement,
        ExpressionStatement,
        VariableDeclaration,
        
        // expressions
        LiteralExpression,
        VariableExpression,
        AssignmentExpression,
        UnaryExpression,
        BinaryExpression,
    }
}