using System;
using System.Reflection;
using System.Linq;

namespace ReflectionLearn
{
    static class DllFromPathExample
    {
        public static void Run()
        {
            string path = @"C:\tmp\Some.dll";
            // Dependencies are needed to read assembly like this.
            var assembly = Assembly.LoadFrom(path); // This loads dependencies.

            Console.WriteLine("Types:");
            foreach (var type in assembly.ExportedTypes)
            {
                var values = type.GetInterfaces().Select(x => x.FullName);
                if (values.Any(x => x.StartsWith("Foo.Bar.")))
                {
                    Console.WriteLine($"  Type: {type}, {string.Join(",", values)}");
                }
            }
        }
    }
}
