using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class GetAllReinsurerBordxByYearandReinsureIdRequestDto
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public Guid ReinsureId { get; set; }
        public int Year { get; set; }
    }
}
