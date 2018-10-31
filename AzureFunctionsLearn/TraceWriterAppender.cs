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
            var text = ">>> " + RenderLoggingEvent(loggingEvent);
            if (loggingEvent.Level == Level.Error)
            {
                _traceWriter.Error(text);
            }
            else
            {
                _traceWriter.Info(text);
            }
        }
    }
}
