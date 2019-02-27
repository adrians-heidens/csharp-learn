using FileEmbedLearn.Properties;
using System;

namespace FileEmbedLearn
{
    class Program
    {
        static void Main(string[] args)
        {
            var lipsum = Resources.lipsum;
            Console.WriteLine(lipsum);

            Console.WriteLine("Press key...");
            Console.ReadKey();
        }
    }
}
