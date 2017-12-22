using OracleFirewall.Interfaces;

namespace OracleFirewall.DTOs {
    public class RecordDto : IDto {
        public string TableName { get; set; }
        public int RecordCount { get; set; }
        public int RecordsProcessed { get; set; }
        public string Message { get; set; }
    }
}