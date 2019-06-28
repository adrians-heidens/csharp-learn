using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Log4NetLearn.Syslog
{
    static class SyslogTcpXmlConfigExample
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SyslogTcpXmlConfigExample));

        private static void ConfigureLogger()
        {
            ILoggerRepository repository = LogManager.GetRepository(Assembly.GetCallingAssembly());

            var path = Path.Combine("Syslog", "Log4netConfig.xml");
            
            using (var stream = File.OpenRead(path))
            {
                XmlConfigurator.Configure(repository, stream);
            }
        }

        public static void Run()
        {
            ConfigureLogger();

            log.Fatal("Logging log4net test (level: Fatal)");
            log.Error("Logging log4net test (level: Error)");
            log.Warn("Logging log4net test (level: Warn)");
            log.Info("Logging log4net test (level: Info)");
            log.Debug("Logging log4net test (level: Debug)");
        }
    }
}
