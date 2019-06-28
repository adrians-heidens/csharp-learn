using System;
using System.Collections.Generic;
using System.Threading;

namespace Log4NetLearn.Syslog
{
    /// <summary>
    /// Limited size synchronized queue which discards oldest elements when no
    /// space left.
    /// </summary>
    class DiscardingQueue<T>
    {
        private List<T> list = new List<T>();

        private int maxSize = 1000;

        private object lockObj = new object();

        public DiscardingQueue(int maxSize = 1000)
        {
            if (maxSize <= 0)
            {
                throw new ArgumentException($"Unexpected {nameof(maxSize)} value.");
            }
            this.maxSize = maxSize;
        }

        public void Add(T x)
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

        public T Get()
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
