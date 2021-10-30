using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class ExcitingCliamEndrosmentDataResponseDto
    {
        public string Status { get; set; }
        public List<ExcitingCliamEndrosment> ExcitingCliamEndrosmentData { get; set; }
    }

    public class ExcitingCliamEndrosment
    {
        public string ClaimSubmittedBy { get; set; }
        public string EntryDate { get; set; }
        public string Status { get; set; }

        public Guid CliamEndrosmentId { get; set; }
        public Guid CliamId { get; set; }


    }
}
