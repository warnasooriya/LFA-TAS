using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{

    [Serializable]
    public class FaultCategoryResponseDto
    {
        public string FaultCategoryCode { get; set; }
        public string FaultCategoryName { get; set; }
        public Guid Id { get; set; }

        public bool IsActive { get; set; }
    }
}
