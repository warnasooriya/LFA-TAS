using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class IdTypeResponseDto
    {
        public int Id { get; set; }
        public string IdTypeName { get; set; }
        public string IdTypeDescription { get; set; }

    }
}
