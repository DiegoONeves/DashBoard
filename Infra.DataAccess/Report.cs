using MongoDB.Bson;
using System;

namespace Infra.DataAccess
{
    public class Report
    {
        public ObjectId Id { get; set; }
        public int TotalRegisters { get; set; }
        public int PercentProcess { get; set; }
        public int RegistersProcess { get; set; }
        public string UserRequest { get; set; }
        public StatusReport StatusReport { get; set; }
        public TypeReport TypeReport { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
