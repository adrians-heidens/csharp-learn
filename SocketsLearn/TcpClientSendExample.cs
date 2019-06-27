using System;
using System.Net.Sockets;
using System.Text;

namespace SocketsLearn
{
    static class TcpClientSendExample
    {
        public static void Run()
        {
            TcpClient client = new TcpClient("192.168.56.11", 8000);
            Console.WriteLine(client.Connected);

            SetKeepAlive(client);

            NetworkStream stream = client.GetStream();
            var message = "Hello from TcpClient!\n";
            var payload = Encoding.UTF8.GetBytes(message);
            stream.Write(payload);

            Console.WriteLine("Press key to send message again...");
            Console.ReadKey();

            stream.Write(payload);

            Console.WriteLine("Press key to close socket...");
            Console.ReadKey();

            stream.Close();
            client.Close();
        }

        private static void SetKeepAlive(TcpClient client)
        {
            /* Set TCP keep-alive values. */
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

            byte[] optionInValue = new byte[sizeof(uint) * 3];

            /* Keep-alive is enabled or disabled. */
            BitConverter.GetBytes(1).CopyTo(optionInValue, 0);

            /* The timeout, in milliseconds, with no activity until the first keep-alive packet is sent. */
            BitConverter.GetBytes(5 * 1000).CopyTo(optionInValue, sizeof(uint));

            /* The interval, in milliseconds, between when successive keep-alive packets are sent if no acknowledgement is received. */
            BitConverter.GetBytes(1000).CopyTo(optionInValue, sizeof(uint) * 2);

            client.Client.IOControl(IOControlCode.KeepAliveValues, optionInValue, null);
        }
    }
}
