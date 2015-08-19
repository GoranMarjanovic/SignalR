using GDi.Abstract.SignalR.DTO;
using GDi.Abstract.SignalR.Server.Interfaces;
using log4net;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;


namespace GDi.Abstract.SignalR.Server.Hubs
{
    [HubName("ExampleHub")]
    public class ExampleHubServer : ABaseHubServer
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static List<string> ConectedClients = new List<string>();

        public override void Send_AddMessage(string name, string message)
        {
            if (ConectedClients.Count == 0) return; // no sense to send data to no one

            logger.Debug(string.Format("Sending ... {0}", message));
            // put the needed hub server
            var context = GlobalHost.ConnectionManager.GetHubContext<ExampleHubServer>();
            context.Clients.All.Send_AddMessage(name, message);
        }

        public override void Send_ExampleDTO(ExampleDTO exampleDto)
        {
            if (ConectedClients.Count == 0) return; // no sense to send data to no one

            logger.Debug(string.Format("Sending ... {0}", exampleDto.ToString()));

            // put the needed hub server
            var context = GlobalHost.ConnectionManager.GetHubContext<ExampleHubServer>();

            context.Clients.All.Send_ExampleDTO(exampleDto);
        }

        public override void Send_Heartbeat()
        {
            if (ConectedClients.Count == 0) return; // no sense to send data to no one

            logger.Debug("Sending heartbeat");
            // put the needed hub server
            var context = GlobalHost.ConnectionManager.GetHubContext<ExampleHubServer>();
            context.Clients.All.Send_Heartbeat();
        }

        public override Task OnConnected()
        {
            logger.Info(string.Format("[{0}] Client '{1}' connected.", DateTime.Now.ToString("dd-mm-yyyy hh:MM:ss"), Context.ConnectionId));

            TryAddClient();

            return (base.OnConnected());
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            logger.Info(string.Format("[{0}] Client '{1}' disconnected.", DateTime.Now.ToString("dd-mm-yyyy hh:MM:ss"), Context.ConnectionId));

            TryRemoveClient();

            return (base.OnDisconnected(stopCalled));
        }

        public override Task OnReconnected()
        {
            logger.Info(string.Format("[{0}] Client '{1}' re-connected.", DateTime.Now.ToString("dd-mm-yyyy hh:MM:ss"), Context.ConnectionId));

            TryAddClient();

            return (base.OnReconnected());
        }

        // PRIVATE HELPER METHODS

        private void TryAddClient()
        {
            if (ConectedClients.IndexOf(Context.ConnectionId) == -1)
            {
                ConectedClients.Add(Context.ConnectionId);
            }
            PrintoutConnections();
        }

        private void TryRemoveClient()
        {
            if (ConectedClients.IndexOf(Context.ConnectionId) > -1)
            {
                ConectedClients.Remove(Context.ConnectionId);
            }
            PrintoutConnections();
        }

        private void PrintoutConnections()
        {
            logger.Info(string.Format("Connected clients: {0}", ConectedClients.Count));
        }
    }
}
