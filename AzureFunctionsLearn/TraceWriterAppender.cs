using log4net.Appender;
using log4net.Core;
using Microsoft.Azure.WebJobs.Host;

namespace AzureFunctionsLearn
{
    class TraceWriterAppender : AppenderSkeleton
    {
        private TraceWriter _traceWriter = null;

        public TraceWriterAppender(TraceWriter traceWriter) : base()
        {
            _traceWriter = traceWriter;
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            var text = RenderLoggingEvent(loggingEvent); // Rendered specified layout.
            var exception = loggingEvent.ExceptionObject;
            var level = loggingEvent.Level;
            var message = loggingEvent.RenderedMessage; // Just a message.

            if (level == Level.Debug)
            {
                _traceWriter.Verbose(text);
            }
            else if (level == Level.Info)
            {
                _traceWriter.Info(text);
            }
            else if (level == Level.Warn)
            {
                _traceWriter.Warning(text);
            }
            else if (level == Level.Error)
            {
                _traceWriter.Error(text, exception);
            }
            else if (level == Level.Fatal)
            {
                _traceWriter.Error(text, exception);
            }
            else
            {
                _traceWriter.Info(text);
            }
        }
    }
}
