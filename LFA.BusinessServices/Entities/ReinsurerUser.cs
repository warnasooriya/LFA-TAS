using System;
using System.Collections.Generic;

namespace TAS.Services.Entities
{
    public class ReinsurerUser
    {
        public virtual Guid Id { get; set; }
        public virtual Guid InternalUserId { get; set; }
        public virtual Guid ReinsurerId { get; set; }


    }
    //public class InternalUserInfo:InternalUser
    //{
    //    public virtual List<Guid> UserRoleMappings { get; set; }
    //}
}