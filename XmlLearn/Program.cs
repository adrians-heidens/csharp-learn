using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace XmlLearn
{
    /// <summary>
    /// Demonstrates LINQ to XML file parsing.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var xmlDoc = "<span>foo <b>bar</b> spam <b><i>eggs</i></b> <i>ex</i> ample.</span>";
            HtmlToMarkdown(xmlDoc);
            Console.WriteLine();

            // XElement
            // XAttribute
            // XDocument

            // Elements()
            // Descendants()
            // Element()
            // Attribute()

            // Root element.
            var root = XElement.Load("DnsBits.xml");
            Console.WriteLine(root.Name);
            Console.WriteLine();

            // Iterate through child elements.
            foreach (var elem in root.Elements())
            {
                Console.WriteLine(elem.Name);
            }
            Console.WriteLine();

            // Get value.
            var assemblyName = (string)root.Element("assembly").Element("name");
            Console.WriteLine(assemblyName);
            Console.WriteLine();

            // Get all member names.
            foreach (var name in root.Descendants("member").Select(x => x.Attribute("name")))
            {
                Console.WriteLine(name);
            }
            Console.WriteLine();

            // Get all summaries by chaining element queries.
            foreach (var elem in root.Element("members").Elements("member").Elements("summary"))
            {
                Console.WriteLine(elem);
            }
            Console.WriteLine();

            // Complex filtering. Get summaries from member elements which contains more than 2 param elements.
            foreach (var summary in root.Descendants("member")
                .Where(x => x.Elements("param").Count() > 2)
                .Elements("summary").Select(x => ((string)x).Trim()))
            {
                Console.WriteLine($"* {summary}");
            }
            Console.WriteLine();

            Console.WriteLine("Press key...");
            Console.ReadKey();
        }

        /// <summary>
        /// Example of converting simple html text to markdown.
        /// </summary>
        private static string HtmlToMarkdown(string html)
        {
            var element = XElement.Parse(html, LoadOptions.PreserveWhitespace);
            Console.WriteLine(element);

            StringBuilder sb = new StringBuilder();
            AddElement(sb, element);

            Console.WriteLine(sb.ToString());

            return null;
        }

        private static void AddElement(StringBuilder builder, XElement element)
        {
            foreach (var e in element.Nodes())
            {
                if (e.NodeType == XmlNodeType.Text)
                {
                    builder.Append((XText)e);
                    continue;
                }

                var elem = e as XElement;
                if (elem.Name == "b")
                {
                    builder.Append("**");
                    AddElement(builder, elem);
                    builder.Append("**");
                }
                else if (elem.Name == "i")
                {
                    builder.Append("_");
                    AddElement(builder, elem);
                    builder.Append("_");
                }
            }
        }
    }
}
