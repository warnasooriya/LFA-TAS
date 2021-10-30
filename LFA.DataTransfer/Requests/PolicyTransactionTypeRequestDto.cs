using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class PolicyTransactionTypeRequestDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public bool PolicyTransactionTypeInsertion
        {
            get;
            set;
        }
    }
}
