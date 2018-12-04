using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandLineArgsLearn
{
    /// <summary>
    /// The most minimal and very crude command line argument parsing.
    /// </summary>
    static class SimpleExample
    {
        public static void Run(string[] args)
        {
            // Check if help requested.
            if (args.Any(x => x == "--help"))
            {
                var name = AppDomain.CurrentDomain.FriendlyName;
                PrintUsage(name);
                Environment.Exit(0);
            }

            // A makeshift command line argument parser which expects only
            // options starting with double dashes and no position params.

            // Gather arguments and their values in a dictionary.
            var argsDict = new Dictionary<string, List<string>>();
            List<string> current = null;
            foreach (var arg in args)
            {
                if (arg.StartsWith("--"))
                {
                    var key = arg.Substring(2);
                    if (argsDict.ContainsKey(key))
                    {
                        current = argsDict[key];
                    }
                    else
                    {
                        current = new List<string>();
                        argsDict[key] = current;
                    }
                }
                else
                {
                    current.Add(arg);
                }
            }

            string email = argsDict["email"][0];
            string password = argsDict["password"][0];
            string uri = argsDict.ContainsKey("uri") ? argsDict["uri"][0] : null;
            bool isSandbox = argsDict.ContainsKey("sandbox");
            string customer = argsDict["customer"][0];
            string environment = argsDict["environment"][0];
            List<string> packages = argsDict["replace-package"];
            bool restartService = argsDict.ContainsKey("restart-service");

            Console.WriteLine($"email: {email}");
            Console.WriteLine($"password: {password}");
            Console.WriteLine($"uri: {uri}");
            Console.WriteLine($"isSandbox: {isSandbox}");
            Console.WriteLine($"customer: {customer}");
            Console.WriteLine($"environment: {environment}");
            foreach (var package in packages)
            {
                Console.WriteLine($"package: {package}");
            }
            Console.WriteLine($"restartService: {restartService}");

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        static void PrintUsage(string prog)
        {
            Console.WriteLine($@"
Usage: {prog} [options]

A command line tool for interacting with inRiver Control Center website.

Options:

  --email EMAIL           Email for inRiver control center.
  --password PASSWORD     Password for inRiver control center.
  --uri URI               URI for inRiver control center (use default if omited).
  --sandbox               Use default sandbox control center URI.
  --customer CUST         Customer safename.
  --environment ENV       Environment safename.
  --replace-package PATH  Replace existing inRiver package with the same name.
  --restart-service       Restart inRiver service.

Examples:

  {prog}
    --email user@example.test
    --password 1234
    --sandbox
    --customer cust1
    --environment sandbox1
    --replace-package C:\tmp\SomePackage.zip
    --restart-service

".Trim());
        }
    }
}
