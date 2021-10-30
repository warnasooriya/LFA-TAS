using System;
using System.Collections.Generic;

namespace TAS.Services.Entities
{
    public class InternalUser
    {
        public virtual string Id { get; set; }
        public virtual int NationalityId { get; set; }
        public virtual Guid CountryId { get; set; }
        public virtual char Gender { get; set; }
        public virtual string MobileNo { get; set; }
        public virtual string OtherTelNo { get; set; }
        public virtual string InternalExtension { get; set; }
        public virtual DateTime EntryDate { get; set; }
        public virtual string EntryBy { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual DateTime? DateOfBirth { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string Address1 { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string Address3 { get; set; }
        public virtual string Address4 { get; set; }
        public virtual string IDNo { get; set; }
        public virtual int IDTypeId { get; set; }
        public virtual DateTime? DLIssueDate { get; set; }
        public virtual string ProfilePicture { get; set; }
        public virtual bool IsDealerAccount { get; set; }

    }
    public class InternalUserInfo:InternalUser
    {
        public virtual List<Guid> UserRoleMappings { get; set; }
    }
}