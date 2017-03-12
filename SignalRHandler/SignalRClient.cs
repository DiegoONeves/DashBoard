using Microsoft.AspNet.SignalR.Client;
using System.Collections.Generic;

namespace SignalRHandler
{
    public class SignalRClient
    {
        private HubConnection HubConnection { get; }
        private IHubProxy HubProxy { get; }

        public SignalRClient(string url, string hub, IDictionary<string, string> queryString = null)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                HubConnection = new HubConnection(url);

                if (queryString != null)
                {
                    foreach (var kvPair in queryString)
                    {
                        HubConnection.Headers.Add(kvPair);
                    }
                }

                HubProxy = HubConnection.CreateHubProxy(hub);
            }
        }

        public void Invoke(string method, params object[] args)
        {
            try
            {
                HubConnection.Start().Wait();
                HubProxy?.Invoke(method, args);
                HubConnection.Stop();
            }
            catch (System.Exception)
            {

            }

        }
    }
}
