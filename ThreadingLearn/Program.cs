using System;
using ThreadingLearn.Queue;

namespace ThreadingLearn
{
    class Program
    {
        static void Main(string[] args)
        {
            QueueExample.Run();

            Console.WriteLine("Press key...");
            Console.ReadKey();
        }
    }
}
