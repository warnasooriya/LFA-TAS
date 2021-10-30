using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class RoleMenuMappingResponseDto
    {
        public Guid Id { get; set; }
        public Guid MenuId { get; set; }
        public Guid RoleId { get; set; }
        public Guid LevelId { get; set; }
        public string Description { get; set; }
        
        public bool IsRoleMenuMappingExists { get; set; }
    }
}
