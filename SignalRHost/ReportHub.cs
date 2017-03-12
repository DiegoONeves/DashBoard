using Infra.DataAccess;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace SignalRSelfHost
{
    public class ReportHub : Hub
    {
        private readonly static ConnectionMapping<string> _connections =
           new ConnectionMapping<string>();

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

        public void SubscribeUser(string user) => _connections.Add(user, Context.ConnectionId);


        public void UpdateProgress(string user)
        {
            var reports = new MongoRepository().Find(x => x.UserRequest == user
                                                      && x.StatusReport != StatusReport.Completed);

            var listDTO = new List<ReportDTO>();
            foreach (var item in reports)
                listDTO.Add(new ReportDTO
                {
                    CreateDate = $"{item.CreateDate.ToShortDateString()} às {item.CreateDate.ToShortTimeString()}",
                    EndDate = $"{item.EndDate.ToShortDateString()} às {item.EndDate.ToShortTimeString()}",
                    UserRequest = item.UserRequest,
                    RegistersProcess = item.RegistersProcess,
                    StatusReport = item.StatusReport.ToString(),
                    TypeReport = item.TypeReport.ToString(),
                    TotalRegisters = item.TotalRegisters,
                    PercentProcess = item.PercentProcess
                });

            var connections = _connections.GetConnections(user).ToList();
            foreach (var item in connections)
                Clients.Client(item).updateProgress(JsonConvert.SerializeObject(listDTO));
        }
    }
}
