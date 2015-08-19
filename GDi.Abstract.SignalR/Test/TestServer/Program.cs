using GDi.Abstract.SignalR.DTO;
using GDi.Abstract.SignalR.Server.Hubs;
using log4net;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using System;
using System.Reflection;
using System.Threading;

namespace TestServer
{
    class Program
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        static void Main(string[] args)
        {
            //string listenOnUrl = "http://*:8088"; // listen on all available interfaces, but must be started with admin priviledges

            string listenOnUrl = "http://localhost:8088;http://127.0.0.1:8088";
            StartOptions options = new StartOptions();

            string[] urls = listenOnUrl.Split(';');

            foreach (var item in urls)
            {
                options.Urls.Add(item);
            }
            /*
            options.Urls.Add("http://localhost:8088");
            options.Urls.Add("http://127.0.0.1:8088");
            options.Urls.Add(string.Format("http://{0}:8088", Environment.MachineName));
            */
            ExampleHubServer hub = null;
            try
            {
                WebApp.Start<Startup>(options);

                hub = new ExampleHubServer();
                logger.Info("Server running !");

                // Make long polling connections wait a maximum of 110 seconds for a
                // response. When that time expires, trigger a timeout command and
                // make the client reconnect.
                GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(110);

                // Wait a maximum of 30 seconds after a transport connection is lost
                // before raising the Disconnected event to terminate the SignalR connection.
                GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(30); //30

                // For transports other than long polling, send a keepalive packet every
                // 10 seconds. 
                // This value must be no more than 1/3 of the DisconnectTimeout value.
                GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds(10); //10
            }
            catch (Exception)
            {
                throw;
            }

            for (int i = 0; i < 150; i++)
            {
                hub.Send_AddMessage("AdminUser", "message No." + i);
                ExampleDTO exampleDTO = new ExampleDTO()
                {
                    MyProperty1 = "Some prop 1",
                    MyProperty2 = "Some prop 2"
                };
                hub.Send_ExampleDTO(exampleDTO);

                // for eg. we can send heartbeat also..
                //hub.Send_Heartbeat();

                Thread.Sleep(5000);
            }

            logger.Info("Done with broadcasting");

            var key = Console.ReadLine();
            if (key == "quit")
            {
                //context.Clients.All.Send("Server closed", key);
                //hub.Send("serverClosed", key);
            }
        }
    }
}
