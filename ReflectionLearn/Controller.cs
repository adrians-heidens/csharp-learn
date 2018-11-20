using System;

namespace ReflectionLearn
{
    class Controller
    {
        public string Connection { get; }

        public Controller(string connection)
        {
            Connection = connection;
        }

        public void DoFoo()
        {
            Console.WriteLine($"DoFoo() wtih {Connection}");
        }

        public void DoBar(string spam, int eggs = 0)
        {
            Console.WriteLine($"DoBar(spam={spam}, eggs={eggs}) wtih {Connection}");
        }
    }
}
