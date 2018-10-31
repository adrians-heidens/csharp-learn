using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository;
using System.Reflection;

namespace Log4NetLearn
{
    /// <summary>
    /// Demonstrates log4net programmatic configuration.
    /// </summary>
    static class ProgrammaticConfig
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        private static void ConfigureLogger()
        {
            ILoggerRepository repository = LogManager.GetRepository(Assembly.GetCallingAssembly());
            if (repository.Configured)
            {
                throw new System.Exception("Configured already");
            }

            PatternLayout layout = new PatternLayout();
            layout.ConversionPattern = "%utcdate %level %logger: %message%newline";
            layout.ActivateOptions();

            ConsoleAppender appender = new ConsoleAppender();
            appender.Layout = layout;
            appender.ActivateOptions();

            IBasicRepositoryConfigurator configurableRepository = repository as IBasicRepositoryConfigurator;
            configurableRepository.Configure(appender);
        }

        public static void Run()
        {
            ConfigureLogger();

            log.Debug("Debug message from logger");
            log.Error("Error message from logger");
        }
    }
}
