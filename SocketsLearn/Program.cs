using System;

namespace SocketsLearn
{
    class Program
    {
        static void Main(string[] args)
        {
            //UdpSendReceive.Run();
            //TcpSendReceive.Run();
            //TcpClientSendExample.Run();
            TcpClientSendTlsExample.Run();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
