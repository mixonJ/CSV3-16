using System.Collections.Generic;
using OracleFirewall.Interfaces;

namespace OracleFirewall.DTOs {
    public class ResultDto : IDto {
        public List<RecordDto> Records { get; set; }

        public string Message { get; set; }
    }
}