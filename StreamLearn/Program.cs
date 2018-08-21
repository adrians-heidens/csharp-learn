using System;
using System.IO;

namespace StreamLearn
{
    class Program
    {
        static void Main(string[] args)
        {
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.Write(BitConverter.GetBytes((ushort) 65535));
            memoryStream.Write(BitConverter.GetBytes((int) 120));

            Console.WriteLine(BitConverter.ToString(memoryStream.ToArray()));

            Console.ReadKey();
        }
    }
}
