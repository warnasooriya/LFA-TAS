using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class Make
    {
        public virtual Guid Id { get; set; }
        public virtual Guid CommodityTypeId { get; set; }
        public virtual  string MakeCode { get; set; }
        public virtual string MakeName { get; set; }
        public virtual Guid ManufacturerId { get; set; }
        public virtual bool WarantyGiven { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }
        public virtual bool IsMakeExists { get; set; }
    }
}
