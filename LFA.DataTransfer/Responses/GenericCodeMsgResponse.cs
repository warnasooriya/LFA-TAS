using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class GenericCodeMsgResponse
    {
        public string code { get; set; }
        public string msg { get; set; }

    }

    public class GenericCodeMsgObjResponse: GenericCodeMsgResponse
    {
     
        public object obj { get; set; }

    }
}
