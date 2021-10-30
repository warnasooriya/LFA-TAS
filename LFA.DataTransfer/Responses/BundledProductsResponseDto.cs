using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class BundledProductsResponseDto
    {
        public List<BundledProductResponseDto> BundledProducts
        {
            get;
            set;
        }
    }
}
