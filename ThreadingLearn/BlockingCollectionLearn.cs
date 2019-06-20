using System;
using System.Collections.Concurrent;
using System.Threading;

namespace ThreadingLearn
{
    static class BlockingCollectionLearn
    {
        private static BlockingCollection<string> queue = new BlockingCollection<string>();

        private static void Consume()
        {
            Console.WriteLine("Consume thread start.");

            Console.WriteLine("Waiting for item.");

            var item = queue.Take();
            Console.WriteLine($"Got item: {item}");
            
            Console.WriteLine("Consume thread end.");
        }

        public static void Run()
        {
            Console.WriteLine("Main start.");

            var t = new Thread(Consume);
            t.Start();

            Thread.Sleep(1000);
            Console.WriteLine("Adding items.");
            queue.Add("foo");
            
            t.Join();

            Console.WriteLine("Main end.");
        }
    }
}
