using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Util;
using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace Log4NetLearn.Syslog
{


    /// <summary>
    /// Appender for log4net to send logs to remote syslog server via TCP
    /// protocolo using TLS transport.
    /// </summary>
    /// <remarks>
    /// This class is taken from log4net built-in RemoteSyslogAppender and
    /// modified to work with TCP, TLS.
    /// </remarks>
    public class SyslogTlsQueueAppender : AppenderSkeleton
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

        /// <summary>
        /// Syslog server hostname.
        /// </summary>
        public string Hostname { get; set; } = "localhost";

        /// <summary>
        /// Syslog server port.
        /// </summary>
        public int Port { get; set; } = 6514;

        /// <summary>
        /// Path to PKCS12 certificate file.
        /// </summary>
        public string CertificatePath { get; set; }

        /// <summary>
        /// Binary data of PKCS12 certificate.
        /// </summary>
        public byte[] CertificateData { get; set; }

        /// <summary>
        /// Certificate encoded as base64 string.
        /// </summary>
        public string CertificateBase64 { get; set; }

        /// <summary>
        /// Certificate password.
        /// </summary>
        public string CertificatePassword { get; set; }

        /// <summary>
        /// Enable PING message sending.
        /// </summary>
        public bool PingEnabled { get; set; } = false;

        /// <summary>
        /// Miliseconds between PING messages.
        /// </summary>
        public int PingTime { get; set; } = 60 * 1000;

        /// <summary>
        /// Enable TCP keepalive feature.
        /// </summary>
        public bool TcpKeepAliveEnabled { get; set; } = false;

        /// <summary>
        /// TCP keepalive time value (and interval) in miliseconds.
        /// </summary>
        public int TcpKeepAliveTime { get; set; } = 60 * 1000;

        private X509Certificate2Collection certificates = new X509Certificate2Collection();

        private TcpClient tcpClient;

        private SslStream sslStream;

        private DiscardingQueue<byte[]> Queue { get; } = new DiscardingQueue<byte[]>();

        public SyslogTlsQueueAppender() { }

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
            // TODO: Timeout values.
            tcpClient = new TcpClient { ReceiveTimeout = 1000, SendTimeout = 1000 };

            try
            {
                tcpClient.Connect(Hostname, Port);
            }
            catch (Exception e)
            {
                tcpClient.Close();
                tcpClient = null;

                AppendDebugMessage(e.Message);
                return;
            }

            if (TcpKeepAliveEnabled)
            {
                AppendDebugMessage($"Set TCP keepalive to {TcpKeepAliveTime}");
                SetTcpKeepAlive(tcpClient.Client, TcpKeepAliveTime);
            }

            sslStream = new SslStream(
                tcpClient.GetStream(),
                false,
                new RemoteCertificateValidationCallback(ValidateServerCertificate)
                );

            try
            {
                sslStream.AuthenticateAsClient(
                    targetHost: Hostname,
                    clientCertificates: certificates,
                    enabledSslProtocols: SslProtocols.Tls11 | SslProtocols.Tls12,
                    checkCertificateRevocation: false);
            }
            catch (Exception e)
            {
                CloseConnection();
                AppendDebugMessage(e.Message);
            }
        }

        private void ReconnectIfNeeded()
        {
            if (tcpClient == null)
            {
                ReconnectUntilSucceed();
            }
        }

        private void ReconnectUntilSucceed()
        {
            int maxDelay = 4096; // miliseconds.
            int delay = 16; // miliseconds.
            while (true)
            {
                Connect();
                if (tcpClient != null)
                {
                    return;
                }
                Thread.Sleep(delay);
                delay = Math.Min(delay * 2, maxDelay);
            }
        }

        private void CloseConnection()
        {
            sslStream.Close();
            tcpClient.Close();
            tcpClient = null;
            sslStream = null;
        }

        private void WriteUntilSucceed(byte[] buffer)
        {
            while (true)
            {
                try
                {
                    sslStream.Write(buffer);
                    sslStream.Flush();
                    return;
                }
                catch (Exception e)
                {
                    CloseConnection();
                    AppendDebugMessage(e.Message);
                    ReconnectUntilSucceed();
                }
            }
        }

        // This is run by queue consuming thread.
        private void ConsumeQueue()
        {
            ReconnectIfNeeded();

            while (true)
            {
                var buffer = Queue.Get(); // Wait for item.
                WriteUntilSucceed(buffer);
            }
        }

        private void AppendDebugMessage(string message)
        {
            Append(new LoggingEvent(new LoggingEventData
            {
                Level = Level.Debug,
                Message = message,
                LoggerName = "log4net",
            }));
        }

        private void Ping()
        {
            while (true)
            {
                Thread.Sleep(PingTime);
                Append(new LoggingEvent(new LoggingEventData
                {
                    Level = Level.Debug,
                    Message = "-- PING --",
                    LoggerName = "log4net",
                }));
            }
        }

        private void SetTcpKeepAlive(Socket socket, int tcpKeepAliveTime)
        {
            /* Enable TCP keep-alive. */
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

            byte[] optionInValue = new byte[sizeof(uint) * 3];

            /* Keep-alive is enabled or disabled. */
            BitConverter.GetBytes(1).CopyTo(optionInValue, 0);

            /* The timeout, in milliseconds, with no activity until the first keep-alive packet is sent. */
            BitConverter.GetBytes(tcpKeepAliveTime).CopyTo(optionInValue, sizeof(uint));

            /* The interval, in milliseconds, between when successive keep-alive packets are sent if no acknowledgement is received. */
            BitConverter.GetBytes(tcpKeepAliveTime).CopyTo(optionInValue, sizeof(uint) * 2);

            /* Set TCP keep-alive values. */
            socket.IOControl(IOControlCode.KeepAliveValues, optionInValue, null);
        }

        private void LoadCertificates()
        {
            if (CertificateData == null && CertificateBase64 != null)
            {
                CertificateData = Convert.FromBase64String(CertificateBase64);
            }

            if (CertificateData != null)
            {
                certificates.Add(new X509Certificate2(CertificateData, CertificatePassword));
            }
            else
            {
                certificates.Add(new X509Certificate2(CertificatePath, CertificatePassword));
            }
        }

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            m_levelMapping.ActivateOptions();

            LoadCertificates();

            Thread messageConsumerThread = new Thread(new ThreadStart(ConsumeQueue));
            messageConsumerThread.IsBackground = true;
            messageConsumerThread.Start();

            if (PingEnabled)
            {
                Thread pingThread = new Thread(new ThreadStart(Ping));
                pingThread.IsBackground = true;
                pingThread.Start();
            }
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
