using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class CountryResponseDto
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string PhoneCode { get; set; }
        public Guid CurrencyId { get; set; }
        public bool IsActive { get; set; }
        public Guid Id { get; set; }
        public List<Guid> Makes { get; set; }
        public List<Guid> Modeles { get; set; }

        public bool IsCountryExists { get; set; }
    }
}
