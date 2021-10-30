using System;

namespace TAS.Services.Entities
{
    public class ClaimBatch
    {
        public virtual Guid Id { get; set; }
        public virtual Guid CountryId { get; set; }
        public virtual Guid DealerId { get; set; }
        public virtual Guid InsurerId { get; set; }
        public virtual Guid ReinsurerId { get; set; }
        public virtual string BatchNumber { get; set; }
        public virtual int Number { get; set; }
        public virtual DateTime EntryDate { get; set; }
        public virtual Guid EntryBy { get; set; }

    }
}
