using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ManufacturerWarrentySearchRequestDto
    {
        public PaginationOptionsManufacturerWarrentySearchGrid paginationOptionsManufacturerWarrentySearchGrid { get; set; }
        public ManufacturerWarrentySearchGridSearchCriterias manufacturerWarrentySearchGridSearchCriterias { get; set; }
        public Guid CommodityTypeId { get; set; }
    }

    public class PaginationOptionsManufacturerWarrentySearchGrid
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public object sort { get; set; }
    }

    public class ManufacturerWarrentySearchGridSearchCriterias
    {
        public Guid Id { get; set; }
        public Guid MakeId { get; set; }
        public List<Guid> Countrys { get; set; }
        public List<Guid> Models { get; set; }
        public string WarrantyName { get; set; }
        public string WarrantyMonths { get; set; }
        public string WarrantyKm { get; set; }
        public string ApplicableFrom { get; set; }
       


    }

}
