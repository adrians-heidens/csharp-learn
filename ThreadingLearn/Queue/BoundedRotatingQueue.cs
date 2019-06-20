using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadingLearn.Queue
{
    /// <summary>
    /// Example of producer, consumer queue with maximum number of items to
    /// hold at the same time. Adding new item drops older ones.
    /// </summary>
    class BoundedRotatingQueue
    {
        private List<int> list = new List<int>();

        private int maxSize = 10;

        private object lockObj = new object();

        public BoundedRotatingQueue(int maxSize = 10)
        {
            if (maxSize <= 0)
            {
                throw new ArgumentException($"Unexpected {nameof(maxSize)} value.");
            }
            this.maxSize = maxSize;
        }

        public void Add(int x)
        {
            Monitor.Enter(lockObj);
            try
            {
                list.Add(x);
                while (list.Count > maxSize)
                {
                    list.RemoveAt(0);
                }
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
                return x;
            }
            finally
            {
                Monitor.Exit(lockObj);
            }
        }
    }
}