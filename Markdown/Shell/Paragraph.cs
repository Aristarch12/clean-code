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

        public bool TryMatch(string text, int startPosition, out MatchObject matchObject)
        {
            matchObject = null;
            if (!"  ".IsSubstring(text, startPosition))
            {
                return false;
            }
            var readPosition = startPosition;
            while (text.HasSpace(readPosition) || text[readPosition] == '\r')
            {
                readPosition++;
            }

            if (readPosition < text.Length && text[readPosition] == '\n')
            {
                matchObject =  new MatchObject(startPosition,
                    readPosition - startPosition,readPosition, 1, GetConversionFunctionToHtml(), this);
                return true;
            }
            return false;
        }

        private Func<string, IEnumerable<Attribute>, string> GetConversionFunctionToHtml()
        {
            return (text, attributes) => $"<{Tag}/>";
        }

        public char GetStopSymbol()
        {
            return ' ';
        }
    }
}
