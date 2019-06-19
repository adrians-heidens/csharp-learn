using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadingLearn.Queue
{
    static class QueueExample
    {
        public static void Run()
        {
            var queue = new BoundedRotatingQueue(maxSize: 2);

            var tasks = new List<Task>();
            tasks.Add(Task.Run(() => {
                Console.WriteLine("Adding 1...");
                queue.Add(1);
                Console.WriteLine("1 added.");
                Console.WriteLine("Adding 2...");
                queue.Add(2);
                Console.WriteLine("2 added.");
                Console.WriteLine("Adding 3...");
                queue.Add(3);
                Console.WriteLine("3 added.");
                Console.WriteLine("Adding 4...");
                queue.Add(4);
                Console.WriteLine("4 added.");
            }));
            tasks.Add(Task.Run(() => {
                Thread.Sleep(5000);

                int x;

                Console.WriteLine("Getting...");
                x = queue.Get();
                Console.WriteLine($"Got {x}");

                Console.WriteLine("Getting...");
                x = queue.Get();
                Console.WriteLine($"Got {x}");
            }));
            Task.WaitAll(tasks.ToArray());
        }
    }
}