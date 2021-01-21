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
        NumberToken,
        PlusToken,              // "+"
        MinusToken,             // "-"
        StarToken,              // "*"
        SlashToken,             // "/"
        BangToken,              // "!"
        EqualsToken,            // "="
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