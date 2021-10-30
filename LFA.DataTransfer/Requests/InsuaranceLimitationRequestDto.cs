using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class InsuaranceLimitationRequestDto
    {
        public InsuaranceLimitationDto insuaranceLimitation { get; set; }
        public Guid commodityTypeId { get; set; }
        public bool isRsa { get; set; }
        public bool isOther { get; set; }
        public bool isAutomobile { get; set; }
        public bool isYellowGood { get; set; }
        public Guid loggedInUserId { get; set; }
    }

    public class InsuaranceLimitationDto
    {
        public decimal km { get; set; }
        public int month { get; set; }
        public decimal hours { get; set; }
        public bool upto { get; set; }
        public bool unlimitedcheck { get; set; }

    }
}
