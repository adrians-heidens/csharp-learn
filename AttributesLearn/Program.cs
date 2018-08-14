using System;

namespace AttributesLearn
{
    class Program
    {
        static void PrintTypeInfo(System.Type type)
        {
            Console.WriteLine($"Type '{type.Name}' info:");
            Console.WriteLine($"  Fullname: {type.FullName}");
            
            Console.WriteLine("  Properties:");
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                Console.WriteLine($"    {property.Name} ({property.PropertyType.Name})");
            }
            
            Console.WriteLine("  Attributes:");
            var attributes = System.Attribute.GetCustomAttributes(type);
            foreach (var attribute in attributes)
            {
                Console.WriteLine("    " + attribute);
            }
        }

        static void Main(string[] args)
        {
            var person = new Person { Name = "john", Fullname = "John Smith", Password = "1234" };
            Console.WriteLine(person);

            PrintTypeInfo(typeof(Person));

            Console.ReadKey();
        }
    }
}
