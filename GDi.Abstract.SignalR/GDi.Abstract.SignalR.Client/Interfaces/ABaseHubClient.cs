using log4net;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Reflection;
using System.Threading;

namespace GDi.Abstract.SignalR.Client.Interfaces
{
    abstract public class ABaseHubClient
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected HubConnection _hubConnection;
        protected IHubProxy _signalrHubProxy;

        public string HubConnectionUrl { get; set; }
        public string HubProxyName { get; set; }
        public TraceLevels HubTraceLevel { get; set; }
        public System.IO.TextWriter HubTraceWriter { get; set; }

        private string _url;
        private bool _forceReconection;
        private int _reconnectionInterval = 10000;

        // flag for stopping condition
        private volatile bool _isStopping = false;

        /// <summary>
        /// Method which should be implemented on the real implementation to start the client.
        /// Do the necessarry binding to the methods for listen to before start the hub.
        /// </summary>
        public abstract void StartHub();

        public void CloseHub()
        {
            _isStopping = true;
            _hubConnection.Stop();
            _hubConnection.Dispose();
        }

        public ConnectionState State
        {
            get { return _hubConnection.State; }
        }

        /// <summary>
        /// Initialize hub client parameters.
        /// </summary>
        /// <param name="url">URL for the remote connection.</param>
        /// <param name="forceReconection">Force reconection even if SignalR recconect finish.</param>
        /// <param name="reconnectionInterval">Recconection interval used when force reconnection parameter is used.</param>
        protected void Init(string url, bool forceReconection = true, int reconnectionInterval = 30000)
        {
            this._url = url;
            this._forceReconection = forceReconection;
            this._reconnectionInterval = reconnectionInterval;

            logger.Info("Init connection to " + this._url);

            _hubConnection = new HubConnection(HubConnectionUrl)
            {
                TraceLevel = HubTraceLevel,
                TraceWriter = HubTraceWriter
            };

            _signalrHubProxy = _hubConnection.CreateHubProxy(HubProxyName);

            _hubConnection.Received += HubConnection_Received;
            _hubConnection.Reconnected += HubConnection_Reconnected;
            _hubConnection.Reconnecting += HubConnection_Reconnecting;
            _hubConnection.StateChanged += HubConnection_StateChanged;
            _hubConnection.Error += HubConnection_Error;
            _hubConnection.ConnectionSlow += HubConnection_ConnectionSlow;
            _hubConnection.Closed += HubConnection_Closed;

        }

        protected void StartHubInternal()
        {
            if (_isStopping) return;
            try
            {
                _hubConnection.Start().Wait();
            }
            catch (Exception ex)
            {
                logger.Warn(ex.Message + " " + ex.StackTrace);
            }

        }

        void ReInitConnectionInternal()
        {
            logger.Info("Reinit connection to " + this._url + " in " + _reconnectionInterval);
            //CloseHub();
            Thread.Sleep(_reconnectionInterval);

            Init(this._url);

            StartHubInternal();
        }

        void HubConnection_Closed()
        {
            logger.Info("HubConnection_Closed New State:" + _hubConnection.State + " " + _hubConnection.ConnectionId);

            // this part is used for repetitive reconnection after time defined in the signalr implementation
            if (!_isStopping && _forceReconection)
            {
                ReInitConnectionInternal();
            }
        }

        /* OTHER STATES, can be used, but in this case, only for informational purpose that states can be used */

        void HubConnection_ConnectionSlow()
        {
            logger.Debug("HubConnection_ConnectionSlow New State:" + _hubConnection.State + " " + _hubConnection.ConnectionId);
        }

        void HubConnection_Error(Exception obj)
        {
            logger.Info("HubConnection_Error New State:" + _hubConnection.State + " " + _hubConnection.ConnectionId);
        }

        void HubConnection_StateChanged(StateChange obj)
        {
            logger.Debug("HubConnection_StateChanged New State:" + _hubConnection.State + " " + _hubConnection.ConnectionId);
        }

        void HubConnection_Reconnecting()
        {
            logger.Info("HubConnection_Reconnecting New State:" + _hubConnection.State + " " + _hubConnection.ConnectionId);
        }

        void HubConnection_Reconnected()
        {
            logger.Info("HubConnection_Reconnected New State:" + _hubConnection.State + " " + _hubConnection.ConnectionId);
        }

        void HubConnection_Received(string obj)
        {
            logger.Debug("HubConnection_Received New State:" + _hubConnection.State + " " + _hubConnection.ConnectionId);
        }
    }
}
