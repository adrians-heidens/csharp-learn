using System.Collections.Generic;

namespace JsonNetParse.Models
{
    public class Bundle
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<BundleItem> Items { get; set; } = new List<BundleItem>();

        public override string ToString()
        {
            var itemsString = string.Join(", ", Items);
            return $"Bundle(Id={Id}, Name={Name}, Items={itemsString})";
        }
    }
}
