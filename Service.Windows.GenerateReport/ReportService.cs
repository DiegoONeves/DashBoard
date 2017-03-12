using Infra.DataAccess;
using SignalRHandler;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;

namespace Service.Windows.GenerateReport
{
    public partial class ReportService : ServiceBase
    {
        private System.Timers.Timer _timer;
        MongoRepository repo = new MongoRepository();
        ConcurrentDictionary<string, bool> itensInProcess = new ConcurrentDictionary<string, bool>();

        public ReportService() => InitializeComponent();

        protected override void OnStart(string[] args)
        {
            _timer = new System.Timers.Timer(1000) { AutoReset = true };
            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();
        }

        protected override void OnStop()
        {
            _timer.Stop();
            _timer.Dispose();
        }

        void VerifyIfExistsReportToProcessOrStop()
        {
            var reports = repo.Find(x => x.StatusReport == StatusReport.NotStarted
                                     || x.StatusReport == StatusReport.Started
                                     || x.StatusReport == StatusReport.Stoped)
                              .OrderBy(x => x.CreateDate);

            foreach (var item in reports.Where(x => x.StatusReport == StatusReport.Stoped))
            {
                bool v;
                itensInProcess.TryRemove(item.Id.ToString(), out v);
            }

            foreach (var item in reports.Where(x => x.StatusReport == StatusReport.NotStarted || x.StatusReport == StatusReport.Started))
            {
                if (!itensInProcess.ContainsKey(item.Id.ToString()))
                {
                    if (itensInProcess.Count < 2)
                    {
                        itensInProcess.TryAdd(item.Id.ToString(), true);
                        Task.Factory.StartNew(() =>
                        {
                            ProcessReport(item);
                            bool a;
                            itensInProcess.TryRemove(item.Id.ToString(), out a);
                        }, TaskCreationOptions.LongRunning);

                    }
                }
            }

        }

        void ProcessReport(Report report)
        {
            report.StatusReport = StatusReport.Started;
            repo.Update(report, report.Id.ToString());
            int registers = report.TotalRegisters - report.RegistersProcess;
            for (int i = 0; i < registers; i++)
            {
                if (repo.Find(x => x.Id == report.Id).FirstOrDefault().StatusReport == StatusReport.StopRequested)
                    break;
                report.RegistersProcess++;
                report.PercentProcess = CalculatePercent(report.TotalRegisters, report.RegistersProcess);
                repo.Update(report, report.Id.ToString());

                signalRClient.Invoke("UpdateProgress", report.UserRequest);
            }

            report.StatusReport = StatusReport.Completed;
            report.EndDate = DateTime.Now;
            repo.Update(report, report.Id.ToString());
            signalRClient.Invoke("UpdateProgress", report.UserRequest);

        }

        private int CalculatePercent(int total, int parcial)
        {
            float totalFloat = total;
            float parcialFloat = parcial;

            float a = totalFloat / 100;
            var b = parcialFloat / a;
            decimal resultDecimal = (decimal)b;
            return (int)Math.Round(resultDecimal, MidpointRounding.ToEven);
        }
        SignalRClient signalRClient = new SignalRClient(@"http://localhost:8080/", "ReportHub", null);
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                VerifyIfExistsReportToProcessOrStop();
            }
            catch (Exception)
            {

            }

        }

        public void ExecutarInterativo()
        {
            OnStart(null);

            using (var dc = new DebugConsole())
            {
                Console.WriteLine("Serviço em execução. Pressione ENTER para terminar.");
                Console.ReadLine();
            }

            OnStop();
        }
    }
}
