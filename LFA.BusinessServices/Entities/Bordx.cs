using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class Bordx
    {
        public virtual Guid Id { get; set; }
        public virtual Guid CommodityTypeId { get; set; }
        //public virtual Guid CountryId { get; set; }
        public virtual int Year { get; set; }
        public virtual int Month { get; set; }
        public virtual int Number { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual bool IsConformed { get; set; }
        public virtual bool IsProcessed { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }
        public virtual int SequenceNo { get; set; }
        public virtual Guid Insurer { get; set; }
        public virtual Guid Reinsurer { get; set; }
        public virtual Guid ProductId { get; set; }

    }
}
