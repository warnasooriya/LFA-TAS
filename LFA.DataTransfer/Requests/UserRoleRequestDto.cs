using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class UserRoleRequestDto
    {
        public Guid RoleId { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public bool IsGoodWillAuthorized { get; set; }
        public bool IsClaimAuthorized { get; set; }


        public bool UserRoleInsertion
        {
            get;
            set;
        }
    }
}
