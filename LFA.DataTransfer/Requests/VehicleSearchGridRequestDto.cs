using System;

namespace TAS.DataTransfer.Requests
{

    public class VehicleSearchGridRequestDto
    {
        public PaginationOptionsVehicleSearchGrid paginationOptionsVehicleSearchGrid { get; set; }
        public VehicalSearchGridSearchCriterias vehicalSearchGridSearchCriterias { get; set; }
        public Guid dealerId { get; set; }

    }

    public class PaginationOptionsVehicleSearchGrid
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string sort { get; set; }
    }

    public class VehicalSearchGridSearchCriterias
    {
        public String vinNo { get; set; }
        public String plateNo { get; set; }

        public Guid makeid { get; set; }

        public Guid modelid { get; set; }
    }
}
