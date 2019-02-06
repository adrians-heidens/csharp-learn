using System;

namespace JsonNetParse
{
    class Program
    {
        static void Main(string[] args)
        {
            //SimpleParsing.Run();
            //SerializerParsing.Run();
            //AttributeNaming.Run();
            //MissingMembersTest.Run();
            //JObjectLearn.Run();
            //NestedObjectsTest.Run();
            //DynamicParsingExample.Run();
            //CommandDispatchExample.Run();
            //CustomTypeHandling.Run();
            //SelfReferenceTest.Run();
            //JObjectDataTypes.Run();
            //DateTimeExample.Run();
            //KeyValuePairExample.Run();

            StrictConverters.StrictConverters.Run();

            Console.WriteLine("End...");
            Console.ReadKey();
        }
    }
}
