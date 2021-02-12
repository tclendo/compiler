namespace Forte.CodeAnalysis.Binding
{
    internal enum BoundNodeKind {

        // statements
        BlockStatement,
        ExpressionStatement,
        IfStatement,
        WhileStatement,
        ForStatement,
        VariableDeclaration,
        
        // expressions
        LiteralExpression,
        VariableExpression,
        AssignmentExpression,
        UnaryExpression,
        BinaryExpression,
    }
}