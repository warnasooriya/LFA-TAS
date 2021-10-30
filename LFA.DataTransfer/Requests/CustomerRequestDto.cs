using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class CustomerRequestDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int NationalityId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Guid CountryId { get; set; }
        public char Gender { get; set; }
        public string MobileNo { get; set; }
        public string OtherTelNo { get; set; }
        public int CustomerTypeId { get; set; }
        public int UsageTypeId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string IDNo { get; set; }
        public int IDTypeId { get; set; }
        public Guid CityId { get; set; }
        public DateTime? DLIssueDate { get; set; }
        public string Email { get; set; }
        public string BusinessName { get; set; }
        public string BusinessAddress1 { get; set; }
        public string BusinessAddress2 { get; set; }
        public string BusinessAddress3 { get; set; }
        public string BusinessAddress4 { get; set; }
        public string BusinessTelNo { get; set; }
        public DateTime? EntryDateTime { get; set; }
        public string EntryUserId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public List<Guid> UserRoleMappings { get; set; }
        public Guid OccupationId { get; set; }
        public Guid TitleId { get; set; }
        public Guid MaritalStatusId { get; set; }
        public string PostalCode { get; set; }

        public bool CustomerInsertion
        {
            get;
            set;
        }
    }
    public class OccupationRequestDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool OccupationInsertion
        {
            get;
            set;
        }
    }
    public class TitleRequestDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool TitleInsertion
        {
            get;
            set;
        }
    }
    public class MarritalStatusRequestDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool MarritalStatusInsertion
        {
            get;
            set;
        }
    }
}
