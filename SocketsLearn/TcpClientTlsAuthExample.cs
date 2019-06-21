using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SocketsLearn
{
    /// <summary>
    /// A connection with mandatory TLS client cert authentication.
    /// </summary>
    static class TcpClientTlsAuthExample
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
            TcpClient client = new TcpClient("127.0.0.1", 8080);
            Console.WriteLine($"Client connected: {client.Connected}");

            SslStream sslStream = new SslStream(
                client.GetStream(),
                false,
                new RemoteCertificateValidationCallback(ValidateServerCertificate)
                );

            /* To convert PEM cert and key to PKCS12:
             * openssl pkcs12 -export -out cert.p12 -in cert.pem -inkey key.pem
             */
            X509CertificateCollection certificates = new X509CertificateCollection();
            certificates.Add(new X509Certificate(@"C:\keys\cert.p12", "12345678"));
            
            sslStream.AuthenticateAsClient(
                targetHost: "",
                clientCertificates: certificates,
                checkCertificateRevocation: false);

            var message = "Hello from TcpClient using TLS auth!\n";
            var payload = Encoding.UTF8.GetBytes(message);
            sslStream.Write(payload);

            sslStream.Close();
            client.Close();
        }
    }
}
