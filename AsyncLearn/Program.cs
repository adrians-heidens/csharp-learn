using System;

namespace AsyncLearn
{
    class Program
    {
        static void Main(string[] args)
        {
            //AsyncInSync.Run();
            CpuBoundAsync.Run();

            Console.WriteLine("Press key...");
            Console.ReadKey();
        }
    }
}
