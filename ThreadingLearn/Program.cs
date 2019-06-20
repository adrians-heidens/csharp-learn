using System;

namespace ThreadingLearn
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadLearn.Run();
            //BlockingCollectionLearn.Run();

            Console.WriteLine("Press key...");
            Console.ReadKey();
        }
    }
}
