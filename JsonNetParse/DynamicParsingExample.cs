using Newtonsoft.Json;
using System;

namespace JsonNetParse
{

    /// <summary>
    /// Using JObject, JArray, JValue, JProperty as dynamic types.
    /// </summary>
    static class DynamicParsingExample
    {
        public static void Run()
        {
            string json1 = @"{
    'Func': 'GetEntities',
    'Args': [1, 2],
}";
            dynamic obj = JsonConvert.DeserializeObject(json1);
            Console.WriteLine(obj.GetType());
            Console.WriteLine($"Func ({obj.Func.GetType()}): '{obj.Func}'");
            Console.WriteLine($"Args ({obj.Args.GetType()}): '{obj.Args}'");

            string json2 = @"{
    'Func': 'CreateEntity',
    'Args': {
        'EntityTypeId': 'Product',
        'Fields': {
            'ProductId': 123,
            'ProductName': {'en': 'Some name'},
        }
    },
}";
            obj = JsonConvert.DeserializeObject(json2);
            Console.WriteLine($"Func: '{obj.Func}'");

            var args = obj.Args;
            Console.WriteLine($"Args: '{args}'");

            Console.WriteLine($"Args (EntityTypeId): '{args.EntityTypeId}'");
            Console.WriteLine($"Args (Fields): '{args.Fields}'");

            foreach (var field in args.Fields)
            {
                Console.WriteLine($"  Field: ({field.GetType()}) {field.Name}: {field.Value}");
            }
        }
    }
}
