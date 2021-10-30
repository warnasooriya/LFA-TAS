using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class CityResponseDto
    {
        public Guid Id { get; set; }
        public string CityName { get; set; }
        public Guid CountryId { get; set; }
        public string ZipCode { get; set; }
        public bool IsCityExists { get; set; }

    }
}
