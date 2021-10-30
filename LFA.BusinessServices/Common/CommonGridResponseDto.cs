using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Common
{
    public class CommonGridResponseDto
    {
        public long totalRecords { get; set; }
        public object data { get; set; }
    }
}
