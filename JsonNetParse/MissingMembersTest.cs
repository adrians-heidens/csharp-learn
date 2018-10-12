using JsonNetParse.Models;
using Newtonsoft.Json;
using System;

namespace JsonNetParse
{
    class MissingMembersTest
    {
        public static void Run()
        {
            // Additional unexpected field in json value.
            string json = "{id: 12, fullname: 'John Doe', length: 180}";
            Person person = null;
            try
            {
                person = JsonConvert.DeserializeObject<Person>(json, new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Error,
                });
            }
            catch (JsonSerializationException e) {
                Console.WriteLine(e.Message);
            }

            // Required field is not provided.
            json = "{id: 12}";
            try
            {
                person = JsonConvert.DeserializeObject<Person>(json);
            }
            catch (JsonSerializationException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
