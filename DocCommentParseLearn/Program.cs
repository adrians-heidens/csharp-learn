using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Xml.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace DocCommentParseLearn
{
    /// <summary>
    /// Initial version of documentation generator taking information from
    /// doc XML and using querying DLL file using reflection.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                var name = AppDomain.CurrentDomain.FriendlyName;
                PrintUsage(name);
                Environment.Exit(0);
            }

            var xmlDocPath = args[0];
            var dllPath = args[1];

            Console.WriteLine($"xmlDocPath='{xmlDocPath}', dllPath='{dllPath}'");

            var rootElement = XElement.Load(xmlDocPath);
            var assembly = Assembly.LoadFrom(dllPath);

            var assemblyName = (string)rootElement.Element("assembly").Element("name");
            Console.WriteLine($"# {assemblyName} Assembly\n");

            var nameRegex = new Regex(@"^(.+):([^\(]+)(\([^\)]+\))?$");

            foreach (var memberElement in rootElement.Element("members").Elements("member"))
            {
                var memberName = (string)memberElement.Attribute("name");

                var match = nameRegex.Match(memberName);
                if (match == null)
                {
                    throw new Exception($"Could not parse name '{memberName}'");
                }

                var type = match.Groups[1].Value;
                var name = match.Groups[2].Value;
                var param = match.Groups[3].Value;

                // Member.
                var className = name;
                if (type == "T")
                {
                    Console.WriteLine($"## {name} Class");

                    // Summary.
                    var summary = (string)memberElement.Element("summary");
                    if (summary != null)
                    {
                        summary = string.Join("\n", summary.Split("\n").Select(x => x.Trim()));
                        Console.WriteLine(summary);
                    }
                }
                else if (type == "M")
                {
                    var parts = name.Split(".");
                    className = string.Join(".", parts.Take(parts.Count() - 1));
                    var methodName = parts.TakeLast(1).First();
                    if (string.IsNullOrEmpty(param))
                    {
                        param = "()";
                    }

                    var typeInfo = assembly.GetType(className);
                    var methodInfo = typeInfo.GetMethod(methodName);

                    if (methodInfo == null)
                    {
                        continue;
                    }

                    Console.WriteLine($"### {typeInfo.Name}.{GetMethodStrShort(methodInfo)} Method\n");
                    Console.WriteLine("```C#");
                    Console.WriteLine($"{GetMethodCode(methodInfo)}");
                    Console.WriteLine("```");

                    // Summary.
                    var summary = (string)memberElement.Element("summary");
                    if (!string.IsNullOrEmpty(summary))
                    {
                        summary = string.Join("\n", summary.Split("\n").Select(x => x.Trim()));
                        Console.WriteLine(summary);
                    }

                    // Parameters.
                    var paramElements = memberElement.Elements("param");
                    if (paramElements.Count() > 0)
                    {
                        Console.WriteLine("#### Parameters\n");
                        foreach (var paramElement in paramElements)
                        {
                            var paramName = (string)paramElement.Attribute("name");
                            Console.WriteLine($"**{paramName}**\n");

                            var text = (string)paramElement;
                            text = string.Join("\n", text.Split("\n").Select(x => x.Trim()));
                            Console.WriteLine($"{text}\n");
                        }
                    }

                    // Returns.
                    var returns = (string)memberElement.Element("returns");
                    if (!string.IsNullOrEmpty(returns))
                    {
                        Console.WriteLine("#### Returns\n");
                        Console.WriteLine(returns);
                        Console.WriteLine();
                    }

                    Console.WriteLine();
                }
                else if (type == "P")
                {
                    var parts = name.Split(".");
                    className = string.Join(".", parts.Take(parts.Count() - 1));
                    var propertyName = parts.TakeLast(1).First();

                    var typeInfo = assembly.GetType(className);
                    var propInfo = typeInfo.GetProperty(propertyName);

                    Console.WriteLine($"### {typeInfo.Name}.{propertyName} Property");

                    // Summary.
                    var summary = (string)memberElement.Element("summary");
                    if (summary != null)
                    {
                        summary = string.Join("\n", summary.Split("\n").Select(x => x.Trim()));
                        Console.WriteLine(summary);
                    }

                    Console.WriteLine("#### Property Value\n");
                    Console.WriteLine(GetTypeStr(propInfo.PropertyType));
                    Console.WriteLine();
                }
                else
                {
                    throw new Exception($"Unexpected member type: '{type}'");
                }
            }

            //Console.WriteLine("Press key...");
            //Console.ReadKey();
        }

        private static string GetMethodCode(MethodInfo method)
        {
            var retType = method.ReturnType;
            var retName = GetTypeStr(retType);

            var parameters = method.GetParameters();
            var paramStringList = new List<string>();
            foreach (var parameter in parameters)
            {
                paramStringList.Add(GetParameterStr(parameter));
            }
            var paramString = string.Join(", ", paramStringList);

            string mod = "";
            if (method.IsStatic)
            {
                mod = " static";
            }

            return $"public{mod} {retName} {method.Name}({paramString})";
        }

        private static string GetMethodStrLong(MethodInfo method)
        {
            var pp = method.GetParameters();
            string s = "";
            if (pp.Count() > 0)
            {
                s = string.Join(",", pp.Select(x => GetTypeStr2(x.ParameterType)));
            }
            return $"{method.DeclaringType}.{method.Name}({s})";
        }

        private static string GetMethodStrShort(MethodInfo method)
        {
            var pp = method.GetParameters();
            string s = "";
            if (pp.Count() > 0)
            {
                s = string.Join(",", pp.Select(x => GetTypeStr(x.ParameterType)));
            }
            return $"{method.Name}({s})";
        }

        private static string GetParameterStr(ParameterInfo parameter)
        {
            var typeStr = GetTypeStr(parameter.ParameterType);
            return $"{typeStr} {parameter.Name}";
        }

        private static string GetTypeStr2(Type type)
        {
            if (!type.IsGenericType)
            {
                return type.FullName;
            }

            var i = type.FullName.IndexOf('`');
            var name = type.FullName.Substring(0, i);

            var genArgs = type.GetGenericArguments().Select(x => GetTypeStr2(x));
            var genArgsStr = string.Join(", ", genArgs);

            return $"{name}{{{genArgsStr}}}";
        }

        private static string GetTypeStr(Type type)
        {
            if (!type.IsGenericType)
            {
                return type.Name;
            }

            var i = type.Name.IndexOf('`');
            var name = type.Name.Substring(0, i);

            var genArgs = type.GetGenericArguments().Select(x => GetTypeStr(x));
            var genArgsStr = string.Join(", ", genArgs);

            return $"{name}<{genArgsStr}>";
        }

        static void PrintUsage(string prog)
        {
            Console.WriteLine($@"
Usage: {prog} XML DLL

Generate mardown documentation from XML documentation file and DLL.
".Trim());
        }
    }
}
