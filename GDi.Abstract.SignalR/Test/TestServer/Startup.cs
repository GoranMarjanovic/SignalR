using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;

namespace TestServer
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            // Setup the CORS middleware to run before SignalR.
            // By default this will allow all origins. You can 
            // configure the set of origins and/or http verbs by
            // providing a cors options with a different policy.
            app.UseCors(CorsOptions.AllowAll);

            var hubConfiguration = new HubConfiguration
            {
                // You can enable JSONP by uncommenting line below.
                // JSONP requests are insecure but some older browsers (and some
                // versions of IE) require JSONP to work cross domain
                // EnableJSONP = true
                EnableDetailedErrors = true,
            };
            // Run the SignalR pipeline. We're not using MapSignalR
            // since this branch already runs under the "/signalr"
            // path.

            app.RunSignalR(hubConfiguration);
        }
    }
}
