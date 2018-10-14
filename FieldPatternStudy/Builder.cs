using System;

namespace FieldPatternStudy
{
    class Builder
    {
        public void Add(uint value)
        {
            Console.WriteLine($"Builder add uint {value}");
        }

        public void Add(ushort value)
        {
            Console.WriteLine($"Builder add ushort {value}");
        }

        public void Add(string value)
        {
            Console.WriteLine($"Builder add string '{value}'");
        }
    }
}
