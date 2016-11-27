using System;
using System.Collections.Generic;

namespace Markdown.Shell
{
    class UrlShell : IShell
    {
        private const string Prefix = "[";
        private const string Tag = "a";

        public bool Contains(IShell shell)
        {
            return false;
        }

        public MatchObject MatchText(string text, int startPosition)
        {
            if (TryOpen(text, startPosition))
            {
                for (var readPosition = startPosition + Prefix.Length - 1; readPosition < text.Length; readPosition++)
                {
                    var endSuffixPosition = GetEndSuffixPosition(text, readPosition);
                    if (endSuffixPosition != -1)
                    {
                        var suffixLength = endSuffixPosition - readPosition + 1;
                        var attribute = new Attribute(text.Substring(readPosition + 2, suffixLength - 3), AttributeType.Url);
                        return new MatchObject(
                            startPosition,
                            Prefix.Length,
                            readPosition,
                            suffixLength,
                            GetConversionFunctionToHtml(),
                            this,
                            attribute);
                    }
                }
            }
            return null;
        }

        public char[] GetStopSymbols()
        {
            return new [] { Prefix[0]};
        }

        private Func<string, IEnumerable<Attribute>, string> GetConversionFunctionToHtml()
        {
            return (text, attributes) =>
            {
                var attributesText = Attribute.ConvertAttributesToString(attributes);
                return $"<{Tag}{attributesText}>{text}</{Tag}>";
            };
        }

        private static bool IsIncorrectTagPosition(string text, int startPosition, int endPosition)
        {
            if (text.IsEscapedCharacter(startPosition))
            {
                return true;
            }
            return text.IsSurroundedByNumbers(startPosition, endPosition + Prefix.Length - 1);
        }

        public bool TryOpen(string text, int startPrefix)
        {
            if (IsIncorrectTagPosition(text, startPrefix, startPrefix + Prefix.Length - 1))
            {
                return false;
            }
            return text.TryMatchSubstring(Prefix, startPrefix);
        }

        public int GetEndSuffixPosition(string text, int startSuffix)
        {
            if (!"](".IsSubstring(text, startSuffix))
            {
                return -1;
            }
            for (var readPosition = startSuffix + 3; readPosition < text.Length; readPosition++)
            {
                if (text[readPosition] == ')' && !text.IsEscapedCharacter(readPosition))
                {
                    return readPosition;
                }
            }
            return -1;
        }
    }
}
