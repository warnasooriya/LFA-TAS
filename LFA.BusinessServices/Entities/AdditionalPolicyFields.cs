using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class AdditionalPolicyFields
    {
        public virtual Guid Id { get; set; }
        public virtual Guid CommodityTypeId { get; set; }
        public virtual Guid ProductId { get; set; }
        public virtual string ProductCode { get; set; }
        public virtual string DataFieldCode { get; set; }
        public virtual string DataFieldDisplayName { get; set; }
        public virtual string DataFieldType { get; set; }
        public virtual bool IsMandetory { get; set; }
        public virtual string InputType { get; set; }
        public virtual string MasterTable { get; set; }
        public virtual string DataField { get; set; }
        public virtual string ValueField { get; set; }
    }
}
