using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class BordxDetailsRequestDto
    {
        public virtual Guid Id { get; set; }
        public virtual Guid PolicyId { get; set; }
        public virtual Guid BordxId { get; set; }

        public bool BordxDetailsInsertion
        {
            get;
            set;
        }

    }
}
