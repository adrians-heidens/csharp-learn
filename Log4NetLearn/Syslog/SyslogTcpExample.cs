using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

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

            SyslogTcpAppender remoteSyslogAppender = new SyslogTcpAppender();
            remoteSyslogAppender.Layout = noTimeLayout;
            remoteSyslogAppender.Facility = SyslogTcpAppender.SyslogFacility.User;
            remoteSyslogAppender.Identity = identity;
            remoteSyslogAppender.Hostname = "f00.lv";
            remoteSyslogAppender.Port = 515;
            remoteSyslogAppender.ActivateOptions();

            //RemoteSyslogAppender remoteSyslogAppender = new RemoteSyslogAppender();
            //remoteSyslogAppender.Layout = noTimeLayout;
            //remoteSyslogAppender.RemoteAddress = GetIpv4Address("f00.lv");
            //remoteSyslogAppender.RemotePort = 514;
            //remoteSyslogAppender.Facility = RemoteSyslogAppender.SyslogFacility.User;
            //remoteSyslogAppender.Identity = identity;
            //remoteSyslogAppender.ActivateOptions();

            IBasicRepositoryConfigurator configurableRepository = repository as IBasicRepositoryConfigurator;
            configurableRepository.Configure(consoleAppender, remoteSyslogAppender);
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
