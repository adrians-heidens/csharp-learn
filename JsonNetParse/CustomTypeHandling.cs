using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace JsonNetParse
{
    /// <summary>
    /// Example of adding custom type handling.
    /// </summary>
    class ARecord
    {
        public string Name { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int Ttl { get; set; }

        public Ipv4 Value { get; set; }

        public override string ToString()
        {
            return $"ARecord(Name={Name}, Ttl={Ttl}, Value={Value})";
        }
    }

    class Ipv4
    {
        public byte b1 { get; set; }

        public byte b2 { get; set; }

        public byte b3 { get; set; }

        public byte b4 { get; set; }

        public override string ToString()
        {
            return $"Ipv4({b1}, {b2}, {b3}, {b4})";
        }
    }

    class Ipv4Converter : JsonConverter<Ipv4>
    {
        public override Ipv4 ReadJson(JsonReader reader, Type objectType, Ipv4 existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
            {
                throw new Exception("Not a string token");
            }
            var ipv4Str = (string)reader.Value;
            var parts = ipv4Str.Split(".");
            if (parts.Length != 4)
            {
                throw new Exception("Unexpected string value");
            }
            byte b1 = byte.Parse(parts[0]);
            byte b2 = byte.Parse(parts[1]);
            byte b3 = byte.Parse(parts[2]);
            byte b4 = byte.Parse(parts[3]);
            return new Ipv4 { b1 = b1, b2 = b2, b3 = b3, b4 = b4 };
        }

        public override void WriteJson(JsonWriter writer, Ipv4 value, JsonSerializer serializer)
        {
            writer.WriteValue($"{value.b1}.{value.b2}.{value.b3}.{value.b4}");
        }
    }

    class CustomTypeHandling
    {
        public static void Run()
        {
            var json = @"{
    'Name': 'foo.test',
    'Ttl': 123,
    'Value': '127.0.0.2',
}";

            var arecord = JsonConvert.DeserializeObject<ARecord>(json, new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Error,
                Converters = new List<JsonConverter> { new Ipv4Converter() }
            });
            Console.WriteLine(arecord);

            arecord = new ARecord
            {
                Name = "spam.test",
                Ttl = 123,
                Value = new Ipv4 { b1 = 127, b2 = 0, b3 = 0, b4 = 1 },
            };
            json = JsonConvert.SerializeObject(arecord, new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Error,
                Converters = new List<JsonConverter> { new Ipv4Converter() }
            });
            Console.WriteLine(json);

            
        }
    }
}
