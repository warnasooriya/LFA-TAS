using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class GetAllClaimBordxByYearandCountryRequestDto
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public Guid Country { get; set; }
        public int Year { get; set; }
    }
}
