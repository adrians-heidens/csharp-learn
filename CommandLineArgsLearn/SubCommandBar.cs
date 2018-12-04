using System;
using System.Collections.Generic;

namespace CommandLineArgsLearn
{
    static class SubCommandBar
    {
        public static void Run(string username, IDictionary<string, IList<string>> args)
        {
            Console.WriteLine($"Exec 'bar' (username={username})");
        }
    }
}
