using System;
using System.Collections.Generic;
using System.Text;

namespace JsonNetParse.Models
{
    class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            return $"Product(name='{Name}')";
        }
    }
}
