using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class PartSuggestion
    {
        public virtual Guid Id { get; set; }
        public virtual Guid PartId { get; set; }
        public virtual Guid SuggestedPartId { get; set; }
        public virtual int Quantity { get; set; }

    }
}
