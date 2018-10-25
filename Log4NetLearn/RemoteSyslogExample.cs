using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository;
using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace Log4NetLearn
{
    /// <summary>
    /// Example of logging remotely to syslog server.
    /// </summary>
    static class RemoteSyslogExample
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        private static IPAddress GetIpv4Address(string hostName)
        {
            IPAddress[] ipAddresses = Dns.GetHostAddresses(hostName);
            foreach (var ipAddress in ipAddresses)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ipAddress;
                }
            }
            throw new Exception($"Could not resolve hostname '{hostName}' to Ipv4 address");
        }

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

            RemoteSyslogAppender remoteSyslogAppender = new RemoteSyslogAppender();
            remoteSyslogAppender.Layout = noTimeLayout;
            remoteSyslogAppender.RemoteAddress = GetIpv4Address("f00.lv");
            remoteSyslogAppender.RemotePort = 514;
            remoteSyslogAppender.Facility = RemoteSyslogAppender.SyslogFacility.User;
            remoteSyslogAppender.Identity = identity;
            remoteSyslogAppender.ActivateOptions();

            IBasicRepositoryConfigurator configurableRepository = repository as IBasicRepositoryConfigurator;
            configurableRepository.Configure(remoteSyslogAppender, consoleAppender);
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
