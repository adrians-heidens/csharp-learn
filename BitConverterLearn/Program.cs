using System;

namespace BitConverterLearn
{
    class Program
    {
        /// <summary>
        /// Demonstrates converting to from network byte order.
        /// </summary>
        static void Main(string[] args)
        {
            int value = 12345678;
            byte[] bytes = BitConverter.GetBytes(value);
            Console.WriteLine(BitConverter.ToString(bytes));

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            Console.WriteLine(BitConverter.ToString(bytes));
            // Call method to send byte stream across machine boundaries.

            // Receive byte stream from beyond machine boundaries.
            Console.WriteLine(BitConverter.ToString(bytes));
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            Console.WriteLine(BitConverter.ToString(bytes));
            int result = BitConverter.ToInt32(bytes, 0);
            Console.WriteLine("Original value: {0}", value);
            Console.WriteLine("Returned value: {0}", result);

            Console.ReadKey();
        }
    }
}
