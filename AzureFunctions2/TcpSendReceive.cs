using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;

namespace AzureFunctions2
{
    public static class TcpSendReceive
    {
        [FunctionName("TcpSendReceive")]
        public static IActionResult Run(
            [HttpTrigger("get")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("TcpSendReceive");

            string host = "f00.lv";
            int port = 8080;

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.SendTimeout = 2000;
            socket.ReceiveTimeout = 2000;

            IPAddress ipv4Address = GetIpv4Address(host);
            IPEndPoint endPoint = new IPEndPoint(ipv4Address, port);

            socket.Connect(endPoint);

            var textSend = "Hello!";
            log.LogInformation($"Sending TCP data. Message to send: '{textSend}'");
            socket.Send(Encoding.ASCII.GetBytes(textSend));

            log.LogInformation("Receiving TCP data.");
            var buffer = new byte[512];
            var c = socket.Receive(buffer);

            var textReceive = Encoding.ASCII.GetString(buffer, 0, c);

            log.LogInformation($"Received {c} bytes.");
            log.LogInformation($"Received message: '{textReceive}'");

            socket.Close();
            
            return new OkObjectResult("OK");
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
