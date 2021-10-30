using System;

namespace TAS.Services.Entities
{
    public class UserRole
    {
        public virtual Guid RoleId { get; set; }
        public virtual string RoleCode { get; set; }
        public virtual string RoleName { get; set; }
        public virtual bool IsGoodWillAuthorized { get; set; }
        public virtual bool IsClaimAuthorized { get; set; }


    }
}
