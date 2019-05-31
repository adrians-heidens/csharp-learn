using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace JsonNetParse.ObjectPropertySetter
{
    static class ObjectPropertySetterExample
    {
        public static void Run()
        {
            // We have some object.
            var person = new Person("foo@bar.local", "abc");

            // And json representing property updates.
            var json = @"{
    ""Email"": ""foo@example.test"",
    ""Score"": 123
}";

            Console.WriteLine(person);

            // Interpret Json as IDictionary<string, object> and to update.
            var values = JsonConvert.DeserializeObject<IDictionary<string, object>>(json);
            foreach (var entry in values)
            {
                PropertyInfo propertyInfo = person.GetType().GetProperty(entry.Key);
                var value = Convert.ChangeType(entry.Value, propertyInfo.PropertyType);
                propertyInfo.SetValue(person, value);
            }

            Console.WriteLine(person);
        }
    }
}
