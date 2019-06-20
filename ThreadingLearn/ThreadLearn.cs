using System;
using System.Threading;

namespace ThreadingLearn
{
    static class ThreadLearn
    {
        public static void ThreadProc()
        {
            Console.WriteLine("Thread start.");
            int sleepTimeSeconds = 5;
            Console.WriteLine($"Sleeping {sleepTimeSeconds} seconds...");
            Thread.Sleep(sleepTimeSeconds * 1000);
            Console.WriteLine("Thread end.");
        }

        public static void Run()
        {
            Thread t1 = new Thread(new ThreadStart(ThreadProc));
            Thread t2 = new Thread(new ThreadStart(ThreadProc));

            var startTime = DateTime.UtcNow;
            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();
            var endTime = DateTime.UtcNow;
            Console.WriteLine($"Time spent: {endTime - startTime}");
        }
    }
}
