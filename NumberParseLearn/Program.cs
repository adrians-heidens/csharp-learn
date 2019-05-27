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

            // From hex string to number.
            ushortHex = "00Ba";
            ushort val = 0;
            var result = ushort.TryParse(ushortHex, NumberStyles.HexNumber, null, out val);
            Console.WriteLine(result);
            Console.WriteLine(val);

            // To uppercase hex string.
            Console.WriteLine(val.ToString("x4"));

            Console.WriteLine("----");

            // Trying to convert different things to number.
            Console.WriteLine(Convert.ToInt32("0012")); // 12
            // Console.WriteLine(Convert.ToInt32("0x12")); // System.FormatException
            // Console.WriteLine(Convert.ToInt32("12.34")); // System.FormatException
            // Console.WriteLine(Convert.ToInt32("")); // System.FormatException
            Console.WriteLine(Convert.ToInt32(" 16")); // 16
            Console.WriteLine(Convert.ToInt32(56.7)); // 57
            
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
