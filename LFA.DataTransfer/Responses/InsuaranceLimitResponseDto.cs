using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class InsuaranceLimitResponseDto
    {
        public Guid Id { get; set; }
        public string InsuaranceLimitationName { get; set; }
        public int Months { get; set; }




    }
}
