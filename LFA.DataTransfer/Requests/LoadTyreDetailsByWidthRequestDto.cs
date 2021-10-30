using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
     public class LoadTyreDetailsByWidthRequestDto
    {        
        public String frontwidth { get; set; }
        public String frontcross { get; set; }
        public String frontdiameter { get; set; }
        public String frontloadSpeed { get; set; }
    }
}
