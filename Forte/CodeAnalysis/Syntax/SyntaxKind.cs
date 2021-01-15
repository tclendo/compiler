namespace Forte.CodeAnalysis.Syntax
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
        IdentifierToken,

        // Expressions
        LiteralExpression,
        BinaryExpression,
        ParenthesizedExpressionSyntax,
        UnaryExpression,

        // Keywords
        FalseKeyword,
        TrueKeyword,
    }
}