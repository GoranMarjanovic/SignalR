using GDi.Abstract.SignalR.Client.Hubs;
using System;

namespace TestClient
{
    class Program
    {
        private static ExampleHubClient hub;

        static void Main(string[] args)
        {
            StartSignalRClientConnection();

            var key = Console.ReadLine();
            if (key == "quit")
            {
                //context.Clients.All.Send("Server closed", key);
                //hub.Send("serverClosed", key);
            }
        }

        private static void StartSignalRClientConnection()
        {
            hub = new ExampleHubClient("http://localhost:8088/");
        }

        private static void StopSignalRClientConnection()
        {
            hub.CloseHub();
        }
    }
}
