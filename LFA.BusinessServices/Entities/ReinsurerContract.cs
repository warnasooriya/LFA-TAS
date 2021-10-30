using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ReinsurerContract
    {
        public virtual Guid Id { get; set; }
        public virtual Guid LinkContractId { get; set; }
        public virtual Guid ReinsurerId { get; set; }
        public virtual string UWYear { get; set; }
        public virtual Guid CountryId { get; set; }
        public virtual Guid CommodityTypeId { get; set; }
        public virtual Guid InsurerId { get; set; }
        public virtual DateTime FromDate { get; set; }
        public virtual DateTime ToDate { get; set; }
        public virtual string ContractNo { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }
        public virtual Guid BrokerId { get; set; }
    }
}
