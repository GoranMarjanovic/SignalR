using GDi.Abstract.SignalR.DTO;
using GDi.Abstract.SignalR.Interfaces;
using log4net;
using Microsoft.AspNet.SignalR;
using System.Reflection;
using System.Threading.Tasks;

namespace GDi.Abstract.SignalR.Server.Interfaces
{
    abstract public class ABaseHubServer : Hub<ISendHub>
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public virtual void Send_AddMessage(string name, string message)
        {
            logger.Debug("Sending AddMessage " + name + " " + message);
            Clients.All.Send_AddMessage(name, message);
        }

        public virtual void Send_Heartbeat()
        {
            logger.Debug("HubSync Sending Heartbeat");
            Clients.All.Send_Heartbeat();
        }

        public virtual void Send_ExampleDTO(ExampleDTO exampleDto)
        {
            logger.Debug("HubSync Sending ExampleDTO: " + exampleDto.ToString());
            Clients.All.Send_ExampleDTO(exampleDto);
        }

        public override Task OnConnected()
        {
            logger.Debug("Hub OnConnected " + Context.ConnectionId);
            return (base.OnConnected());
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            logger.Debug("Hub OnDisconnected " + Context.ConnectionId);
            return (base.OnDisconnected(stopCalled));
        }

        public override Task OnReconnected()
        {
            logger.Debug("Hub OnReconnected " + Context.ConnectionId);
            return (base.OnReconnected());
        }

    }
}
