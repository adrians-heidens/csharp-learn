using System;
using System.Net.Sockets;
using System.Text;

namespace SocketsLearn
{
    static class TcpClientSendExample
    {
        public static void Run()
        {
            TcpClient client = new TcpClient("f00.lv", 8000);
            Console.WriteLine(client.Connected);

            NetworkStream stream = client.GetStream();
            var message = "Hello from TcpClient!\n";
            var payload = Encoding.UTF8.GetBytes(message);
            stream.Write(payload);

            stream.Close();
            client.Close();
        }
    }
}
