using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class CustomerResponseDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int NationalityId { get; set; }
        public string Nationality { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Guid CountryId { get; set; }
        public string Country { get; set; }
        public string MobileNo { get; set; }
        public string OtherTelNo { get; set; }
        public char Gender { get; set; }
        public int CustomerTypeId { get; set; }
        public string CustomerType { get; set; }
        public string Customer { get; set; }
        public int UsageTypeId { get; set; }
        public string UsageType { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string IDNo { get; set; }
        public int IDTypeId { get; set; }
        public string IDType { get; set; }
        public Guid CityId { get; set; }
        public string City { get; set; }
        public DateTime? DLIssueDate { get; set; }
        public string Email { get; set; }
        public byte[] ProfilePicture { get; set; }
        public string ProfilePictureSrc { get; set; }
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
        public string Occupation { get; set; }
        public string Title { get; set; }
        public string MaritalStatus { get; set; }
        public string PostalCode { get; set; }
        public string status { get; set; }

        public bool IsCustomerExists
        {
            get;
            set;
        }
    }

    public class CustomerAddPolicyResponseDto
    {
        public string PolicyId { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int NationalityId { get; set; }
        public string Nationality { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Guid CountryId { get; set; }
        public string Country { get; set; }
        public string MobileNo { get; set; }
        public string OtherTelNo { get; set; }
        public char Gender { get; set; }
        public int CustomerTypeId { get; set; }
        public string CustomerType { get; set; }
        public string Customer { get; set; }
        public int UsageTypeId { get; set; }
        public string UsageType { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string IDNo { get; set; }
        public int IDTypeId { get; set; }
        public string IDType { get; set; }
        public Guid CityId { get; set; }
        public string City { get; set; }
        public DateTime? DLIssueDate { get; set; }
        public string Email { get; set; }
        public byte[] ProfilePicture { get; set; }
        public string ProfilePictureSrc { get; set; }
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
        public string Occupation { get; set; }
        public string Title { get; set; }
        public string MaritalStatus { get; set; }
        public string PostalCode { get; set; }
        public string status { get; set; }

        public bool IsCustomerExists
        {
            get;
            set;
        }
    }

    [Serializable]
    public class OccupationResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsOccupationExists
        {
            get;
            set;
        }
    }

    [Serializable]
    public class TitleResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsTitleExists
        {
            get;
            set;
        }
    }

    [Serializable]
    public class MarritalStatusResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsMarritalStatusExists
        {
            get;
            set;
        }
    }
}
