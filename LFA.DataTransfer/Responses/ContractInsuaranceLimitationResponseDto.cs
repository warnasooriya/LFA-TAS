using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class ContractInsuaranceLimitationResponseDto
    {
        public  Guid Id { get; set; }
        public  Guid ContractId { get; set; }
        public  Guid BaseProductId { get; set; }
        public  DateTime EntryDateTime { get; set; }
        public  Guid InsuaranceLimitationId { get; set; }
    }
}
