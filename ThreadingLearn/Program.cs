using System;
using ThreadingLearn.Queue;

namespace ThreadingLearn
{
    class Program
    {
        static void Main(string[] args)
        {
            //ThreadLearn.Run();
            //BlockingCollectionLearn.Run();
            QueueExample.Run();

            Console.WriteLine("Press key...");
            Console.ReadKey();
        }
    }
}
