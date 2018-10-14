using System;
using System.Collections.Generic;

namespace FieldPatternStudy
{
    /// <summary>
    /// Demonstrates the idea of a list of values wrapped in a container
    /// (like a field) where these values get processed at some level outside
    /// the container. There it can be decided how to deal with each particular
    /// type of field.
    /// 
    /// In this example we see a ValueField wich contains some primitive type
    /// where these values are being processed in one way. On the other hand
    /// a special field is defined (NameField) which is being handled in a
    /// special way.
    /// 
    /// Alternatives: visitor pattern, if/else branches.
    /// 
    /// Also it demonstrates usage of dynamic type where Builder.Add is being
    /// called on that type and decided which overloaded method to use on the
    /// runtime.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Builder builder = new Builder();
            
            // Sequencial list of fields.
            var fields = new List<IField>();
            fields.Add(new ValueField<uint>(12345));
            fields.Add(new ValueField<ushort>(67));
            fields.Add(new ValueField<string>("bar"));
            fields.Add(new NameField("foo"));

            foreach (var field in fields)
            {
                Console.WriteLine(field);

                // Handle NameField in a different way.
                if (field.GetType() == typeof(NameField))
                {
                    var value = (string) field.Value;
                    builder.Add(value.ToUpper());
                }
                else
                {
                    builder.Add(field.Value);
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
