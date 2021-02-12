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
        OpenBraceToken,
        CloseBraceToken,
        IdentifierToken,        //
        LessToken,
        LessOrEqualsToken,
        GreaterToken,
        GreaterOrEqualsToken,

        // Keywords
        ElseKeyword,
        FalseKeyword,           // "true"
        IfKeyword,
        TrueKeyword,            // "false"
        LetKeyword,
        VarKeyword,
        WhileKeyword,
                
        // Nodes
        CompilationUnit,
        ElseClause,

        // Statements
        BlockStatement,
        VariableDeclaration,
        ifStatement,
        WhileStatement,
        ExpressionStatement,

        // Expressions
        LiteralExpression,  
        NameExpression,
        BinaryExpression,
        ParenthesizedExpressionSyntax,
        UnaryExpression,
        AssignmentExpression,
    }
}