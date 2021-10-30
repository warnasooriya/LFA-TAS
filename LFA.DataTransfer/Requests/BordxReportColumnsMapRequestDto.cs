using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class BordxReportColumnsMapRequestDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ColumnId { get; set; }
        public Guid ProductType { get; set; }
        public bool IsActive { get; set; }

        public bool BordxReportColumnsMapInsertion
        {
            get;
            set;
        }

    }
}
