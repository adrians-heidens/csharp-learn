using System;
using System.Linq;
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
    }
}
