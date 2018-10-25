using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace AzureFunctionsLearn
{
    /// <summary>
    /// Experiment on sending UDP message when function executes.
    /// </summary>
    public static class SendUdpFunction
    {
        [FunctionName("SendUdpFunction")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req,
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            var message = DateTime.Now.ToString();
            SendUdpMessage("f00.lv", 8080, message);

            return req.CreateResponse(HttpStatusCode.OK, message);
        }

        private static void SendUdpMessage(string host, int port, string message)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.SendTimeout = 1000;
            socket.ReceiveTimeout = 1000;

            IPAddress ipv4Address = GetIpv4Address(host);
            IPEndPoint endPoint = new IPEndPoint(ipv4Address, port);

            socket.Connect(endPoint);
            socket.Send(Encoding.ASCII.GetBytes(message));
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
