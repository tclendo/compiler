namespace Forte.CodeAnalysis
{
    enum SyntaxKind {

        /*
            Represents the various tokens that the lexer
            can identify.
        */

        WhitespaceToken,
        NumberToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
        BadToken,
        EndOfFileToken,

        // expressions
        NumberExpression,
        BinaryExpression,
        ParenthesizedExpressionSyntax
    }
}