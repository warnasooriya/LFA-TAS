using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class FaultCategoryRequestDto
    {
        public string FaultCategoryCode { get; set; }
        public string FaultCategoryName { get; set; }
        public Guid Id { get; set; }

        public bool IsActive { get; set; }
        public bool FaultCategoryInsertion
        {
            get;
            set;
        }

    }
}
