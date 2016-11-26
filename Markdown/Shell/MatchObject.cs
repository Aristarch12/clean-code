using System;
using System.Collections.Generic;

namespace Markdown.Shell
{
    public class MatchObject
    {
        public readonly int StartPrefix;
        public readonly int PrefixLength;
        public readonly int StartSuffix;
        public readonly int SuffixLength;
        public readonly IShell Shell;
        public readonly Func<string, IEnumerable<Attribute>, string> ConvertToHtml;
        public readonly Attribute Attribute;
        public MatchObject(int startPrefix, int prefixLength, int startSuffix, int suffixLength, Func<string, IEnumerable<Attribute>, string> convertToHtml, IShell shell, Attribute attribute=null)
        {
            StartPrefix = startPrefix;
            PrefixLength = prefixLength;
            StartSuffix = startSuffix;
            SuffixLength = suffixLength;
            ConvertToHtml = convertToHtml;
            Shell = shell;
            Attribute = attribute;
        }


    }
}
