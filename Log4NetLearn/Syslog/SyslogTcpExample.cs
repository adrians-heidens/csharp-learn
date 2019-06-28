using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository;
using System.Reflection;
using System.Threading;

namespace Log4NetLearn.Syslog
{
    static class SyslogTcpExample
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SyslogTcpExample));

        private static void ConfigureLogger()
        {
            ILoggerRepository repository = LogManager.GetRepository(Assembly.GetCallingAssembly());

            PatternLayout timeLayout = new PatternLayout();
            timeLayout.ConversionPattern = "%utcdate %level %logger: %message%newline";
            timeLayout.ActivateOptions();

            PatternLayout noTimeLayout = new PatternLayout();
            noTimeLayout.ConversionPattern = "%level %logger: %message%newline";
            noTimeLayout.ActivateOptions();

            ConsoleAppender consoleAppender = new ConsoleAppender();
            consoleAppender.Layout = timeLayout;
            consoleAppender.ActivateOptions();

            // Not sure about usage of this.
            PatternLayout identity = new PatternLayout();
            identity.ConversionPattern = "log4net";
            identity.ActivateOptions();

            var remoteSyslogAppender = new SyslogTlsQueueAppender();
            remoteSyslogAppender.Layout = noTimeLayout;
            remoteSyslogAppender.Facility = SyslogTlsQueueAppender.SyslogFacility.User;
            remoteSyslogAppender.Identity = identity;
            remoteSyslogAppender.Hostname = "192.168.56.11";
            remoteSyslogAppender.Port = 6514;
            remoteSyslogAppender.CertificatePath = @"c:\keys\client.p12";
            remoteSyslogAppender.CertificatePassword = "123456";
            remoteSyslogAppender.ActivateOptions();
            
            IBasicRepositoryConfigurator configurableRepository = repository as IBasicRepositoryConfigurator;
            configurableRepository.Configure(consoleAppender, remoteSyslogAppender);
        }

        public static void Run()
        {
            ConfigureLogger();

            for (int i = 0; i < 100; i++)
            {
                log.Info($"Message: {i}");
                Thread.Sleep(1000);
            }

            log.Fatal("Logging log4net test (level: Fatal)");
            log.Error("Logging log4net test (level: Error)");
            log.Warn("Logging log4net test (level: Warn)");
            log.Info("Logging log4net test (level: Info)");
            log.Debug("Logging log4net test (level: Debug)");
        }
    }
}
