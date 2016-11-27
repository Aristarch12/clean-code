using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Shell;

namespace Markdown.Tokenizer
{
    public class StringTokenizer
    {
        private readonly string text;
        private readonly List<IShell> shells;
        private int currentPosition;

        public StringTokenizer(string text, List<IShell> shells)
        {
            this.text = text;
            this.shells = shells;
        }

        public IEnumerable<Token> ReadAllTokens()
        {
            while (HasMoreTokens())
            {
                yield return NextToken();
            }
        }

        public bool HasMoreTokens()
        {
            return currentPosition < text.Length;
        }

        public Token NextToken()
        {
            if (!HasMoreTokens())
            {
                throw new InvalidOperationException("impossible to get the next token. all tokens listed");
            }
            var token = ReadFormattingToken();
            if (token != null)
            {
                return token;
            }
            var stopSymbols = shells.Select(s => s.GetStopSymbols()).SelectMany(c => c).ToArray();
            var rawText = ReadRawText(stopSymbols);
            return new Token(rawText, new List<Attribute>(), ConvertRawTextToHtml);
        }

        private static Func<string, IEnumerable<Attribute>, string> ConvertRawTextToHtml => (rawText, attributes) => rawText;

        public Token ReadFormattingToken()
        {
            var maxPrefix = 0;
            MatchObject resultMatchObject = null;
            foreach (var shell in shells)
            {
                var matchObject = shell.MatchText(text, currentPosition);
                if (maxPrefix < matchObject?.PrefixLength)
                {
                    maxPrefix = matchObject.PrefixLength;
                    resultMatchObject = matchObject;
                }
            }
            if (resultMatchObject == null)
            {
                return null;
            }
            var content = GetContent(text, resultMatchObject);
            var attributes = new List<Attribute>();
            if (resultMatchObject.Attribute != null)
            {
                attributes.Add(resultMatchObject.Attribute);
            }
            var resultToken = new Token(content, attributes, resultMatchObject.ConvertToHtml, resultMatchObject.Shell);
            currentPosition = resultMatchObject.StartSuffix + resultMatchObject.SuffixLength;
            return resultToken;
        }

        private static string GetContent(string originalText, MatchObject matchObject)
        {
            var startText = matchObject.StartPrefix + matchObject.PrefixLength;
            var textLength = matchObject.StartSuffix - (matchObject.StartPrefix + matchObject.PrefixLength);
            return originalText.Substring(startText, textLength);
        }

        private string ReadRawText(char[] stopSypbols)
        {
            var readPosition = currentPosition;
            var tokenText = new StringBuilder();
            for (; readPosition < text.Length; readPosition++)
            {
                tokenText.Append(text[readPosition]);
                if (readPosition != text.Length - 1 && stopSypbols.Contains(text[readPosition + 1]))
                {
                    readPosition++;
                    break;
                }
            }
            currentPosition = readPosition;
            return tokenText.ToString();
        }
    }
}