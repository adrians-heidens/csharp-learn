using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace JsonNetParse
{
    /// <summary>
    /// JSON can be read and written using JObject, JArray, JValue, JToken.
    /// Called "LINQ to JSON" in the docs.
    /// </summary>
    public class JObjectLearn
    {
        public static void Run()
        {
            var jsonString = "{astring: 'foo', anumber: 12, " +
                "anobject: {foo: 1, bar: 'abc'}, " +
                "anarray: [1, 2, 3], aboolean: true, anull: null}";

            // Parse as JSON object (can also be an array or dynamic type).
            var jobject = JObject.Parse(jsonString);
            
            // Iterate JSON object and print values.
            foreach (var item in jobject)
            {
                var key = item.Key;
                var value = item.Value;
                Console.WriteLine($"> {key}: {value} ({value.GetType()})");
            }

            // Get JToken value and cast as concrete type.
            bool aboolean = jobject.GetValue("aboolean").Value<bool>();
            Console.WriteLine(aboolean);

            // Get JObject property as IDictionary.
            var anobject = jobject.GetValue("anobject").ToObject<IDictionary<string, object>>();
            Console.WriteLine(anobject.GetType());
            foreach (var entry in anobject)
            {
                Console.WriteLine($"  {entry.Key}: {entry.Value}");
            }

            // Dynamic feature.
            dynamic ob = jobject["anobject"];
            int obFoo = ob.foo;
            string obBar = ob.bar;
            Console.WriteLine($"{obFoo}, {obBar}");
        }
    }
}
