using log4net.Appender;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Log4NetLearn.Syslog
{
    class TcpAppender : AppenderSkeleton
    {
        public TcpAppender(string hostname, int port)
        {
            TcpClient client = new TcpClient(hostname, port);
            NetworkStream stream = client.GetStream();

            //TcpClient client = new TcpClient("f00.lv", 8000);
            //Console.WriteLine(client.Connected);

            //NetworkStream stream = client.GetStream();
            //var message = "Hello from TcpClient!\n";
            //var payload = Encoding.UTF8.GetBytes(message);
            //stream.Write(payload);

            //stream.Close();
            //client.Close();
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            var message = RenderLoggingEvent(loggingEvent);
            throw new NotImplementedException();
        }
    }
}
