using System.Collections.Generic;
using System.Threading;

namespace ThreadingLearn.Queue
{
    /// <summary>
    /// Example of producer, consumer queue with maximum number of items to
    /// hold at the same time. Adding new item blocks if queue full.
    /// </summary>
    class BoundedQueue
    {
        private List<int> list = new List<int>();

        private int maxSize = 10;

        private object lockObj = new object();

        public BoundedQueue(int maxSize = 10)
        {
            this.maxSize = maxSize;
        }

        public void Add(int x)
        {
            Monitor.Enter(lockObj);
            try
            {
                while (list.Count >= maxSize)
                {
                    Monitor.Wait(lockObj); // Wait for not-full condition.
                }
                list.Add(x);
                Monitor.Pulse(lockObj); // Notify non-empty condition.
            }
            finally
            {
                Monitor.Exit(lockObj);
            }
        }

        public int Get()
        {
            Monitor.Enter(lockObj);
            try
            {
                while (list.Count == 0)
                {
                    Monitor.Wait(lockObj); // Wait not-empty condition.
                }
                var x = list[0];
                list.RemoveAt(0);
                Monitor.Pulse(lockObj); // Notify not-full condition.
                return x;
            }
            finally
            {
                Monitor.Exit(lockObj);
            }
        }
    }
}