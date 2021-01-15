using System.Collections.Generic;

namespace Forte.CodeAnalysis.Syntax
{
    internal sealed class Lexer {

        /*
            Our lexer class

            Contains functions that assist in tokenizing user input

            Read text -> Create tokens
        */

        private readonly string _text;
        private int _position;
        private List<string> _diagnostics = new List<string>();

        public Lexer(string text) {
            
            /*
                NextTokener constructor

                sets the instance variable _text with input text

            */
            
            _text = text;
        }

        public IEnumerable<string> Diagnostics => _diagnostics;

        private char Current => Peek(0);

        private char Lookahead => Peek(1);

            /*

                Current

                returns the _text character at the current _position
                if we've exceeded the text length, return '\0'.
            */

        private char Peek(int offset) {

            var index = _position + offset;

            if (index >= _text.Length) {

                return '\0';
            }

            return _text[index];
        }

        private void Next() {

            /*
                Next()

                Increses the index we use to reference our _text.
            */

            _position++;
        }

        public SyntaxToken NextToken() {

            /*
                NextToken

                Identifies tokens and returns them.

                Current working tokens:
                <numbers>
                = - * / () TODO ^ %
                <whitespace>
            */

            // if we've reached the end of the text, return "\0"
            if (_position >= _text.Length) {

                // return an end of file token
                return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);
            }

            // check if the current character is a number
            if (char.IsDigit(Current)) {

                // starting digit of the number
                var start = _position;

                while (char.IsDigit(Current)) {

                    Next();
                }

                // at the end of the previous while loop, we should have the start and end digit
                var length = _position - start;
                var text = _text.Substring(start, length);

                // try to convert the string into an int, add errors if it can't be done
                if (!int.TryParse(text, out var value)) {

                    _diagnostics.Add($"The number {_text} isn't valid Int32");
                }

                // return a number token
                return new SyntaxToken(SyntaxKind.LiteralToken, start, text, value);
            }

            // check if the current character is a white space
            if (char.IsWhiteSpace(Current)) {

                // starting position of whitespace (in case multiple)
                var start = _position;

                while (char.IsWhiteSpace(Current)) {

                    Next();
                }

                // get the length of the whitespace block
                var length = _position - start;
                var text = _text.Substring(start, length);

                // return a whitespace token
                return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text, null);
            }

            // true and false
            if (char.IsLetter(Current)) {

                // starting position of whitespace (in case multiple)
                var start = _position;

                while (char.IsLetter(Current)) {

                    Next();
                }

                // get the length of the whitespace block
                var length = _position - start;
                var text = _text.Substring(start, length);
                var kind = SyntaxFacts.GetKeywordKind(text);

                // return a boolean token
                return new SyntaxToken(kind, start, text, null);
            }
            // Artithmetic operators

            // generate token for addition
            switch (Current)
            {
                case '+':
                    return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
                case '-':
                    return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
                case '*':
                    return new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null);
                case '/':
                    return new SyntaxToken(SyntaxKind.SlashToken, _position++, "/", null);
                case '(':
                    return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(", null);
                case ')':
                    return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")", null);
                case '&':
                    if (Lookahead == '&')
                        return new SyntaxToken(SyntaxKind.AmpersandAmpersandToken, _position += 2, "&&", null);
                    break;
                case '|':
                    if (Lookahead == '|')
                        return new SyntaxToken(SyntaxKind.PipePipeToken, _position += 2, "||", null);
                    break;
                case '=':
                    if (Lookahead == '=')
                        return new SyntaxToken(SyntaxKind.EqualsEqualsToken, _position += 2, "==", null);
                    break;
                case '!':
                    if (Lookahead == '=')
                        return new SyntaxToken(SyntaxKind.BangEqualsToken, _position += 2, "!=", null);
                    else
                        return new SyntaxToken(SyntaxKind.BangToken, _position++, "!", null);
            }

            _diagnostics.Add($"ERROR: bad character input: '{Current}'");
            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
        }
    }
}