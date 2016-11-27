using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Shell
{
    public class CodeBlock : IShell
    {
        public bool Contains(IShell shell)
        {
            return false;
        }

        public MatchObject MatchText(string text, int startPosition)
        {
            if (!text.IsLineStart(startPosition))
            {
                return null;
            }
            var readPosition = startPosition;
            SkipPrefix(text, ref readPosition);
            if (readPosition - startPosition == 0)
            {
                return null;
            }
            var prefixLength = readPosition - startPosition;
            SkipLine(text, ref readPosition);
            ReadToBlockEnd(text, ref readPosition);
            var suffixPosition = readPosition;
            var suffixLenght = 0;
            if (text.HasSymbol(readPosition - 1, '\n'))
            {
                suffixPosition = readPosition - 1;
                suffixLenght = 1;
            }
            return new MatchObject(
                startPosition,
                prefixLength,
                suffixPosition,
                suffixLenght,
                GetConversionFunctionToHtml(),
                this);
        }

        private static void ReadToBlockEnd(string text, ref int readPosition)
        {
            while (IsIndent(text, readPosition))
            {
                readPosition++;
                SkipLine(text, ref readPosition);
            }
        }

        private static bool IsIndent(string text, int readPosition)
        {
            return text.HasSymbol(readPosition, '\t') || "    ".IsSubstring(text, readPosition);
        }

        private static void SkipPrefix(string text, ref int readPosition)
        {
            if (text[readPosition] == '\t')
            {
                readPosition++;
                return;
            }
            var position = readPosition;
            while (text.HasSymbol(position, ' '))
            {
                position++;
            }
            if (position - readPosition == 4)
            {
                readPosition = position;
            }
        }

        private static void SkipLine(string text, ref int readPosition)
        {
            while (!text.IsLineStart(readPosition) && readPosition < text.Length)
            {
                readPosition++;
            }
        }


        private Func<string, IEnumerable<Attribute>, string> GetConversionFunctionToHtml()
        {
            return (text, attributes) =>
            {
                var lines = text.Split('\n').Select(s => s.Replace("\r", "")).ToArray();
                var resultLines = new List<string> {lines[0]};
                resultLines.AddRange(lines.Skip(1).Select(RemoveIndent));
                var resutText = string.Join("\n", resultLines);
                return $"<pre><code>{resutText}</code></pre>";
            };
        }

        private string RemoveIndent(string line)
        {
            if (line.HasSymbol(0, '\t'))
            {
                return line.Substring(1, line.Length - 1);
            }
            if ("    ".IsSubstring(line, 0))
            {
                return line.Substring(4, line.Length - 4);
            }

            return line;

        }

        public char[] GetStopSymbols()
        {
            return new[] {' ', '\t'};
        }
    }
}
