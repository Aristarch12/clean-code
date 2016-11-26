using System.Collections.Generic;
using System.Linq;
using static System.String;

namespace Markdown
{
    public class Attribute
    {
        public AttributeType Type { get; set; }
        public string Value { get; set; }
        private static readonly Dictionary<AttributeType, string> HtmlAttributes = new Dictionary<AttributeType, string>
        {
            {AttributeType.Style, "class" },
            {AttributeType.Url, "href" }
        };

        public static string  ConvertAttributesToString(IEnumerable<Attribute> attributes)
        {
            return Concat(attributes.Select(s => s.ConvertToString()));
        }

        public Attribute(string value, AttributeType type)
        {
            Value = value;
            Type = type;
        }

        public string ConvertToString()
        {
            return $" {HtmlAttributes[Type]}={Value}";
        }

    }

    public enum AttributeType
    {
        Url,
        Style
    }
}
