using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace AzureFunctionsLearn
{
    public static class LoggingTestFunction
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LoggingTestFunction));

        [FunctionName("LoggingTestFunction")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req,
            TraceWriter traceWriter)
        {
            ConfigureLogger(traceWriter);

            log.Fatal("Logging log4net test (level: Fatal)");
            log.Error("Logging log4net test (level: Error)");
            log.Warn("Logging log4net test (level: Warn)");
            log.Info("Logging log4net test (level: Info)");
            log.Debug("Logging log4net test (level: Debug)");
            
            return req.CreateResponse(HttpStatusCode.OK, "OK");
        }

        private static void ConfigureLogger(TraceWriter traceWriter)
        {
            ILoggerRepository repository = LogManager.GetRepository(Assembly.GetCallingAssembly());

            // Static data might be retained between function calls -- it might
            // already be configured.
            if (repository.Configured)
            {
                return;
            }

            PatternLayout timeLayout = new PatternLayout();
            timeLayout.ConversionPattern = "%utcdate %level %logger: %message%newline";
            timeLayout.ActivateOptions();

            PatternLayout noTimeLayout = new PatternLayout();
            noTimeLayout.ConversionPattern = "%level %logger: %message%newline";
            noTimeLayout.ActivateOptions();

            // Not sure about usage of this.
            PatternLayout identity = new PatternLayout();
            identity.ConversionPattern = "azure";
            identity.ActivateOptions();

            RemoteSyslogAppender remoteSyslogAppender = new RemoteSyslogAppender();
            remoteSyslogAppender.Layout = noTimeLayout;
            //remoteSyslogAppender.RemoteAddress = GetIpv4Address("f00.lv");
            remoteSyslogAppender.RemoteAddress = IPAddress.Parse("192.188.1.100"); // rsyslog address.
            remoteSyslogAppender.RemotePort = 514;
            remoteSyslogAppender.Facility = RemoteSyslogAppender.SyslogFacility.User;
            remoteSyslogAppender.Identity = identity;
            remoteSyslogAppender.ActivateOptions();

            TraceWriterAppender traceWriterAppender = new TraceWriterAppender(traceWriter);
            traceWriterAppender.Layout = noTimeLayout;
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
            throw new Exception($"Could not resolve hostname '{hostName}' to Ipv4 address");
        }
    }
}
