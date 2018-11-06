using Newtonsoft.Json.Linq;
using System;

namespace JsonNetParse
{
    /// <summary>
    /// Example of parsing Json which describes command and it's parameters.
    /// </summary>
    static class CommandDispatchExample
    {
        public static void Run()
        {
            string json = @"{
    'Function': 'GetSettings',
    'Args': {},
}";
            ParseJson(json);

            json = @"{
    'Function': 'GetSetting',
    'Args': {'Name': 'SYSLOG_HOST'},
}";
            ParseJson(json);

            json = @"{
    'Function': 'CreateEntity',
    'Args': {
        'EntityTypeId': 'Book',
        'Fields': {
            'Id': 123,
            'Isbn': '12345',
            'Title': {'en': 'Title in en', 'lv': 'Title in lv'},
            'Genre': 'Fiction',
        },
    },
}";
            ParseJson(json);

            // No Function field.
            json = @"{
    'Method': 'GetSetting',
    'Args': {'Name': 'SYSLOG_HOST'},
}";
            ParseJson(json);

            // No Args field.
            json = @"{
    'Function': 'GetSetting',
    'Kwargs': {'Name': 'SYSLOG_HOST'},
}";
            ParseJson(json);


        }

        private static void ParseJson(string json)
        {
            var command = JObject.Parse(json);

            var function = command["Function"]?.Value<string>();
            if (function == null)
            {
                Console.WriteLine("No Function found");
                return;
            }
            Console.WriteLine($"Function: '{function}'");

            var args = (JObject) command["Args"];
            if (args == null)
            {
                Console.WriteLine("No args found");
                return;
            }
            Console.WriteLine("Args found");

            if (function == "GetSettings")
            {
                if (args.Count != 0)
                {
                    Console.WriteLine("Unexpected arguments");
                    return;
                }
            }

            else if (function == "GetSetting")
            {
                if (args.Count != 1)
                {
                    Console.WriteLine("Unexpected arguments");
                    return;
                }
            }

            else if (function == "CreateEntity")
            {
                // Complex arguments.
                if (args.Count != 2)
                {
                    Console.WriteLine("Unexpected arguments");
                    return;
                }
            }

            else
            {
                Console.WriteLine($"Unknown function: '{function}'");
                return;
            }
        }
    }
}
