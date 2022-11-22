
using System;
using System.Collections.Generic;
using ICSharpCode.AvalonEdit.Document;

namespace RobotTools.UI.Editor
{
    public static  class AvalonEditorExtensions
    {
        public static int FindNextWordEnd(this TextDocument document, int offset)
        {
            return document.FindNextWordEnd(offset, new List<char>());
        }

        public static int FindNextWordEnd(this TextDocument document, int offset, IList<char> allowedChars)
        {
            for (var num = offset; num != -1; num++)
            {
                if (num >= document.TextLength)
                {
                    return -1;
                }
                var charAt = document.GetCharAt(num);
                if (!IsWordPart(charAt) && !allowedChars.Contains(charAt))
                {
                    return num;
                }
            }
            return -1;
        }

        public static int FindNextWordStart(this TextDocument document, int offset)
        {
            for (var num = offset; num != -1; num++)
            {
                if (num >= document.TextLength)
                {
                    return 0;
                }
                var charAt = document.GetCharAt(num);
                if (!IsWhitespaceOrNewline(charAt))
                {
                    return num;
                }
            }
            return 0;
        }

        public static int FindNextWordStartRelativeTo(this TextDocument document, int offset)
        {
            for (var num = offset; num != -1; num++)
            {
                var charAt = document.GetCharAt(num);
                if (!IsWhitespaceOrNewline(charAt))
                {
                    return num - offset;
                }
            }
            return 0;
        }

        public static int FindPrevWordStart(this TextDocument document, int offset)
        {
            for (var num = offset - 1; num != -1; num--)
            {
                var charAt = document.GetCharAt(num);
                if (!IsWordPart(charAt))
                {
                    return num + 1;
                }
            }
            return 0;
        }

        public static ISegment GetLineWithoutIndent(this TextDocument document, int lineNumber)
        {
            var lineByNumber = document.GetLineByNumber(lineNumber);
            var whitespaceAfter = TextUtilities.GetWhitespaceAfter(document, lineByNumber.Offset);
            if (whitespaceAfter.Length == 0)
            {
                return lineByNumber;
            }
            return new TextSegment
            {
                StartOffset = lineByNumber.Offset + whitespaceAfter.Length,
                EndOffset = lineByNumber.EndOffset,
                Length = lineByNumber.Length - whitespaceAfter.Length
            };
        }

        public static string GetWordBeforeCaret(this AvalonEditor editor)
        {
            if (editor == null)
            {
                throw new ArgumentNullException("editor");
            }
             
            var offset = editor.TextArea.Caret.Offset;
            var num = editor.Document.FindPrevWordStart(offset);
            if (num < 0)
            {
                return string.Empty;
            }
            return editor.Document.GetText(num, offset - num);
        }

        public static string GetWordBeforeCaret(this AvalonEditor editor, char[] allowedChars)
        {
            if (editor == null)
            {
                throw new ArgumentNullException("editor");
            }
            var offset = editor.TextArea.Caret.Offset;
            var num = FindPrevWordStart(editor.Document, offset, allowedChars);
            if (num < 0)
            {
                return string.Empty;
            }
            return editor.Document.GetText(num, offset - num);
        }

        public static string GetStringBeforeCaret(this AvalonEditor editor)
        {
            if (editor == null)
            {
                throw new ArgumentNullException("editor");
            }
            var line = editor.TextArea.Caret.Line;
            if (line < 1)
            {
                return string.Empty;
            }
            var offset = editor.TextArea.Caret.Offset;
            if (line > editor.Document.LineCount)
            {
                return string.Empty;
            }
            var lineByNumber = editor.Document.GetLineByNumber(line);
            var length = offset - lineByNumber.Offset;
            return editor.Document.GetText(lineByNumber.Offset, length);
        }

        public static string GetWordBeforeOffset(this AvalonEditor editor, int offset, char[] allowedChars)
        {
            if (editor == null)
            {
                throw new ArgumentNullException("editor");
            }
            var num = FindPrevWordStart(editor.Document, offset, allowedChars);
            if (num < 0)
            {
                return string.Empty;
            }
            return editor.Document.GetText(num, offset - num);
        }

