using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class BordxReportColumnsRequestDto
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string HeaderName { get; set; }
        public string FieldName { get; set; }
        public bool IsActive { get; set; }

        public bool BordxReportColumnsInsertion
        {
            get;
            set;
        }

    }
}
