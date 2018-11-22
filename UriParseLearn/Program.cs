using System;

namespace UriParseLearn
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri uri = new Uri("http://www.example.test:8080/path/to/file.png?foo=1&bar=2#spam");
            Console.WriteLine(uri);

            Console.WriteLine($"Scheme: '{ uri.Scheme }'");
            Console.WriteLine($"Host: '{ uri.Host }'");
            Console.WriteLine($"Port: '{ uri.Port }'");
            Console.WriteLine($"AbsolutePath: '{ uri.AbsolutePath }'");
            Console.WriteLine($"Query: '{ uri.Query }'");
            Console.WriteLine($"Fragment: '{ uri.Fragment }'");

            uri = new Uri("/", UriKind.Relative);

            Console.WriteLine($"ToString: '{ uri.ToString() }'");

            Console.WriteLine("End.");
            Console.ReadKey();
        }
    }
}
