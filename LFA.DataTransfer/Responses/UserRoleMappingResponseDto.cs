using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class UserRoleMappingResponseDto
    {
        public Guid Id { get; set; }
        public Guid UserRoleId { get; set; }
        public Guid RoleId { get; set; }
        public bool IsUserRoleMappingExists { get; set; }
    }
}
