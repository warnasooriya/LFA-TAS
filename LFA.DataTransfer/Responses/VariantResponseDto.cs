using System;
using System.Collections.Generic;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class VariantResponseDto
    {
        public Guid Id { get; set; }
        public Guid ModelId { get; set; }
        public Guid CommodityTypeId { get; set; }
        public string VariantName { get; set; }
        public int? FromModelYear { get; set; }
        public int? ToModelYear { get; set; }
        public Guid EngineCapacityId { get; set; }
        public Guid CylinderCountId { get; set; }
        public string BodyCode { get; set; }
        public List<Guid> BodyTypes { get; set; }
        public List<Guid> Countrys { get; set; }
        public List<Guid> FuelTypes { get; set; }
        public List<Guid> Aspirations { get; set; }
        public List<Guid> Transmissions { get; set; }
        public List<Guid> DriveTypes { get; set; }
        public List<Guid> PremiumAddonType { get; set; }
        public bool IsActive { get; set; }
        public decimal GrossWeight { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }
        public bool IsSports { get; set; }
        public bool IsForuByFour { get; set; }
        public bool IsVariantExists { get; set; }

    }
}
