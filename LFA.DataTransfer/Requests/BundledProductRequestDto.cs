using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class BundledProductRequestDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid ParentProductId { get; set; }
        public bool IsCurrentProduct { get; set; }

        public bool BundledProductInsertion
        {
            get;
            set;
        }
    }
}
