using System;

namespace TAS.Services.Entities
{
    public class TASSystemUser
    {
        public TASSystemUser() { }
        public virtual Guid Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual Guid RoleId { get; set; }
    }
}