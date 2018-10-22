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
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
