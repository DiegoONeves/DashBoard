using Microsoft.Owin.Hosting;
using System;
using System.ServiceProcess;

namespace SignalRSelfHost
{
    public partial class SignalRService : ServiceBase
    {
        public SignalRService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
           
        }

        protected override void OnStop()
        {

        }

        public void ExecutarInterativo()
        {
            OnStart(null);

            using (var dc = new DebugConsole())
            {
                string url = "http://localhost:8080";
                using (WebApp.Start(url))
                {
                    Console.WriteLine("Server running on {0}", url);
                    Console.ReadLine();
                }
            }

            OnStop();
        }
    }
}
