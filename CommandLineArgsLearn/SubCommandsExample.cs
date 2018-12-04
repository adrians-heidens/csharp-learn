using System;
using System.Linq;
using System.Collections.Generic;

namespace CommandLineArgsLearn
{
    /// <summary>
    /// An example of a very minimal subcommand supported argument parser.
    /// </summary>
    class SubCommandsExample
    {
        public static void Run(string[] args)
        {
            // If --help found anywhere then print global usage help.
            if (args.Any(x => x == "--help"))
            {
                var name = AppDomain.CurrentDomain.FriendlyName;
                PrintUsage(name);
                Environment.Exit(0);
            }

            // Find command name.
            var commandIndex = Array.FindIndex(args, x => x == "--command");
            if (commandIndex == -1 || args.Length <= commandIndex + 1)
            {
                Console.WriteLine("No command specified.");
                Environment.Exit(2);
            }
            var command = args[commandIndex + 1];

            // Global arguments before --command.
            var globalArgs = ParseArgs(args.SkipLast(args.Length - commandIndex));
            
            // We could check if required parameters are present etc.
            string username = null;
            string password = null;
            try
            {
                username = globalArgs["username"][0];
                password = globalArgs["password"][0];
            } catch (Exception)
            {
                Console.WriteLine("Wrong usage.\n");
                PrintUsage(AppDomain.CurrentDomain.FriendlyName);
                Environment.Exit(2);
            }

            Console.WriteLine("Global parameters:");
            var lines = globalArgs.Select(entry => $"  {entry.Key}: {string.Join(',', entry.Value)}");
            Console.WriteLine(string.Join(Environment.NewLine, lines));

            // Command specific arguments.
            var commandArgs = ParseArgs(args.Skip(commandIndex + 2));

            Console.WriteLine("Command parameters:");
            lines = commandArgs.Select(entry => $"  {entry.Key}: {string.Join(',', entry.Value)}");
            Console.WriteLine(string.Join(Environment.NewLine, lines));

            // Executing commands.
            if (command == "foo")
            {
                SubCommandFoo.Run(username, password, commandArgs);
            }
            else if (command == "bar")
            {
                SubCommandBar.Run(username, commandArgs);
            }
            else
            {
                Console.WriteLine($"Unknown command: '{command}'");
                Environment.Exit(2);
            }

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
        
        // Parse arguments to a dictionary.
        private static IDictionary<string, IList<string>> ParseArgs(IEnumerable<string> args)
        {
            var argsDict = new Dictionary<string, IList<string>>();
            IList<string> current = null;
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
            return argsDict;
        }

        // A basic help message.
        private static void PrintUsage(string prog)
        {
            Console.WriteLine($@"
Usage: {prog} [global-options] COMMAND [command-options]

An example command line app which provides many subcommands.

Global options:

  --username USERNAME  Username for some service.
  --password PASSWORD  A password to log in.

Commands:

  foo
    Description about what foo does.

    --target  Some kind of parameter.

  bar
    Description about what bar does.
".Trim());
        }
    }
}
