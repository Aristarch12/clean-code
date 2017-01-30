using System;

namespace Markdown
{
    public static class StringExtension
    {
        public static bool IsSubstring(this string substring, string text, int start)
        {
            if (start + substring.Length > text.Length)
            {
                return false;
            }
            return text.IndexOf(substring, start, substring.Length, StringComparison.Ordinal) != -1;
        }

        public static bool IsIncorrectEndingShell(this string text, int currentPosition)
        {
            return currentPosition >= text.Length || text.HasSpace(currentPosition);
        }

        public static bool IsEscapedCharacter(this string text, int currentPosition)
        {
            return currentPosition - 1 >= 0 && currentPosition < text.Length && text[currentPosition - 1] == '\\';
        }
        public static bool IsSurroundedByNumbers(this string text, int startPrefix, int endSuffix)
        {
            return startPrefix > 0 && char.IsDigit(text[startPrefix - 1]) &&
                   endSuffix + 1 < text.Length && char.IsDigit(text[endSuffix + 1]);
        }
        public static string RemoveEscapeСharacters(this string text)
        {
            return text.Replace("\\", "");
        }

        public static int GetPositionEndSubstring(this string substring, int startPosition)
        {
            return startPosition + substring.Length - 1;
        }

        public static bool TryMatchSubstring(this string text, string substring, int startPosition)
        {
            return substring.IsSubstring(text, startPosition);
        }

        public static bool HasSpace(this string text, int position)
        {
            return text.HasSymbol(position, ' ') || text.HasSymbol(position, '\t');
        }

        public static bool HasEndLine(this string text, int position)
        {
            return "\n".IsSubstring(text, position) || "\r\n".IsSubstring(text, position);
        }

        public static bool IsLineStart(this string text, int position)
        {
            return position == 0 || text.HasSymbol(position - 1, '\n');
        }

        public static bool HasSymbol(this string text, int position, char symbol)
        {
            if (position < 0 || position >= text.Length)
            {
                return false;
            }
            return text[position] == symbol;
        }
    }
    
}
