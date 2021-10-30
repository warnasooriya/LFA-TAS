using System;

namespace TAS.Services.Entities
{
    public class SystemUser
    {
        public virtual string Id { get; set; }
        public virtual int SequanceNumber { get; set; }
        public virtual int RecordVersion { get; set; }
        public virtual DateTime EntryDate { get; set; }
        public virtual string EntryBy { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual Guid RoleId { get; set; }
        public virtual Guid UserTypeId { get; set; }
        public virtual Guid LoginMapId { get; set; }
        public virtual Guid? LanguageId { get; set; }
    }
}