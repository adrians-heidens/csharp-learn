using System.Collections.Generic;

namespace JsonNetParse.Models
{
    public class Car
    {
        public string Model { get; set; }

        public string  Manufacturer { get; set; }

        // We need this for deserialization but not when serialize.
        public IDictionary<string, string> Details { get; set; } =
            new Dictionary<string, string>();

        // We could do some tricks here or leave it all to be done with later
        // JObject manipulations.
        public string Layout =>
            Details.TryGetValue("Layout", out string value) ? value : null;

        public string Class =>
            Details.TryGetValue("Class", out string value) ? value : null;

        public override string ToString()
        {
            return "Car(" +
                $"Model={Model}, " +
                $"Manufacturer={Manufacturer}" +
                ")";
        }
    }
}
