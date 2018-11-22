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

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
