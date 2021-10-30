using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class UserRequestDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public int NationalityId { get; set; }
        public Guid CountryId { get; set; }
        public char Gender { get; set; }
        public string MobileNo { get; set; }
        public string OtherTelNo { get; set; }
        public string InternalExtension { get; set; }
        public DateTime EntryDate { get; set; }
        public string EntryBy { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string IDNo { get; set; }
        public int IDTypeId { get; set; }
        public DateTime? DLIssueDate { get; set; }
        public string ProfilePicture { get; set; }
        public List<Guid> UserRoleMappings { get; set; }
        public List<Guid> UserBranches { get; set; }

        public Guid? LanguageId { get; set; }

        public bool IsDealerAccount { get; set; }
        public bool UserInsertion
        {
            get;
            set;

        }
    }
}
