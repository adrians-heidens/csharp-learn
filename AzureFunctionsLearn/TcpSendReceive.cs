using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace AzureFunctionsLearn
{
    /// <summary>
    /// Sending TCP data and receiving response.
    /// </summary>
    public static class TcpSendReceive
    {
        [FunctionName("TcpSendReceive")]
        public static HttpResponseMessage Run([HttpTrigger("get")]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("TcpSendReceive");
            
            string host = "f00.lv";
            int port = 8080;

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.SendTimeout = 2000;
            socket.ReceiveTimeout = 2000;

            IPAddress ipv4Address = GetIpv4Address(host);
            IPEndPoint endPoint = new IPEndPoint(ipv4Address, port);

            socket.Connect(endPoint);

            var textSend = "Hello!";
            log.Info($"Sending TCP data. Message to send: '{textSend}'");
            socket.Send(Encoding.ASCII.GetBytes(textSend));

            log.Info("Receiving TCP data.");
            var buffer = new byte[512];
            var c = socket.Receive(buffer);

            var textReceive = Encoding.ASCII.GetString(buffer, 0, c);

            log.Info($"Received {c} bytes.");
            log.Info($"Received message: '{textReceive}'");

            socket.Close();

            return req.CreateResponse(HttpStatusCode.OK, "OK");
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
