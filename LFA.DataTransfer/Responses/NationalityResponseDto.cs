using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class NationalityResponseDto
    {
        public int Id { get; set; }
        public string NationalityName { get; set; }

    }
}
