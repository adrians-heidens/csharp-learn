using System;

namespace NullConditionalLearn
{
    class Foo
    {
        public string Bar { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Foo foo1 = new Foo { Bar = "01" };
            Console.WriteLine($"foo1.Bar = '{ foo1.Bar }'");

            Foo foo2 = null;
            Console.WriteLine($"foo2.Bar = '{ foo2?.Bar }'");

            int[] array1 = { 1, 2, 3 };
            Console.WriteLine($"array1[2] = '{ array1[2] }'");

            int[] array2 = null;
            Console.WriteLine($"array2[2] = '{ array2?[2] }'");

            Console.ReadKey();
        }
    }
}
