using System;
using System.Linq;

namespace LinqLearn
{
    class Program
    {
        static void Main(string[] args)
        {
            // A linq query.
            int[] numbers = new int[7] { 0, 1, 2, 3, 4, 5, 6 };
            var numQuery = from num in numbers where (num % 2) == 0 select num;
            foreach (int num in numQuery)
            {
                Console.Write("{0,1} ", num);
            }
            Console.WriteLine();

            // Counting elements.
            Console.WriteLine(numQuery.Count());

            // Convert to array or list.
            var numList = numQuery.ToList();
            Console.WriteLine(string.Join(",", numList));

            Console.WriteLine("Press key...");
            Console.ReadKey();
        }
    }
}
