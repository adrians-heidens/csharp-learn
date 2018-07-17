using System;

namespace JsonNetParse
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleParsing.Run();
            SerializerParsing.Run();
            AttributeNaming.Run();
            Console.ReadKey();
        }
    }
}
