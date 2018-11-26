using System;
using ExtensionsLearn.Extensions; // To enable extensions.

namespace ExtensionsLearn
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("12345".Truncate(10));
            Console.WriteLine("12345678901234".Truncate(10));

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
