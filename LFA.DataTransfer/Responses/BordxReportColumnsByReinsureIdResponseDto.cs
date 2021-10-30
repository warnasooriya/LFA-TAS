using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class BordxReportColumnsByReinsureIdResponseDto
    {

        public Guid ColumnId { get; set; }
        public string DisplayName { get; set; }
        public Guid ReinsureId { get; set; }
        public bool IsAllowed { get; set; }
        public Guid HeaderId { get; set; }
        public int Sequance { get; set; }

    }
}
