using System;
using System.Text.RegularExpressions;

namespace RegexLearn
{
    class Program
    {
        static void Main(string[] args)
        {
            var regex = new Regex("^[a-z]([a-z0-9-]*[a-z0-9])?$", RegexOptions.IgnoreCase);
            Console.WriteLine(regex.IsMatch("abe"));
            Console.WriteLine(regex.IsMatch("abe-"));
            Console.WriteLine(regex.IsMatch("abe0"));

            // Example of matching in multiline text and capturing a group.
            var text = @"
Lorem ipsum dolor sit amet, consectetur adipiscing elit.
Mauris tincidunt viverra nisi, fringilla <b>posuere</b> sapien elementum at.
Donec scelerisque <b>dictum</b> diam vitae placerat. Etiam euismod sapien
tempor quam pulvinar,
".Trim();

            regex = new Regex(@"<b>([^<]+)</b>");
            var match = regex.Match(text);
            Console.WriteLine(match.Success);
            Console.WriteLine(match.Groups[1]);
            Console.WriteLine();

            // Removing whitespace from both ends with Trim.
            text = @"

Lorem ipsum dolor sit amet, consectetur adipiscing elit.
Mauris tincidunt viverra nisi, fringilla <b>posuere</b> sapien elementum at.

Donec scelerisque <b>dictum</b> diam vitae placerat. Etiam euismod sapien
tempor quam pulvinar,

";

            Console.WriteLine("Remove whitespace:\n----");
            text = text.Trim();
            Console.WriteLine(text);
            Console.WriteLine("----");
            Console.WriteLine();

            // Dedent.
            text = @"  
    
        foo
            bar
                span
            eggs
            foo
                bar
        spam


";

            text = Dedent(text);
            text = TrimEmptyLines(text);
            Console.WriteLine("----");
            Console.WriteLine(text);
            Console.WriteLine("----");

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        // From python textwrap dedent().
        private static string Dedent(string text)
        {
            var text2 = Regex.Replace(text, "p", "^[ \t]+$", RegexOptions.Multiline);
            var matches = Regex.Matches(text2, "(^[ \t]*)(?:[^ \t\n])", RegexOptions.Multiline);

            string margin = null;
            foreach (Match match in matches)
            {
                var indent = match.Groups[1].Value;
                if (margin == null)
                {
                    margin = indent;
                }
                else if (indent.StartsWith(margin))
                {

                }
                else if (margin.StartsWith(indent))
                {
                    margin = indent;
                }
                else
                {
                    var len = Math.Min(margin.Length, indent.Length);
                    for (var i = 0; i < len; i++)
                    {
                        if (margin[i] != indent[i])
                        {
                            margin = margin.Substring(0, i);
                        }
                    }
                }
            }

            return Regex.Replace(text, $"^{margin}", "", RegexOptions.Multiline);
        }

        /// <summary>
        /// Remove empty lines from start and end of the text.
        /// </summary>
        private static string TrimEmptyLines(string text)
        {
            var lines = text.Split('\n'); // TODO: Other line separators.

            int leading = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(lines[i]))
                {
                    break;
                }
                leading += 1;
            }

            int trailing = 0;
            for (int i = lines.Length - 1; i >= 0; i--)
            {
                if (!string.IsNullOrWhiteSpace(lines[i]))
                {
                    break;
                }
                trailing += 1;
            }

            Console.WriteLine($"... {leading}, {trailing}");

            return string.Join('\n', lines, leading, lines.Length - leading - trailing);
        }
    }
}
