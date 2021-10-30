using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class RoleMenuLevelMappingResponseDto
    {
        public Guid MenuId { get; set; }
        public Guid RoleId { get; set; }
        public string RoleCode { get; set; }
        public string Menu { get; set; }

        public bool Read { get; set; }
        public bool Create { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
    }
}
