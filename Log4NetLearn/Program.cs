using log4net;
using log4net.Config;
using System;
using System.Reflection;

namespace Log4NetLearn
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            BasicConfigurator.Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()));

            log.Debug("Debug message from logger");
            log.Debug("Error message from logger");

            Console.WriteLine("End.");
            Console.ReadKey();
        }
    }
}
