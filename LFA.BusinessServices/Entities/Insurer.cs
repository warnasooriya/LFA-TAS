using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class Insurer
    {
        public virtual Guid Id { get; set; }
        public virtual string InsurerCode { get; set; }
        public virtual string BookletCode { get; set; }
        public virtual string InsurerShortName { get; set; }
        public virtual string InsurerFullName { get; set; }
        public virtual string Comments { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }
    }

    public class InsurerCountries
    {
        public virtual Guid Id { get; set; }
        public virtual Guid InsurerId { get; set; }
        public virtual Guid CountryId { get; set; }
    }

    public class InsurerProducts
    {
        public virtual Guid Id { get; set; }
        public virtual Guid InsurerId { get; set; }
        public virtual Guid ProductId { get; set; }
    }

    public class InsurerCommodityTypes
    {
        public virtual Guid Id { get; set; }
        public virtual Guid InsurerId { get; set; }
        public virtual Guid CommodityTypeId { get; set; }
    }

    public class InsurerInfo
    {
        public virtual Guid Id { get; set; }
        public virtual string InsurerCode { get; set; }
        public virtual string BookletCode { get; set; }
        public virtual string InsurerShortName { get; set; }
        public virtual string InsurerFullName { get; set; }
        public virtual string Comments { get; set; }
        public virtual List<Guid> Countries { get; set; }
        public virtual List<Guid> CommodityTypes { get; set; }
        public virtual List<Guid> Products { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }
    }
}
