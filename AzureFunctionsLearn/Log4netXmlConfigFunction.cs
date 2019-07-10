using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace AzureFunctionsLearn
{
    /*
     * Demonstrates log4net remote syslog configuration from XML.
     * 
     * Xml:
     *
<?xml version="1.0" encoding="utf-8" ?>
<log4net>
  <appender name="SyslogUdp" type="log4net.Appender.RemoteSyslogAppender">
    <remoteAddress value="some.syslog.test" />
    <remotePort value="514" />
    <layout type="log4net.Layout.PatternLayout">
      <param name="ConversionPattern" value="%level %logger: %message%newline" />
    </layout>
    <identity value="azure" />
  </appender>
  <root>
    <level value="DEBUG" />
    <appender-ref ref="SyslogUdp" />
  </root>
</log4net>
     */
    public static class Log4netXmlConfigFunction
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Log4netXmlConfigFunction));

        private static readonly object logConfigLock = new object();

        private static void ConfigureLogger()
        {
            ILoggerRepository repository = LogManager.GetRepository(Assembly.GetCallingAssembly());
            if (repository.Configured)
            {
                return;
            }

            lock (logConfigLock)
            {
                if (repository.Configured)
                {
                    return;
                }
                
                var log4netXmlConfig = Environment.GetEnvironmentVariable("LOGNET_CONFIG");
                if (log4netXmlConfig == null)
                {
                    return;
                }

                var stream = new MemoryStream(Encoding.UTF8.GetBytes(log4netXmlConfig));
                XmlConfigurator.Configure(repository, stream);
            }
        }

        [FunctionName("Log4netXmlConfigFunction")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req,
            TraceWriter traceWriter
            )
        {
            traceWriter.Info("C# HTTP trigger function processed a request.");

            ConfigureLogger();

            log.Fatal("Logging log4net test (level: Fatal)");
            log.Error("Logging log4net test (level: Error)");
            log.Warn("Logging log4net test (level: Warn)");
            log.Info("Logging log4net test (level: Info)");
            log.Debug("Logging log4net test (level: Debug)");

            var log4netXmlConfig = Environment.GetEnvironmentVariable("LOGNET_CONFIG");
            return req.CreateResponse(HttpStatusCode.OK, log4netXmlConfig);
        }
    }
}
