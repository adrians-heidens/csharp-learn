using System;
using System.Reflection;

namespace ReflectionLearn
{
    /// <summary>
    /// This is an example demonstrating some reflection features.
    /// </summary>
    static class UsageExample
    {
        public static void Run()
        {
            Assembly assembly = typeof(UsageExample).Assembly;
            // Or get executing assembly:
            //Assembly assembly = Assembly.GetExecutingAssembly();

            Console.WriteLine(assembly);

            foreach (var type in assembly.DefinedTypes)
            {
                Console.WriteLine($"  Type: {type}");
            }

            foreach (var module in assembly.GetModules())
            {
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

            // Demonstrate invoking methods on object instance using
            // reflection InvokeMember
            Console.WriteLine("----");

            var controller = new Controller("a/connection/string");

            controller.DoFoo();
            controller.DoBar(spam: "foo");

            object controllerAsObject = controller;

            Console.WriteLine("----");

            var t = controllerAsObject.GetType();
            t.InvokeMember("DoFoo",
                BindingFlags.InvokeMethod | BindingFlags.OptionalParamBinding, null,
                controllerAsObject, new object[] { });
            t.InvokeMember("DoBar",
                BindingFlags.InvokeMethod | BindingFlags.OptionalParamBinding, null,
                controllerAsObject, new object[] { "test", Type.Missing });

            Console.WriteLine("----");

            // Create instance and invoke method on it using reflection.
            var o = assembly.CreateInstance(
                "ReflectionLearn.Controller", false, BindingFlags.CreateInstance, null,
                new object[] { "/other/conn/string" }, null, null);
            t.InvokeMember("DoBar",
                BindingFlags.InvokeMethod | BindingFlags.OptionalParamBinding, null,
                o, new object[] { "test2", 12 });
        }
    }
}
