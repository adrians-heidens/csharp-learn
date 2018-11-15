using System;

namespace StringFormatLearn
{
    class Program
    {
        static void Main(string[] args)
        {
            // String interpolation.
            var foo = 12;
            Console.WriteLine($"Variable foo={foo}.");

            // String formatting with placeholders.
            var bar = "bar";
            string template = "Variable {0} in a spearate {0}, {1} template";
            Console.WriteLine(string.Format(template, foo, bar));

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
