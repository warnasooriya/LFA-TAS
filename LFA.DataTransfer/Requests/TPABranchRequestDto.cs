using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class TPABranchRequestDto
    {
        public virtual Guid Id { get; set; }
        public virtual string BranchName { get; set; }
        public virtual string BranchCode { get; set; }
        public virtual Guid ContryId { get; set; }
        public virtual Guid CityId { get; set; }
        public virtual string State { get; set; }
        public virtual Guid TimeZone { get; set; }
        public virtual string Address { get; set; }
        public virtual bool IsHeadOffice { get; set; }
        public virtual Guid TpaId { get; set; }
        
    }
}
