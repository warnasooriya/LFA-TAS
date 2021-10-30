using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class BordxReportColumnsResponseDto
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string HeaderName { get; set; }
        public string FieldName { get; set; }
        public bool IsActive { get; set; }

        public bool IsAllowed { get; set; }

        public bool IsBordxReportColumnsExists { get; set; }

    }
}
