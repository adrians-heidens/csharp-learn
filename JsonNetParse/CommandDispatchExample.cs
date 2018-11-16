using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace JsonNetParse
{
    /// <summary>
    /// Collection of methods used for calls.
    /// </summary>
    static class Commands
    {
        public static void GetSettings() {
            Console.WriteLine($"+++ GetSettings()");
        }

        public static void GetSetting(string name) {
            Console.WriteLine($"+++ GetSetting(name='{name}')");
        }

        public static void CreateEntity(string entityTypeId, FieldValues fieldValues)
        {
            Console.WriteLine($"+++ CreateEntity(entityTypeId={entityTypeId}, fieldValues={fieldValues})");
        }

        public static void FuncFoo(int foo, string bar, string spam="eggs") {
            Console.WriteLine($"+++ FuncFoo(foo={foo}, bar='{bar}', spam='{spam}')");
        }

        public static void FuncBar(int bar, LocaleString localeString)
        {
            Console.WriteLine($"+++ FuncBar(bar={bar}, localeString='{localeString}')");
        }
    }

    /// <summary>
    /// An example of complex custom data type.
    /// </summary>
    class FieldValues
    {
        public IDictionary<string, object> Fields { get; } = new Dictionary<string, object>();

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine("FieldValues(");
            foreach (var entry in Fields)
            {
                builder.AppendLine($"  {entry.Key}: {entry.Value} ({entry.Value?.GetType()})");
            }
            builder.AppendLine(")");
            return builder.ToString();
        }
    }

    /// <summary>
    /// Another example of custom data type.
    /// </summary>
    class LocaleString
    {
        private Dictionary<string, string> values =
            new Dictionary<string, string>();

        public void Add(string language, string value)
        {
            values.Add(language, value);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("LocaleString(");
            foreach (var entry in values)
            {
                builder.Append($"'{entry.Key}': '{entry.Value}', ");
            }
            builder.Append(")");
            return builder.ToString();

        }
    }

    /// <summary>
    /// Another custom data type.
    /// </summary>
    class CvlValue
    {
        [JsonProperty("cvlId", Required = Required.Always)]
        public string CvlId { get; set; }

        [JsonProperty("value", Required = Required.Always)]
        public string Value { get; set; }

        public override string ToString()
        {
            return $"CvlValue(CvlId='{CvlId}', Value='{Value}')";
        }
    }

    /// <summary>
    /// Custom type Json converter.
    /// </summary>
    class FieldValuesConvertor : JsonConverter<FieldValues>
    {
        public override FieldValues ReadJson(JsonReader reader, Type objectType, FieldValues existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var fieldValues = new FieldValues();

            var rootObject = serializer.Deserialize<JObject>(reader);
            foreach (var item in rootObject)
            {
                var name = item.Key;
                var value = item.Value;
                if (item.Value.Type == JTokenType.Object)
                {
                    var o = item.Value.ToObject<JObject>();
                    if (o.ContainsKey("cvlId"))
                    {
                        fieldValues.Fields.Add(name, o.ToObject<CvlValue>(serializer));
                    }
                    else
                    {
                        fieldValues.Fields.Add(name, o.ToObject<LocaleString>(serializer));
                    }
                }
                else
                {
                    fieldValues.Fields.Add(name, value.ToObject<object>());
                }
            }
            return fieldValues;
        }

        public override void WriteJson(JsonWriter writer, FieldValues value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Custom type Json converter.
    /// </summary>
    class LocaleStringConvertor : JsonConverter<LocaleString>
    {
        public override LocaleString ReadJson(JsonReader reader, Type objectType, LocaleString existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonObj = serializer.Deserialize<JObject>(reader);
            var localeString = new LocaleString();
            foreach (var item in jsonObj)
            {
                var language = item.Key;
                var value = item.Value.Value<string>();
                localeString.Add(language, value);
            }
            return localeString;
        }

        public override void WriteJson(JsonWriter writer, LocaleString value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// Example of parsing Json command and invoke according method with reflection.
    /// </summary>
    static class CommandDispatchExample
    {
        static IDictionary<string, MethodInfo> methods = new Dictionary<string, MethodInfo>();

        static CommandDispatchExample()
        {
            var commandClass = typeof(Commands);
            foreach (var method in commandClass.GetMethods(
                BindingFlags.Public | BindingFlags.DeclaredOnly
                | BindingFlags.Instance | BindingFlags.Static))
            {
                methods.Add(method.Name, method);
            }
        }

        public static void Run()
        {
            // Normal call.
            ParseJson(@"{
    'function': 'GetSetting',
    'args': {'name': 'SYSLOG_HOST'},
}");

            // Error: Unexpected argument 'foo'.
            ParseJson(@"{
    'function': 'GetSetting',
    'args': {'foo': 123, 'name': 'SYSLOG_HOST'},
}");

            // Error: Missing argument 'name'.
            ParseJson(@"{
    'function': 'GetSetting',
    'args': {},
}");
            
            // Normal call using default value (optional argument).
            ParseJson(@"{
    'function': 'FuncFoo',
    'args': {'foo': 12, 'bar': '2'},
}");

            // Normal call using custom data types for some arguments.
            ParseJson(@"{
    'function': 'CreateEntity',
    'args': {
        'entityTypeId': 'Product',
        'fieldValues': {
            'ProductId': 123,
            'ProductName': 'Foo',
            'ProductDescription': {
                'en': 'A Product',
                'lv': 'Produkts',
            },
            'Genre': {
                'cvlId': 'Genre',
                'value': 'A',
            },
        },
    },
}");
            // Normal call using custom data type.
            ParseJson(@"{
    'function': 'FuncBar',
    'args': {
        'bar': 123,
        'localeString': {'en': 'Some string'},
    },
}");

            // Error: No function field.
            ParseJson(@"{
    'method': 'GetSetting',
    'args': {'name': 'SYSLOG_HOST'},
}");

            // Error: No Args field.
            ParseJson(@"{
    'function': 'GetSetting',
    'kwargs': {'name': 'SYSLOG_HOST'},
}");

            // Error: Invalid JSON.
            ParseJson("}invalid json{");

            // Error: Missing argument 'name'.
            // Error: Or unexpected argument 'isbn'?
            ParseJson(@"{
    'function': 'GetSetting',
    'args': {'isbn': '12345'},
}");

            // Error: Unexpected argument 'isbn'.
            ParseJson(@"{
    'function': 'GetSetting',
    'args': {'name': 'SYSLOG_HOST', 'isbn': '12345'},
}");

            // Error: Missing 2 arguments: foo, bar
            ParseJson(@"{
    'function': 'FuncFoo',
    'args': {},
}");

            // Error: Missing 2 arguments: foo, bar
            ParseJson(@"{
    'function': 'FuncFoo',
    'args': {},
}");

            // Error: Unable to parse 'foo'.
            ParseJson(@"{
    'function': 'FuncFoo',
    'args': {'foo': '1a', 'bar': null},
}");

            // Normal call passing null to string.
            ParseJson(@"{
    'function': 'FuncFoo',
    'args': {'foo': 1, 'bar': null},
}");

            // Error: Unable to parse 'foo'.
            ParseJson(@"{
    'function': 'FuncFoo',
    'args': {'foo': null, 'bar': null},
}");
        }

        private static void ParseJson(string json)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            serializer.MissingMemberHandling = MissingMemberHandling.Error;
            
            serializer.Converters.Add(new FieldValuesConvertor());
            serializer.Converters.Add(new LocaleStringConvertor());

            Console.WriteLine("\n>>> Parse");

            // Check Json validity.
            JObject command = null;
            try
            {
                command = JObject.Parse(json);
            } catch (JsonReaderException e)
            {
                Console.WriteLine($"Invalid Json: {e.Message}");
                return;
            }
            
            // Check if function value.
            var function = command["function"]?.Value<string>();
            if (function == null)
            {
                Console.WriteLine("No Function found");
                return;
            }
            Console.WriteLine($"Function: '{function}'");

            // Check if args value.
            var args = (JObject) command["args"];
            if (args == null)
            {
                Console.WriteLine("No args found");
                return;
            }
            Console.WriteLine($"Args found: {args}");

            // Get actual method to call.
            if (!methods.ContainsKey(function))
            {
                Console.WriteLine($"Unknown function: '{function}'");
                return;
            }
            var method = methods[function];
            Console.WriteLine($"Invoking method: {method.Name}");
            
            // Go through reflected arguments, get values from json.
            var paramList = new List<ParameterInfo>(method.GetParameters());
            var argList = new List<object>();
            while (paramList.Count > 0)
            {
                var p = paramList[0];
                paramList.RemoveAt(0);

                if (!args.ContainsKey(p.Name) && p.HasDefaultValue)
                {
                    argList.Add(Type.Missing);
                    continue;
                }

                if (!args.ContainsKey(p.Name))
                {
                    Console.WriteLine($"Missing required argument: " +
                        $"'{p.Name}' ({p.ParameterType})");
                    return;
                }

                try
                {
                    var v = args.GetValue(p.Name).ToObject(p.ParameterType, serializer);
                    argList.Add(v);
                    args.Remove(p.Name);
                }
                catch (JsonSerializationException e)
                {
                    Console.WriteLine($"Unable to parse argument '{p.Name}' as {p.ParameterType}");
                    return;
                }
                catch (JsonReaderException e) {
                    Console.WriteLine($"Unable to parse argument '{p.Name}' as {p.ParameterType}");
                    return;
                }
            }

            if (args.Count > 0)
            {
                var prop = args.First.ToObject<JProperty>();
                Console.WriteLine($"Got an unexpected keyword argument '{prop.Name}'");

                return;
            }

            method.Invoke(null, argList.ToArray());
        }
    }
}
