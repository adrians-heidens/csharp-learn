using System;

namespace Log4NetLearn
{
    class Program
    {
        static void Main(string[] args)
        {
            //BasicConfigExample.Run();
            //ProgrammaticConfig.Run();
            RemoteSyslogExample.Run();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
