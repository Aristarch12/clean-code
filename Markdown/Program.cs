using System;
using System.IO;

namespace Markdown
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("please specify an output file");
                return;
            }
            var md = new Md
            {
                BaseUrl = "http://",
                CssAtribute = "css"
            };
            var text = Console.In.ReadToEnd();
            var result = md.Render(text);
            using (var sw = new StreamWriter(args[0]))
            {
                sw.Write(result);
            }
        }
    }
}
