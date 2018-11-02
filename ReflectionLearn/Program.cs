using System;
using System.Reflection;

namespace ReflectionLearn
{
    /// <summary>
    /// This is an example program demonstrating some reflection features.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Assembly assembly = typeof(Program).Assembly;
            //Assembly assembly = Assembly.GetExecutingAssembly();

            Console.WriteLine(assembly);

            foreach (var type in assembly.DefinedTypes)
            {
                Console.WriteLine($"  Type: {type}");
            }

            foreach (var module in assembly.GetModules()) {
                Console.WriteLine($"  Module: {module}");
            }

            var userType = assembly.GetType("ReflectionLearn.User");
            Console.WriteLine("User Type: " + userType);

            foreach (var constructor in userType.GetConstructors())
            {
                Console.WriteLine($"  Constructor: {constructor}");
            }

            foreach (var member in userType.GetMembers())
            {
                Console.WriteLine($"  Member: {member}");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
