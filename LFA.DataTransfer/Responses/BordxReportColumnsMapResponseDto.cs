using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class BordxReportColumnsMapResponseDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ColumnId { get; set; }
        public bool IsActive { get; set; }

        public bool IsBordxReportColumnsMapExists { get; set; }

    }
}