        public static string GetTokenBeforeOffset(this AvalonEditor editor, int offset)
        {
            if (editor == null)
            {
                throw new ArgumentNullException("editor");
            }
            var num = -1;
            for (var i = offset - 1; i > -1; i--)
            {
                var charAt = editor.Document.GetCharAt(i);
                if (charAt == ' ' || charAt == '\n' || charAt == '\r' || charAt == '\t')
                {
                    num = i + 1;
                    break;
                }
            }
            if (num < 0)
            {
                return string.Empty;
            }
            return editor.Document.GetText(num, offset - num);
        }

        public static string GetWordUnderCaret(this AvalonEditor editor, char[] allowedChars)
        {
            if (editor == null)
            {
                throw new ArgumentNullException("editor");
            }
            var offset = editor.TextArea.Caret.Offset;
            var num = FindPrevWordStart(editor.Document, offset, allowedChars);
            var num2 = editor.Document.FindNextWordEnd(offset, allowedChars);
            if (num < 0 || num2 == 0 || num2 < num)
            {
                return string.Empty;
            }
            return editor.Document.GetText(num, num2 - num);
        }

        public static string GetFirstWordInLine(this AvalonEditor editor, int lineNumber)
        {
            if (editor == null)
            {
                throw new ArgumentNullException("editor");
            }
            return editor.Document.GetFirstWordInLine(lineNumber);
        }

        public static string GetFirstWordInLine(this TextDocument document, int lineNumber)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            var offset = document.GetOffset(lineNumber, 0);
            var num = document.FindNextWordStart(offset);
            if (num < 0)
            {
                return string.Empty;
            }
            var num2 = document.FindNextWordEnd(num);
            if (num2 < 0)
            {
                return string.Empty;
            }
            return document.GetText(num, num2 - num);
        }

        public static string GetWordUnderCaret(this AvalonEditor editor)
        {
            if (editor == null)
            {
                throw new ArgumentNullException("editor");
            }
            var offset = editor.TextArea.Caret.Offset;
            var num = editor.Document.FindPrevWordStart(offset);
            var num2 = editor.Document.FindNextWordEnd(offset);
            if (num < 0 || num2 == 0 || num2 < num)
            {
                return string.Empty;
            }
            return editor.Document.GetText(num, num2 - num);
        }

        public static string GetWordUnderOffset(this AvalonEditor editor, int offset)
        {
            if (editor == null)
            {
                throw new ArgumentNullException("editor");
            }
            var num = editor.Document.FindPrevWordStart(offset);
            var num2 = editor.Document.FindNextWordEnd(offset);
            if (num < 0 || num2 == 0 || num2 < num)
            {
                return string.Empty;
            }
            return editor.Document.GetText(num, num2 - num);
        }

        public static string GetWordUnderOffset(this AvalonEditor editor, int offset, char[] allowedChars)
        {
            if (editor == null)
            {
                throw new ArgumentNullException("editor");
            }
            var num = FindPrevWordStart(editor.Document, offset, allowedChars);
            var num2 = editor.Document.FindNextWordEnd(offset, allowedChars);
            if (num < 0 || num2 == 0 || num2 < num)
            {
                return string.Empty;
            }
            return editor.Document.GetText(num, num2 - num);
        }

        private static int FindPrevWordStart(TextDocument document, int offset, IList<char> allowedChars)
        {
            for (var num = offset - 1; num != -1; num--)
            {
                var charAt = document.GetCharAt(num);
                if (!IsWordPart(charAt) && !allowedChars.Contains(charAt))
                {
                    return num + 1;
                }
            }
            return 0;
        }

        public static bool IsWhitespaceOrNewline(char ch)
        {
            return ch == ' ' || ch == '\t' || ch == '\n' || ch == '\r';
        }

        private static bool IsWordPart(char ch)
        {
            return char.IsLetterOrDigit(ch) || ch == '_';
        }
    }
}
