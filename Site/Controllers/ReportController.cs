using Infra.DataAccess;
using Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        MongoRepository repository = new MongoRepository();
        // GET: Report
        public ActionResult Index()
        {
            var docs = repository.Find(x => true).OrderByDescending(x => x.CreateDate);

            var vm = new List<ReportViewModel>();
            foreach (var item in docs)
            {
                vm.Add(new ReportViewModel
                {
                    UserRequest = item.UserRequest,
                    CreateDate = item.CreateDate,
                    RegistersProcess = item.RegistersProcess,
                    StatusReport = item.StatusReport,
                    TypeReport = item.TypeReport,
                    TotalRegisters = item.TotalRegisters,
                    PercentProcess = $"{item.PercentProcess}%"
                });
            }
            return View(vm);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReportViewModel model)
        {
            if (ModelState.IsValid)
            {
                var reportToProcess = new Report
                {
                    UserRequest = User.Identity.Name,
                    CreateDate = DateTime.Now,
                    TotalRegisters = model.TotalRegisters,
                    TypeReport = model.TypeReport,
                    StatusReport = StatusReport.NotStarted
                };
                repository.Add(reportToProcess);

                return RedirectToAction("Index");
            }


            return View(model);
        }
    }
}