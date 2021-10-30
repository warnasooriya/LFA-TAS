using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class RegionRequestDto
    {
        public Guid Id { get; set; }
        public string RegionName { get; set; }
        public string RegionCode { get; set; }

        public bool RegionInsertion
        {
            get;
            set;
        }

    }
}
