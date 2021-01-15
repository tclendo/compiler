namespace Forte.CodeAnalysis
{
    public enum SyntaxKind {

        /*
            Represents the various tokens that the lexer
            can identify.
        */

        // Tokens
        BadToken,
        WhitespaceToken,
        EndOfFileToken,
        LiteralToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        OpenParenthesisToken,
        CloseParenthesisToken,

        // Expressions
        LiteralExpression,
        BinaryExpression,
        ParenthesizedExpressionSyntax,
        UnaryExpression
    }
}