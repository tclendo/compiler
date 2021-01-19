namespace Forte.CodeAnalysis.Syntax
{
    public enum SyntaxKind {

        /*
            Represents the various tokens that the lexer
            can identify.
        */

        // Tokens
        BadToken,               //
        WhitespaceToken,        // " "
        EndOfFileToken,         // "\0"
        LiteralToken,           //
        PlusToken,              // "+"
        MinusToken,             // "-"
        StarToken,              // "*"
        SlashToken,             // "/"
        BangToken,              // "!"
        AmpersandAmpersandToken,// "&"
        PipePipeToken,          // "|"
        EqualsEqualsToken,      // "=="
        BangEqualsToken,        // "!="
        OpenParenthesisToken,   // "("
        CloseParenthesisToken,  // ")"
        IdentifierToken,        //

        // Keywords
        FalseKeyword,           // "true"
        TrueKeyword,            // "false"
        
        // Expressions
        LiteralExpression,
        NameExpression,
        BinaryExpression,
        ParenthesizedExpressionSyntax,
        UnaryExpression,
        AssignmentExpression,
    }
}