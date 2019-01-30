using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonNetParse
{
    /// <summary>
    /// Example of dealing with KeyValuePair type.
    /// </summary>
    static class KeyValuePairExample
    {
        public static void Run()
        {
            var pair1 = new KeyValuePair<string, object>("foo", "bar");
            var json = JsonConvert.SerializeObject(pair1, Formatting.Indented);
            Console.WriteLine(json);

            var pair2 = new KeyValuePair<string, object>("spam", 12);
            var pair3 = new KeyValuePair<string, object>("other", 4.32);

            var list = new List<KeyValuePair<string, object>>();
            list.Add(pair1);
            list.Add(pair2);
            list.Add(pair3);

            json = JsonConvert.SerializeObject(list, Formatting.Indented);
            Console.WriteLine(json);

            json = @"
[
  {
    ""Key"": ""foo"",
    ""Value"": ""bar""
  },
  {
    ""Key"": ""spam"",
    ""Value"": 12
  },
  {
    ""Key"": ""other"",
    ""Value"": 4.32
  }
]
";

            var obj = JsonConvert.DeserializeObject<IList<KeyValuePair<string, object>>>(json);
            Console.WriteLine(string.Join(", ", obj.Select(x => $"{x.Key}:{x.Value}")));
        }
    }
}
