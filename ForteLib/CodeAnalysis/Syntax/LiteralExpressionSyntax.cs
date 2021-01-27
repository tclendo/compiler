using System.Collections.Generic;

namespace Forte.CodeAnalysis.Syntax
{
    public sealed class LiteralExpressionSyntax : ExpressionSyntax {

        /*
            LiteralExpressionSyntax

            A specified syntax expression node for a number token.
        */

        public LiteralExpressionSyntax(SyntaxToken literalToken) 
            : this(literalToken, literalToken.Value)   
        
        {

        }

        public LiteralExpressionSyntax(SyntaxToken literalToken, object value) {

            LiteralToken = literalToken;
            Value = value;
        }

        public override SyntaxKind Kind => SyntaxKind.LiteralExpression;
        public SyntaxToken LiteralToken { get; }
        public object Value { get; }
    }
}