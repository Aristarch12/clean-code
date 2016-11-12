﻿using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
            return GetHtmlCode(text, mdShells);
        }

        private readonly List<IShell> mdShells = new List<IShell>()
        {
            new SingleUnderline(),
            new DoubleUnderline()
        };

        private static string GetHtmlCode(string text, List<IShell> shells)
        {
            var result = new StringBuilder();
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.SplitToTokens(text, shells);
            foreach (var token in tokens)
            {
                if (token.HasShell())
                {
                    var shellsInToken = shells.Where(s => token.Shell.Contains(s)).ToList();
                    var resultTextToken = RemoveEscapeСharacters(GetHtmlCode(token.Text, shellsInToken));
                    result.Append(token.Shell.RenderToHtml(resultTextToken));
                }
                else
                {
                    var resultText = RemoveEscapeСharacters(token.RenderToHtml());
                    result.Append(resultText);
                }
            }
            return result.ToString();
        }

        private static string RemoveEscapeСharacters(string text)
        {
            return text.Replace("\\", "");
        }
    }
}