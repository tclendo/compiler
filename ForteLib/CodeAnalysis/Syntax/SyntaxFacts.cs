using System;

namespace Forte.CodeAnalysis.Syntax
{
    internal static class SyntaxFacts {
        
        /*
            SyntaxFacts class

            Gives information about the precedence of various operators
        */
        public static int GetUnaryOperatorPrecedence(this SyntaxKind kind) {

            /*
                Helps get precedence of unary operators
            */
            
            switch (kind) {

                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                case SyntaxKind.BangToken:
                    return 6;
                
                default:
                    return 0;
            }
        }

        public static int GetBinaryOperatorPrecedence(this SyntaxKind kind) {

            /*
                Helps get precedence of binary operators
            */
            
            switch (kind) {

                case SyntaxKind.StarToken:
                case SyntaxKind.SlashToken:
                    return 5;

                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 4;
                
                case SyntaxKind.EqualsEqualsToken:
                case SyntaxKind.BangEqualsToken:
                    return 3;

                case SyntaxKind.AmpersandAmpersandToken:
                    return 2;

                case SyntaxKind.PipePipeToken:
                    return 1;

                default:
                    return 0;
            }
        }

        public static SyntaxKind GetKeywordKind(string text)
        {
            switch(text) {

                case ("true"):
                    return SyntaxKind.TrueKeyword;
                case ("false"):
                    return SyntaxKind.FalseKeyword;
                default:
                    return SyntaxKind.IdentifierToken;
            }
        }
    }
}