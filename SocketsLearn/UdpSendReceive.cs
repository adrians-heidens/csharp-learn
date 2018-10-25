using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketsLearn
{
    /// <summary>
    /// Send text mesage via UDP protocol to an address and wait for a response.
    /// </summary>
    static class UdpSendReceive
    {
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

        public static void Run()
        {
            string host = "127.0.0.1";
            int port = 8080;

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.SendTimeout = 2000;
            socket.ReceiveTimeout = 2000;

            IPAddress ipv4Address = GetIpv4Address(host);
            IPEndPoint endPoint = new IPEndPoint(ipv4Address, port);

            socket.Connect(endPoint);

            var textSend = "Hello!";
            Console.WriteLine($"Sending UDP data. Message to send: '{textSend}'");
            socket.Send(Encoding.ASCII.GetBytes(textSend));

            Console.WriteLine("Receiving UDP data.");
            var buffer = new byte[512];
            var c = socket.Receive(buffer);

            var textReceive = Encoding.ASCII.GetString(buffer, 0, c);

            Console.WriteLine($"Received {c} bytes.");
            Console.WriteLine($"Received message: '{textReceive}'");
        }
    }
}
