using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class DealerStaffRequestDto
    {
        public Guid Id { get; set; }
        public Guid DealerId { get; set; }
        public Guid UserId { get; set; }

        public bool DealerStaffInsertion
        {
            get;
            set;
        }
        public string result { get; set; }

    }
}
