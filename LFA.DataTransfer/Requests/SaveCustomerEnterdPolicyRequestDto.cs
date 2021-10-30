using System;

namespace TAS.DataTransfer.Requests
{
    public class SaveCustomerEnterdPolicyRequestDto
    {
        public Customer_maindata data { get; set; }
        public Guid tpaId { get; set; }
    }

    public class Customer_data
    {
        public Guid customerId { get; set; }
        public int customerTypeId { get; set; }
        public int usageTypeId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string gender { get; set; }
        public string idTypeId { get; set; }
        public string idNo { get; set; }
        public string idIssueDate { get; set; }
        public int nationalityId { get; set; }
        public Guid countryId { get; set; }
        public Guid cityId { get; set; }
        public string mobileNo { get; set; }
        public string otherTelNo { get; set; }
        public string email { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string address4 { get; set; }
        public string businessName { get; set; }
        public string businessTelNo { get; set; }
        public string businessAddress1 { get; set; }
        public string businessAddress2 { get; set; }
        public string businessAddress3 { get; set; }
        public string businessAddress4 { get; set; }
        public string password { get; set; }
        public string cpassword { get; set; }
        public string userName { get; set; }
    }

    public class Customer_maindata
    {
        public Customer_data customer { get; set; }
        public Guid tempInvId { get; set; }
    }
}
