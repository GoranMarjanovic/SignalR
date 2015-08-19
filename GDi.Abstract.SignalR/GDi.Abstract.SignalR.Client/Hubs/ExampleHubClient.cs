using GDi.Abstract.SignalR.Client.Interfaces;
using GDi.Abstract.SignalR.DTO;
using GDi.Abstract.SignalR.Interfaces;
using log4net;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Reflection;

namespace GDi.Abstract.SignalR.Client.Hubs
{
    public class ExampleHubClient : ABaseHubClient, ISendHub, IRecieveHub
    {
        /// <summary>
        /// Logger for this class. 
        /// </summary>
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// URL connection to the hub.
        /// </summary>
        private string _url;

        /// <summary>
        /// The name of the server hub to connect to.
        /// </summary>
        private static readonly string SERVER_HUB_NAME = "ExampleHub";

        /// <summary>
        /// Default c'tor.
        /// </summary>
        /// <param name="url">Connection parameter for remote signalr server hub.</param>
        public ExampleHubClient(string url)
        {
            this._url = url;
            Init();
        }

        /// <summary>
        /// Start connection to the desired hub.
        /// </summary>
        public override void StartHub()
        {
            _hubConnection.Dispose();
            Init();
        }

        /// <summary>
        /// This part is crucial for client initialization.
        /// Probably some parts still can be moved to the abstract,.. this is part for the investigation and additional TODO..
        /// </summary>
        private void Init()
        {
            HubConnectionUrl = this._url;
            HubProxyName = SERVER_HUB_NAME; // this is the name of the server hub
            HubTraceLevel = TraceLevels.Messages;// TraceLevels.Events;
            HubTraceWriter = Console.Out;

            base.Init(this._url);

            // configure listeners for the messages, if not selected, all messages will be received
            _signalrHubProxy.On<string, string>("Send_AddMessage", Recieve_AddMessage);
            _signalrHubProxy.On("Send_Heartbeat", Recieve_Heartbeat);
            _signalrHubProxy.On<ExampleDTO>("Send_ExampleDTO", Recieve_ExampleDTO);
            
            base.StartHubInternal();
        }

        /* IMPLEMENTATION FROM THE IRecieveHub ; direction server --> client */

        public void Recieve_AddMessage(string name, string message)
        {
            logger.Debug("Recieved addMessage: " + name + ": " + message);
        }

        public void Recieve_Heartbeat()
        {
            logger.Debug("Recieved heartbeat");
        }

        public void Recieve_ExampleDTO(DTO.ExampleDTO exampleDto)
        {
            logger.Debug("Recieved ExampleDTO => " + exampleDto.ToString());
        }

        /* IMPLEMENTATION FROM THE ISendHub ; direction client --> server */

        public void Send_AddMessage(string name, string message)
        {
            throw new NotImplementedException();
        }

        public void Send_Heartbeat()
        {
            throw new NotImplementedException();
        }

        public void Send_ExampleDTO(DTO.ExampleDTO exampleDto)
        {
            throw new NotImplementedException();
        }
    }
}
