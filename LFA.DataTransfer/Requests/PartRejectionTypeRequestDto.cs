using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class PartRejectionTypeRequestDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }

        public bool PartRejectionTypeInsertion
        {
            get;
            set;
        }
    }
}
