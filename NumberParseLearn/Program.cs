using System;
using System.Globalization;

namespace NumberParseLearn
{
    /// <summary>
    /// Demonstrates number parsing.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var ushortHex = "FEB1";
            var num = ushort.Parse(ushortHex, NumberStyles.HexNumber);
            Console.WriteLine(num);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
