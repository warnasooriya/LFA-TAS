using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class MenuResponseDto
    {
        public Guid Id { get; set; }
        public string MenuName { get; set; }
        public string MenuCode { get; set; }
        public string LinkURL { get; set; }
        public Guid ParentMenuId { get; set; }
        public string Icon { get; set; }
        public int OrderVal { get; set; }

        public bool IsMenuExists { get; set; }

    }
}
