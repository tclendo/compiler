namespace Forte.CodeAnalysis.Syntax
{
    internal static class SyntaxFacts {
        
        public static int GetUnaryOperatorPrecedence(this SyntaxKind kind) {

            /*
                Helps get precedence of different operators
            */
            
            switch (kind) {

                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 3;
                
                default:
                    return 0;
            }
        }

        public static int GetBinaryOperatorPrecedence(this SyntaxKind kind) {

            /*
                Helps get precedence of different operators
            */
            
            switch (kind) {

                case SyntaxKind.StarToken:
                case SyntaxKind.SlashToken:
                    return 2;

                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 1;
                
                default:
                    return 0;
            }
        }
    }
}