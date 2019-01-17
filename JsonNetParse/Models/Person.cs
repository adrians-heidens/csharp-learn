using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JsonNetParse.Models
{
    class Person
    {
        [JsonProperty("id", Required = Required.AllowNull)]
        public int Id { get; set; }

        [JsonProperty("fullname", Required = Required.Always)]
        public string Fullname { get; set; }

        public override string ToString()
        {
            return $"Person(Id={Id}, Fullname='{Fullname}')";
        }
    }
}
