using System;
using System.Collections.Generic;

namespace Markdown.Shell
{
    public class Paragraph : IShell
    {
        private const string Tag = "br";

        public bool Contains(IShell shell)
        {
            return false;
        }

        public MatchObject MatchText(string text, int startPosition)
        {
            if (!"  ".IsSubstring(text, startPosition))
            {
                return null;
            }
            var readPosition = startPosition;
            while (text.HasSpace(readPosition) || text[readPosition] == '\r')
            {
                readPosition++;
            }

            if (readPosition < text.Length && text[readPosition] == '\n')
            {
                return  new MatchObject(startPosition,
                    readPosition - startPosition,readPosition, 1, GetConversionFunctionToHtml(), this);
            }
            return null;
        }

        private Func<string, IEnumerable<Attribute>, string> GetConversionFunctionToHtml()
        {
            return (text, attributes) => $"<{Tag}/>";
        }

        public char[] GetStopSymbols()
        {
            return new char[] {' '};
        }
    }
}
