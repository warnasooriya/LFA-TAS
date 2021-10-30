using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class UserRoleMappingRequestDto
    {
        public Guid Id { get; set; }
        public Guid UserRoleId { get; set; }
        public Guid UserId { get; set; }

        public bool UserRoleMappingInsertion
        {
            get;
            set;
        }
    }
}
