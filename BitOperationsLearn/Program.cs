using System;

namespace BitOperationsLearn
{
    static class Bits
    {
        public static int GetBits(int value, int index, int count)
        {
            int ones = (1 << count) - 1;
            int offset = 8 - (index + count);
            int mask = ones << offset;
            return (value & mask) >> offset;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            byte value = 0b10010001;
            Console.WriteLine(Convert.ToString(value, 2).PadLeft(8, '0'));

            int v = Bits.GetBits(value, 2, 3);
            Console.WriteLine(Convert.ToString(v, 2).PadLeft(8, '0'));

            Console.ReadKey();
        }
    }
}
