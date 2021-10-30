using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class DealTypeRequestDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public bool DealTypeInsertion
        {
            get;
            set;
        }

    }
}
