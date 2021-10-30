using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class ManufacturerWarrantyDetailsResponseDto
    {
        public  Guid Id { get; set; }
        public  Guid ManufacturerWarrantyId { get; set; }
        public  Guid ModelId { get; set; }
        public  Guid CountryId { get; set; }
        public bool ManufacturerWarrantyDetailsExists { get; set; }
    }
}
