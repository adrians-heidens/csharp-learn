using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Util;
using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace Log4NetLearn.Syslog
{
    public class SyslogTcpTlsQueueAppender : AppenderSkeleton
    {
        public enum SyslogSeverity
        {
            Emergency = 0,
            Alert = 1,
            Critical = 2,
            Error = 3,
            Warning = 4,
            Notice = 5,
            Informational = 6,
            Debug = 7
        };
        
        public enum SyslogFacility
        {
            Kernel = 0,
            User = 1,
            Mail = 2,
            Daemons = 3,
            Authorization = 4,
            Syslog = 5,
            Printer = 6,
            News = 7,
            Uucp = 8,
            Clock = 9,
            Authorization2 = 10,
            Ftp = 11,
            Ntp = 12,
            Audit = 13,
            Alert = 14,
            Clock2 = 15,
            Local0 = 16,
            Local1 = 17,
            Local2 = 18,
            Local3 = 19,
            Local4 = 20,
            Local5 = 21,
            Local6 = 22,
            Local7 = 23
        }

        public Encoding Encoding { get; set; } = Encoding.UTF8;

        public string Hostname { get; set; } = "localhost";

        public int Port { get; set; } = 6514;

        public string CertificatePath { get; set; }

        public byte[] CertificateData { get; set; }

        public string CertificatePassword { get; set; }

        private TcpClient tcpClient;

        private SslStream sslStream;

        public RotatingQueue<byte[]> Queue { get; } = new RotatingQueue<byte[]>();

        public SyslogTcpTlsQueueAppender()
        {
        }
        
        public PatternLayout Identity
        {
            get { return m_identity; }
            set { m_identity = value; }
        }
        
        public SyslogFacility Facility
        {
            get { return m_facility; }
            set { m_facility = value; }
        }
        
        public void AddMapping(LevelSeverity mapping)
        {
            m_levelMapping.Add(mapping);
        }
        
        protected override void Append(LoggingEvent loggingEvent)
        {
            try
            {
                // Priority
                int priority = GeneratePriority(m_facility, GetSeverity(loggingEvent.Level));

                // Identity
                string identity;

                if (m_identity != null)
                {
                    identity = m_identity.Format(loggingEvent);
                }
                else
                {
                    identity = loggingEvent.Domain;
                }

                // Message. The message goes after the tag/identity
                string message = RenderLoggingEvent(loggingEvent);

                Byte[] buffer;
                int i = 0;
                char c;

                StringBuilder builder = new StringBuilder();

                while (i < message.Length)
                {
                    // Clear StringBuilder
                    builder.Length = 0;

                    // Write priority
                    builder.Append('<');
                    builder.Append(priority);
                    builder.Append('>');

                    // Write identity
                    builder.Append(identity);
                    builder.Append(": ");

                    for (; i < message.Length; i++)
                    {
                        c = message[i];

                        // Accept only visible ASCII characters and space. See RFC 3164 section 4.1.3
                        if (((int)c >= 32) && ((int)c <= 126))
                        {
                            builder.Append(c);
                        }
                        // If character is newline, break and send the current line
                        else if ((c == '\r') || (c == '\n'))
                        {
                            // Check the next character to handle \r\n or \n\r
                            if ((message.Length > i + 1) && ((message[i + 1] == '\r') || (message[i + 1] == '\n')))
                            {
                                i++;
                            }
                            i++;
                            break;
                        }
                    }

                    // Grab as a byte array
                    builder.Append('\n');
                    buffer = Encoding.GetBytes(builder.ToString());
                    Queue.Add(buffer);
                }
            }
            catch (Exception e)
            {
                ErrorHandler.Error(
                    "Unable to send logging event to remote syslog " +
                    Hostname +
                    " on port " +
                    Port + ".",
                    e,
                    ErrorCode.WriteFailure);
            }
        }

        private static bool ValidateServerCertificate(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private void Connect()
        {
            tcpClient = new TcpClient();
            
            try
            {
                tcpClient.Connect(Hostname, Port);
            }
            catch (Exception)
            {
                tcpClient.Close();
                tcpClient = null;
                return;
            }

            sslStream = new SslStream(
                tcpClient.GetStream(),
                false,
                new RemoteCertificateValidationCallback(ValidateServerCertificate)
                );

            X509CertificateCollection certificates = new X509CertificateCollection();
            if (CertificateData != null)
            {
                certificates.Add(new X509Certificate(CertificateData, CertificatePassword));
            }
            else
            {
                certificates.Add(new X509Certificate(CertificatePath, CertificatePassword));
            }

            try
            {
                sslStream.AuthenticateAsClient(
                    targetHost: Hostname,
                    clientCertificates: certificates,
                    checkCertificateRevocation: false);
            }
            catch (Exception)
            {
                sslStream.Close();
                tcpClient.Close();
                sslStream = null;
                tcpClient = null;
            }
        }

        private void ReconnectIfNeeded()
        {
            if (tcpClient == null)
            {
                Reconnect();
            }
        }

        private void Reconnect()
        {
            int waitTime = 10;
            while (true)
            {
                Connect();
                if (tcpClient != null)
                {
                    return;
                }
                Thread.Sleep(Math.Min(waitTime, 1000));
                waitTime *= 2;
            }
        }

        // This is run by queue consuming thread.
        private void ConsumeQueue()
        {
            ReconnectIfNeeded();
            
            while (true)
            {
                var buffer = Queue.Get(); // Wait for item.

                try
                {
                    sslStream.Write(buffer);
                }
                catch (IOException)  // Sending error, close connection.
                {
                    sslStream.Close();
                    tcpClient.Close();

                    tcpClient = null;
                    sslStream = null;
                }

                ReconnectIfNeeded();  // Reconnect if closed.
            }
        }

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            m_levelMapping.ActivateOptions();

            Thread t1 = new Thread(new ThreadStart(ConsumeQueue));
            t1.IsBackground = true;
            t1.Start();
        }
        
        virtual protected SyslogSeverity GetSeverity(Level level)
        {
            LevelSeverity levelSeverity = m_levelMapping.Lookup(level) as LevelSeverity;
            if (levelSeverity != null)
            {
                return levelSeverity.Severity;
            }

            //
            // Fallback to sensible default values
            //

            if (level >= Level.Alert)
            {
                return SyslogSeverity.Alert;
            }
            else if (level >= Level.Critical)
            {
                return SyslogSeverity.Critical;
            }
            else if (level >= Level.Error)
            {
                return SyslogSeverity.Error;
            }
            else if (level >= Level.Warn)
            {
                return SyslogSeverity.Warning;
            }
            else if (level >= Level.Notice)
            {
                return SyslogSeverity.Notice;
            }
            else if (level >= Level.Info)
            {
                return SyslogSeverity.Informational;
            }
            // Default setting
            return SyslogSeverity.Debug;
        }
        
        public static int GeneratePriority(SyslogFacility facility, SyslogSeverity severity)
        {
            if (facility < SyslogFacility.Kernel || facility > SyslogFacility.Local7)
            {
                throw new ArgumentException("SyslogFacility out of range", "facility");
            }

            if (severity < SyslogSeverity.Emergency || severity > SyslogSeverity.Debug)
            {
                throw new ArgumentException("SyslogSeverity out of range", "severity");
            }

            unchecked
            {
                return ((int)facility * 8) + (int)severity;
            }
        }
        
        private SyslogFacility m_facility = SyslogFacility.User;
        
        private PatternLayout m_identity;

        private LevelMapping m_levelMapping = new LevelMapping();
        
        public class LevelSeverity : LevelMappingEntry
        {
            private SyslogSeverity m_severity;
            
            public SyslogSeverity Severity
            {
                get { return m_severity; }
                set { m_severity = value; }
            }
        }
    }
}
