namespace Forte.CodeAnalysis.Binding
{
    internal enum BoundNodeKind {

        // statements
        BlockStatement,
        ExpressionStatement,
        IfStatement,
        VariableDeclaration,
        
        // expressions
        LiteralExpression,
        VariableExpression,
        AssignmentExpression,
        UnaryExpression,
        BinaryExpression,
    }
}