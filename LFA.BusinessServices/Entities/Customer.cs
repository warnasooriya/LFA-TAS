using System;
using System.Collections.Generic;

namespace TAS.Services.Entities
{
    public class Customer
    {
        public Customer() { }

        public virtual Guid Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual int NationalityId { get; set; }
        public virtual DateTime? DateOfBirth { get; set; }
        public virtual Guid CountryId { get; set; }
        public virtual Guid OccupationId { get; set; }
        public virtual Guid TitleId { get; set; }
        public virtual Guid MaritalStatusId { get; set; }
        public virtual char Gender { get; set; }
        public virtual string MobileNo { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual string OtherTelNo { get; set; }
        public virtual int CustomerTypeId { get; set; }
        public virtual int UsageTypeId { get; set; }
        public virtual string Address1 { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string Address3 { get; set; }
        public virtual string Address4 { get; set; }
        public virtual string IDNo { get; set; }
        public virtual int IDTypeId { get; set; }
        public virtual Guid CityId { get; set; }
        public virtual DateTime? DLIssueDate { get; set; }
        public virtual string Email { get; set; }
        public virtual string BusinessName { get; set; }
        public virtual string BusinessAddress1 { get; set; }
        public virtual string BusinessAddress2 { get; set; }
        public virtual string BusinessAddress3 { get; set; }
        public virtual string BusinessAddress4 { get; set; }
        public virtual string BusinessTelNo { get; set; }
        public virtual byte[] ProfilePicture { get; set; }
        public virtual DateTime? EntryDateTime { get; set; }
        public virtual string EntryUserId { get; set; }
        public virtual DateTime? LastModifiedDateTime { get; set; }

    }
    public class CustoemrInfo:Customer
    {
        public virtual List<Guid> UserRoleMappings { get; set; }
    }

    public class Occupation
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
    }
    public class Title
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
    }
    public class MaritalStatus
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
    }

}