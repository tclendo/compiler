using System.Collections.Generic;
using Forte.CodeAnalysis.Text;

namespace Forte.CodeAnalysis.Syntax
{
    internal sealed class Lexer {

        /*
            Our lexer class

            Contains functions that assist in tokenizing user input

            Read text -> Create tokens
        */
        
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();
        private readonly SourceText _text;

        private int _position;
        
        private int _start;
        private SyntaxKind _kind;
        private object _value;

        public Lexer(SourceText text) {
            _text = text;
        }

        public DiagnosticBag Diagnostics => _diagnostics;

        private char Current => Peek(0);

        private char Lookahead => Peek(1);

        private char Peek(int offset) {

            var index = _position + offset;

            if (index >= _text.Length) {

                return '\0';
            }

            return _text[index];
        }

        public SyntaxToken Lex() {

            /*
                NextToken

                Identifies tokens and returns them.

                Current working tokens:
                <numbers>
                = - * / () TODO ^ %
                <whitespace>
            */

            _start = _position;
            _kind = SyntaxKind.BadToken;
            _value = null;
            
            // List of operators
            switch (Current)
            {
                case '\0':
                    _kind = SyntaxKind.EndOfFileToken;
                    break;
                // todo: exponents, modulus, 
                // addition
                case '+':
                    _kind = SyntaxKind.PlusToken;
                    _position++;
                    break;
                                    
                // subtraction
                case '-':
                    _kind = SyntaxKind.MinusToken;
                    _position++;
                    break;

                // multiplication
                case '*':
                    _kind = SyntaxKind.StarToken;
                    _position++;
                    break;

                // division
                case '/':
                    _kind = SyntaxKind.SlashToken;
                    _position++;
                    break;

                // open parenthesis
                case '(':
                    _kind = SyntaxKind.OpenParenthesisToken;
                    _position++;
                    break;

                // close parenthesis
                case ')':
                    _kind = SyntaxKind.CloseParenthesisToken;
                    _position++;
                    break;

                // ampersand
                case '&':

                    // logical and
                    if (Lookahead == '&') {
                        _kind = SyntaxKind.AmpersandAmpersandToken;
                        _position += 2;
                        break;
                    }
                    break;

                // pipe
                case '|':

                    // logical or
                    if (Lookahead == '|') {
                        _kind = SyntaxKind.PipePipeToken;
                        _position += 2;
                        break;
                    }
                    break;

                // equals
                case '=':
                    _position++;
                    // is equal to
                    if (Current != '=') {
                        _kind = SyntaxKind.EqualsToken;

                    } else {
                        _position++;
                        _kind = SyntaxKind.EqualsEqualsToken;
                    }
                    break;

                // bang
                case '!':
                    _position++;
                    // not equal to
                    if (Current != '=') {
                        _kind = SyntaxKind.BangToken;
                    }
                    // logical not
                    else {
                        _kind = SyntaxKind.BangEqualsToken;
                        _position++;
                    }
                    break;

                case '0': case '1': case '2': case '3': case '4':
                case '5': case '6': case '7': case '8': case '9':
                    ReadNumberToken();
                    break;

                case ' ': case '\t': case '\n': case '\r':
                    ReadWhiteSpace();
                    break;
                    
                default:
                    if (char.IsLetter(Current))
                    {
                        ReadIdentifierOrKeyword();
                    } else if (char.IsWhiteSpace(Current))
                    {
                        ReadWhiteSpace();
                    }

                    // true and false
                    else {
                        _diagnostics.ReportBadCharacter(_position, Current);
                        _position++;
                    }
                    break;
            }
            
            var length = _position - _start;
            var text = SyntaxFacts.GetText(_kind);
            if (text == null) {
                text = _text.ToString(_start, length);
            }

            return new SyntaxToken(_kind, _start, text, _value);
        }

        private void ReadWhiteSpace()
        {
            while (char.IsWhiteSpace(Current)) {
                _position++;
            }

            _kind = SyntaxKind.WhitespaceToken;
        }

        private void ReadIdentifierOrKeyword()
        {
            while (char.IsLetter(Current)) {
                _position++;
            }

            var length = _position - _start;
            var text = _text.ToString(_start, length);
            _kind = SyntaxFacts.GetKeywordKind(text);
        }

        private void ReadNumberToken()
        {
            while (char.IsDigit(Current)){
                _position++;
            }

            var length = _position - _start;
            var text = _text.ToString(_start, length);

            if (!int.TryParse(text, out var value))
            {
                _diagnostics.ReportInvalidNumber(new TextSpan(_start, length), text, typeof(int));
            }
            _value = value;
            _kind = SyntaxKind.NumberToken;
        }
    }
}