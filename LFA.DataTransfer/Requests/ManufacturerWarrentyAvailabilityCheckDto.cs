using System;

namespace TAS.DataTransfer.Requests
{
    public class ManufacturerWarrentyAvailabilityCheckDto
    {
        public Guid makeId { get; set; }
        public Guid modelId { get; set; }
        public Guid dealerId { get; set; }
        public Guid commodityTypeId { get; set; }
        public DateTime mwStartDate { get; set; }
        public DateTime policySoldDate { get; set; }
        public decimal usage { get; set; }

    }
}
