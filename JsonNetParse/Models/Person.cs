using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonNetParse.Models
{
    class Person
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("fullname")]
        public string Fullname { get; set; }

        public override string ToString()
        {
            return $"Person(Fullname='{Fullname}')";
        }
    }
}
