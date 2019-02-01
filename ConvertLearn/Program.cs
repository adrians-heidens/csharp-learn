using System;

namespace ConvertLearn
{
    class Program
    {
        static void Main(string[] args)
        {
            string numberStr = "1234";
            //string numberStr = "1234a";
            //string numberStr = null;
            int number = -1;

            // Parse and TryParse.
            // System.FormatException if invalid format.
            // System.ArgumentNullException if argument null.
            //number = int.Parse(numberStr); 

            // Return 0 if argument null.
            var success = int.TryParse(numberStr, out number);
            Console.WriteLine(success);

            // Convert.
            // System.FormatException if invalid format.
            // Returns 0 if argument null.
            //number = Convert.ToInt32(numberStr); 

            Console.WriteLine(number);

            Console.WriteLine("Press key...");
            Console.ReadKey();
        }
    }
}
