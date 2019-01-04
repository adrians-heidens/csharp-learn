using System;

namespace AzureServiceBusLearn
{
    class Program
    {
        static void Main(string[] args)
        {
            //SendMessages.Run();
            //ReceiveMessages.Run();
            //SendToTopic.Run();
            //ReceiveFromSubscription.Run();
            ReceiveFromSubSync.Run();
        }
    }
}
