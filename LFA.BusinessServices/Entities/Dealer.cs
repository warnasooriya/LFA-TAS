using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class Dealer
    {
        public virtual Guid Id { get; set; }
        public virtual string DealerCode { get; set; }
        public virtual string DealerName { get; set; }
        public virtual string DealerAliase { get; set; }
        public virtual Guid CountryId { get; set; }
        public virtual Guid CurrencyId { get; set; }
        public virtual string Type { get; set; }
        public virtual Guid CommodityTypeId { get; set; }
        public virtual Guid InsurerId { get; set; }
        public virtual Guid CityId { get; set; }
        public virtual string Location { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual bool IsAutoApproval { get; set; }
        public virtual decimal ManHourRate { get; set; }
        public virtual Guid CurrencyPeriodId { get; set; }
        public virtual decimal ConversionRate { get; set; }

        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }

    }

    public class DealerMakes
    {
        public virtual Guid Id { get; set; }
        public virtual Guid DealerId { get; set; }
        public virtual Guid MakeId { get; set; }
    }

    public class DealerWithMakes:Dealer
    {      
        public virtual List<Guid> Makes { get; set; }    

    }
}
