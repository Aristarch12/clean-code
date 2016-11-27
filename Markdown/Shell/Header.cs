using System;
using System.Collections.Generic;

namespace Markdown.Shell
{
    public class Header : IShell
    {

        public const string Tag = "h";

        public bool Contains(IShell shell)
        {
            return false;
        }

        public MatchObject MatchText(string text, int startPosition)
        {
            var countPrefixHashSign = 0;
            var readPosition = startPosition;
            while (readPosition < text.Length && text[readPosition] == '#')
            {
                countPrefixHashSign++;
                readPosition++;
            }

            if (countPrefixHashSign == 0 || countPrefixHashSign > 6)
            {
                return null;
            }
            for (readPosition++; readPosition < text.Length; readPosition++)
            {
                if (text.HasEndLine(readPosition))
                {
                    var suffixLength = text[readPosition] == '\n' ? 1 : 2;
                    return new MatchObject(
                        startPosition, 
                        countPrefixHashSign,
                        readPosition,
                        suffixLength,
                        GetConversionFunctionToHtml(countPrefixHashSign),
                        this);
                }
                if (text[readPosition] == '#')
                {
                    var startSuffix = readPosition;
                    var countSuffixHashSign = 0;
                    while (readPosition < text.Length && text[readPosition] == '#')
                    {
                        countSuffixHashSign++;
                        readPosition++;
                    }
                    return new MatchObject(
                        startPosition, 
                        countPrefixHashSign, 
                        startSuffix, 
                        countSuffixHashSign,
                        GetConversionFunctionToHtml(countPrefixHashSign),
                        this);
                }
            }
            return null;
        }

        private Func<string, IEnumerable<Attribute>, string> GetConversionFunctionToHtml(int countHashSign)
        {
            return (text, attributes) =>
            {
                var attributesText = Attribute.ConvertAttributesToString(attributes);
                return $"<{Tag}{countHashSign}{attributesText}>{text}</{Tag}{countHashSign}>";
            };
        }

        public char[] GetStopSymbols()
        {
            return new [] {'#'} ;
        }
    }
}
