using Infra.DataAccess;
using System;

namespace Site.Models
{
    public class ReportViewModel
    {
        public int TotalRegisters { get; set; }
        public int RegistersProcess { get; set; }
        public string UserRequest { get; set; }
        public StatusReport StatusReport { get; set; }
        public TypeReport TypeReport { get; set; }
        public DateTime CreateDate { get; set; }
        public string PercentProcess { get; internal set; }
    }
}