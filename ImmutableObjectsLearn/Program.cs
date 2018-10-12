using System;
using System.Collections.Generic;

namespace ImmutableObjectsLearn
{
    class Program
    {
        static void Main(string[] args)
        {
            var record = new Record("foo", 10);
            Console.WriteLine(record);
            // record.Ttl = 0; // Cannot be assigned.
            
            var recordList = new List<Record>();
            recordList.Add(record);
            recordList.Add(new Record("bar", 20));

            var header = new Header(12, (ushort)recordList.Count);
            Console.WriteLine(header);

            var message = new Message(header, recordList);
            Console.WriteLine(message);

            recordList.Clear(); // Clear the original list.

            Console.WriteLine(message); // Message still have records

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
