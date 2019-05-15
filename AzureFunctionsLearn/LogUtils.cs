using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository;
using Microsoft.Azure.WebJobs.Host;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AzureFunctionsLearn
{
    static class LogUtils
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void ConfigureLogger(TraceWriter traceWriter)
        {
            ILoggerRepository repository = LogManager.GetRepository(Assembly.GetCallingAssembly());

            // Static data might be retained between function calls -- it might
            // already be configured.
            if (repository.Configured)
            {
                return;
            }

            PatternLayout localLayout = new PatternLayout();
            localLayout.ConversionPattern = "%message%newline";
            localLayout.ActivateOptions();

            PatternLayout remoteLayout = new PatternLayout();
            remoteLayout.ConversionPattern = "%level %logger: %message%newline";
            remoteLayout.ActivateOptions();

            // Not sure about usage of this.
            PatternLayout identity = new PatternLayout();
            identity.ConversionPattern = "azure";
            identity.ActivateOptions();

            RemoteSyslogAppender remoteSyslogAppender = new RemoteSyslogAppender();
            remoteSyslogAppender.Layout = remoteLayout;
            remoteSyslogAppender.RemoteAddress = GetIpv4Address("f00.lv");
            remoteSyslogAppender.RemotePort = 514;
            remoteSyslogAppender.Facility = RemoteSyslogAppender.SyslogFacility.User;
            remoteSyslogAppender.Identity = identity;
            remoteSyslogAppender.ActivateOptions();

            TraceWriterAppender traceWriterAppender = new TraceWriterAppender(traceWriter);
            traceWriterAppender.Layout = localLayout;
            traceWriterAppender.ActivateOptions();

            IBasicRepositoryConfigurator configurableRepository = repository as IBasicRepositoryConfigurator;
            configurableRepository.Configure(remoteSyslogAppender, traceWriterAppender);
        }

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
            return null;
        }
    }
}
