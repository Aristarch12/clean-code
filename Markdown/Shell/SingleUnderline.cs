using System;
using System.Collections.Generic;

namespace Markdown.Shell
{
    public class SingleUnderline : IShell
    {
        protected virtual string Prefix { get; set; } = "_";
        protected virtual string Suffix { get; set; } =  "_";
        protected virtual string Tag { get; set; } =  "em";
        protected virtual List<Type> InnerShellsTypes { get; set; } = new List<Type>();

        public bool Contains(IShell shell)
        {
            return InnerShellsTypes.Contains(shell.GetType());
        }

        private bool IsIncorrectTagPosition(string text, int startPosition, int endPosition)
        {
            if (text.IsEscapedCharacter(startPosition))
            {
                return true;
            }
            if (text.IsSurroundedByNumbers(startPosition, endPosition + Prefix.Length - 1))
            {
                return true;
            }
            return false;
        }

        public bool TryOpen(string text, int startPrefix)
        {
            if (text.HasSpace(startPrefix + Prefix.Length))
            {
                return false;
            }
            if (IsIncorrectTagPosition(text, startPrefix, startPrefix + Prefix.Length - 1))
            {
                return false;
            }
            return text.TryMatchSubstring(Prefix, startPrefix);
        }

        public bool TryClose(string text, int startSuffix)
        {
            if (text.HasSpace(startSuffix - 1))
            {
                return false;
            }
            if (IsIncorrectTagPosition(text, startSuffix, startSuffix + Suffix.Length - 1))
            {
                return false;
            }
            return text.TryMatchSubstring(Suffix, startSuffix);
        }

        public bool TryMatch(string text, int startPosition, out MatchObject matchObject)
        {
            matchObject = null;
            if (TryOpen(text, startPosition))
            {
                for (var readPosition = startPosition + Prefix.Length; readPosition < text.Length; readPosition++)
                {
                    if (TryClose(text, readPosition))
                    {
                        matchObject = new MatchObject(startPosition, Prefix.Length, readPosition, Suffix.Length, GetConversionFunctionToHtml(), this);
                        return true;
                    }
                }
            }
            return false;
        }

        public char GetStopSymbol()
        {
            return Prefix[0];
        }

        private Func<string, IEnumerable<Attribute>, string> GetConversionFunctionToHtml()
        {
            return (text, attributes) =>
            {
                var attributesText = Attribute.ConvertAttributesToString(attributes);
                return $"<{Tag}{attributesText}>{text}</{Tag}>";
            };
        }
    }
}
