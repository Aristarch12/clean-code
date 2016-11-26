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

        public bool TryMatch(string text, int startPosition, out MatchObject matchObject)
        {
            matchObject = null;
            var countPrefixHashSign = 0;
            var readPosition = startPosition;
            while (readPosition < text.Length && text[readPosition] == '#')
            {
                countPrefixHashSign++;
                readPosition++;
            }

            if (countPrefixHashSign == 0 || countPrefixHashSign > 6)
            {
                return false;
            }
            for (readPosition++; readPosition < text.Length; readPosition++)
            {
                if (text.HasEndLine(readPosition))
                {
                    var suffixLength = text[readPosition] == '\n' ? 1 : 2;
                    matchObject = new MatchObject(
                        startPosition, 
                        countPrefixHashSign,
                        readPosition,
                        suffixLength,
                        GetConversionFunctionToHtml(countPrefixHashSign),
                        this);
                    return true;
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
                    matchObject =  new MatchObject(
                        startPosition, 
                        countPrefixHashSign, 
                        startSuffix, 
                        countSuffixHashSign,
                        GetConversionFunctionToHtml(countPrefixHashSign),
                        this);
                    return true;
                }
            }
            return false;
        }

        private Func<string, IEnumerable<Attribute>, string> GetConversionFunctionToHtml(int countHashSign)
        {
            return (text, attributes) =>
            {
                var attributesText = Attribute.ConvertAttributesToString(attributes);
                return $"<{Tag}{countHashSign}{attributesText}>{text}</{Tag}{countHashSign}>";
            };
        }



        public char GetStopSymbol()
        {
            return '#';
        }
    }
}
