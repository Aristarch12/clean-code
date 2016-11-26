using System;
using System.Collections.Generic;
using Markdown.Shell;

namespace Markdown.Tokenizer
{
    public class Token
    {
        public readonly string Text;
        public readonly List<Attribute> Attributes;
        public readonly Func<string, IEnumerable<Attribute>, string> ConvertToHtml;
        public readonly IShell Shell;

        public Token(string text, List<Attribute> attributes, Func<string, IEnumerable<Attribute>, string> convertToHtml, IShell shell=null)
        {  
            Text = text;
            Attributes = attributes;
            ConvertToHtml = convertToHtml;
            Shell = shell;
        }

        public void AddAttribute(Attribute attribute)
        {
            Attributes.Add(attribute);
        }

        public bool HasShell()
        {
            return Shell != null;
        }

    }
}
