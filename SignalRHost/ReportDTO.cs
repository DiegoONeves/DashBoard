namespace SignalRSelfHost
{
    public class ReportDTO
    {
        public string Id { get; set; }
        public int TotalRegisters { get; set; }
        public int PercentProcess { get; set; }
        public int RegistersProcess { get; set; }
        public string UserRequest { get; set; }
        public string StatusReport { get; set; }
        public string TypeReport { get; set; }
        public string CreateDate { get; set; }
        public string EndDate { get; set; }

    }
}
