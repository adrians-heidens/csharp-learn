using Newtonsoft.Json;
using System;

namespace JsonNetParse.StrictConverters
{
    class Person
    {
        public string Name { get; set; }

        [JsonConverter(typeof(NumberConverter))]
        public decimal Weight { get; set; }

        public DateTime BornDateTime { get; set; }

        [JsonConverter(typeof(NumberConverter))]
        public int Number { get; set; }

        public override string ToString()
        {
            return "Person(\n" +
                $"Name = {Name},\n" +
                $"Weight = {Weight},\n" +
                $"BornDateTime = {BornDateTime},\n" +
                $"Number = {Number}\n" +
                ")";
        }
    }
}
