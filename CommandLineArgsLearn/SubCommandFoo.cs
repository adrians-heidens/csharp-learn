using System;
using System.Collections.Generic;

namespace CommandLineArgsLearn
{
    static class SubCommandFoo
    {
        public static void Run(string username, string password, IDictionary<string, IList<string>> args)
        {
            Console.WriteLine($"Exec 'foo' (username={username}, password={password})");
        }
    }
}
