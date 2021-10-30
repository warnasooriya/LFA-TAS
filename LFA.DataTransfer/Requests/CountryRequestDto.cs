using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class CountryRequestDto
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string PhoneCode { get; set; }
        public Guid CurrencyId { get; set; }
        public bool IsActive { get; set; }
        public Guid Id { get; set; }
        public List<Guid> Makes { get; set; }
        public List<Guid> Modeles { get; set; }

        public bool CountryInsertion
        {
            get;
            set;
        }

    }
}
