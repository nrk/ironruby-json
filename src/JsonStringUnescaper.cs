using System;
using System.Text;
using IronRuby.Builtins;

namespace IronRuby.JsonExt {
    /// <summary>
    /// This class has been implemented following the StringDecoder class 
    /// that can be found in recent versions of Daniel Luz's json-jruby.
    /// </summary>
    internal class JsonStringUnescaper {
        int _position, _quoteStart, _charStart, _surrogatePairStart;
        char[] _source;

        public JsonStringUnescaper(char[] source) {
            _source = source;
            _quoteStart = - 1;
            _surrogatePairStart = -1;
        }

        public MutableString Unescape() {
            MutableStringBuilder output = new MutableStringBuilder(RubyEncoding.Binary);
            while (_position < _source.Length) {
                HandleCharacter(ReadUTF8Character(), output);
            }
            DetectEndOfQuoteSquence(_source.Length, output);
            return output.ToMutableString();
        }

        private void HandleCharacter(char chr, MutableStringBuilder output) {
            if (chr == '\\') {
                DetectEndOfQuoteSquence(_charStart, output);
                HandleEscapeSequence(output);
            }
            else if (Char.IsHighSurrogate(chr)) {
                DetectEndOfQuoteSquence(_charStart, output);
                HandleLowSurrogate(chr, output);
            }
            else if (Char.IsLowSurrogate(chr)) {
                // low surrogate with no high surrogate
                ThrowExceptionForInvalidUTF8();
            }
            else {
                StartQuoteSequence();
            }
        }

        private void HandleEscapeSequence(MutableStringBuilder output) {
            Ensure(1);
            switch (ReadUTF8Character()) {
                case 'b':
                    output.Append('\b');
                    break;
                case 'f':
                    output.Append('\f');
                    break;
                case 'n':
                    output.Append('\n');
                    break;
                case 'r':
                    output.Append('\r');
                    break;
                case 't':
                    output.Append('\t');
                    break;
                case 'u':
                    Ensure(4);
                    char codePoint = ReadUnicodeCodepoint();

                    if (Char.IsHighSurrogate(codePoint)) {
                        HandleLowSurrogate(codePoint, output);
                    }
                    else if (Char.IsLowSurrogate(codePoint)) {
                        // low surrogate with no high surrogate
                        ThrowExceptionForInvalidUTF8();
                    }
                    else {
                        WriteUTF8Character((int)codePoint, output);
                    }

                    break;
                default: 
                    // '\\', '"', '/'...
                    StartQuoteSequence();
                    break;
            }
        }

        private void HandleLowSurrogate(char highSurrogate, MutableStringBuilder output) {
            _surrogatePairStart = _charStart;
            Ensure(1);
            char lowSurrogate = ReadUTF8Character();

            if (lowSurrogate == '\\') {
                Ensure(5);
                if (ReadUTF8Character() != 'u') {
                    ThrowExceptionForInvalidUTF8();
                }
                lowSurrogate = ReadUnicodeCodepoint();
            }

            if (Char.IsLowSurrogate(lowSurrogate)) {
                WriteUTF8Character(Char.ConvertToUtf32(highSurrogate, lowSurrogate), output);
            }
            else {
                ThrowExceptionForInvalidUTF8();
            }
        }

        private void Ensure(int count) {
            if (_position + count > _source.Length) {
                ThrowExceptionForIncompleteUTF8();
            }
        }

        private char ReadUnicodeCodepoint() {
            uint codePoint;

            string hexCp = Encoding.ASCII.GetString(new byte[] { 
                (byte)_source[_position++],
                (byte)_source[_position++],
                (byte)_source[_position++],
                (byte)_source[_position++],
            });
            
            if (!UInt32.TryParse(hexCp, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out codePoint)) {
                Helpers.ThrowParserException("Invalid Unicode codepoint: {0}", hexCp);
            }

            return (char)codePoint;
        }

        private void WriteUTF8Character(int chr, MutableStringBuilder output) {
            byte[] utf16Bytes = Encoding.Convert(Encoding.UTF32, Encoding.UTF8, BitConverter.GetBytes(chr));
            for (int b = 0; b < utf16Bytes.Length; b++) {
                output.Append(utf16Bytes[b]);
            }
        }

        private char ReadUTF8Character() {
            _charStart = _position;
            char head = _source[_position++];

            if (head <= 0x7f) {
                // 0b0xxxxxxx (ASCII)
                return head;
            }
            if (head <= 0xbf) {
                // 0b10xxxxxx
                ThrowExceptionForInvalidUTF8();
            }
            if (head <= 0xdf) { 
                // 0b110xxxxx
                Ensure(1);
                int cp = ((head & 0x1f) << 6) | ReadNextUTF8CharacterPart();
                if (cp < 0x0080) {
                    ThrowExceptionForInvalidUTF8();
                }
                return (char)cp;
            }
            if (head <= 0xef) {
                // 0b1110xxxx
                Ensure(2);
                int cp = ((head & 0x0f) << 12)
                         | (ReadNextUTF8CharacterPart() << 6)
                         | ReadNextUTF8CharacterPart();
                if (cp < 0x0800) {
                    ThrowExceptionForInvalidUTF8();
                }
                return (char)cp;
            }
            if (head <= 0xf7) { 
                // 0b11110xxx
                Ensure(3);
                int cp = ((head & 0x07) << 18)
                         | (ReadNextUTF8CharacterPart() << 12)
                         | (ReadNextUTF8CharacterPart() << 6)
                         | ReadNextUTF8CharacterPart();
                if (!Char.IsSurrogate((char)cp)) {
                    ThrowExceptionForInvalidUTF8();
                }
                return (char)cp;
            }

            // 0b11111xxx?
            ThrowExceptionForInvalidUTF8();
            return Char.MinValue;
        }

        private int ReadNextUTF8CharacterPart() {
            char c = _source[_position++];
            // tail bytes must be 0b10xxxxxx
            if ((c & 0xc0) != 0x80) {
                ThrowExceptionForInvalidUTF8();
            }
            return c & 0x3f;
        }

        private void DetectEndOfQuoteSquence(int position, MutableStringBuilder output) {
            if (_quoteStart != -1) {
                output.Append(_source, _quoteStart, position - _quoteStart);
                _quoteStart = -1;
            }
        }

        private void StartQuoteSequence() {
            if (_quoteStart == -1) {
                _quoteStart = _charStart;
            }
        }

        private void ThrowExceptionForIncompleteUTF8() {
            ThrowExceptionForInvalidUTF8();
        }

        private void ThrowExceptionForInvalidUTF8() {
            int start = _surrogatePairStart != -1 ? _surrogatePairStart : _charStart;
            Helpers.ThrowParserException(
                "partial character in source, but hit end near {0}", 
                new String(_source, start, _source.Length - start)
            );
        }
    }
}