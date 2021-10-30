using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class BundledProductResponseDto
    {
        
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid ParentProductId { get; set; }
        public bool IsCurrentProduct { get; set; }
        public ProductResponseDto parentProduct {get; set;}

    }
}
