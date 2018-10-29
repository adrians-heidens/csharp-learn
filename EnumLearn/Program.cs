using System;

namespace EnumLearn
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get enum from string.
            string recordType = "NS";

            // Return RecordType of throw System.ArgumentException.
            var value = Enum.Parse<RecordType>(recordType);
            Console.WriteLine(value);

            // Try to get enum from string.
            recordType = "A";
            RecordType value2;
            if (!Enum.TryParse(recordType, out value2))
            {
                Console.WriteLine($"RecordType '{recordType}' doesn't exist.");
            }
            else
            {
                Console.WriteLine(value2);
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
