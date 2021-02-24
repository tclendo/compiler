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
        TildeToken,
        AmpersandToken,
        AmpersandAmpersandToken,// "&"
        PipeToken,
        PipePipeToken,          // "|"
        HatToken,
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
        ForKeyword,
        IfKeyword,
        TrueKeyword,            // "false"
        LetKeyword,
        ToKeyword,
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
        ForStatement,
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