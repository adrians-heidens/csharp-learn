using System;
using System.Collections.Generic;
using System.Text;

namespace JsonNetParse.EnumHandling
{
    class Record
    {
        public string Name { get; set; }

        public int Ttl { get; set; }

        public RecordType RType { get; set; }
    }
}
