using System;
using System.Collections.Generic;
namespace Markdown.Shell
{
    public class DoubleUnderline : SingleUnderline
    {
        protected override string Prefix { get; set; } = "__";
        protected override string Suffix { get; set; } = "__";
        protected override string Tag { get; set; } = "strong";
        protected override List<Type> InnerShellsTypes { get; set; } = new List<Type>
        {
            typeof(SingleUnderline)
        };
    }
}
