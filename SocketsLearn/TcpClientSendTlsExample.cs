using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SocketsLearn
{
    static class TcpClientSendTlsExample
    {
        public static bool ValidateServerCertificate(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public static void Run()
        {
            TcpClient client = new TcpClient("f00.lv", 8000);
            Console.WriteLine($"Client connected: {client.Connected}");

            SslStream sslStream = new SslStream(
                client.GetStream(),
                false,
                new RemoteCertificateValidationCallback(ValidateServerCertificate));

            sslStream.AuthenticateAsClient("");
            
            var message = "Hello from TcpClient using TLS!\n";
            var payload = Encoding.UTF8.GetBytes(message);
            sslStream.Write(payload);

            sslStream.Close();
            client.Close();
        }
    }
}
