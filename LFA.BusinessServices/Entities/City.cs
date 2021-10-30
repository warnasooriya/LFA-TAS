using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class City
    {
        public virtual Guid Id { get; set; }
        public virtual string CityName { get; set; }
        public virtual Guid CountryId { get; set; }
        public virtual string ZipCode { get; set; }
    }
}
