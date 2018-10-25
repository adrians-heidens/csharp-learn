using log4net;
using log4net.Config;
using System.Reflection;

namespace Log4NetLearn
{
    static class BasicConfigExample
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        public static void Run()
        {
            BasicConfigurator.Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()));

            log.Debug("Debug message from logger");
            log.Error("Error message from logger");
        }
    }
}
