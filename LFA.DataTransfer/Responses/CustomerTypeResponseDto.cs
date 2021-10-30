using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class CustomerTypeResponseDto
    {
        public int Id { get; set; }
        public string CustomerTypeName { get; set; }
        public string CustomerTypeDescription { get; set; }

    }
}
