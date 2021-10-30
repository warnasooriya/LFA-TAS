using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ReinsurerStaffAddRequestDto
    {
        public ReinsurerStaffList data { get; set; }
    }
    public class ReinsurerStaffList
    {
        public List<ReinsurerStaff> ReinsurerStaff { get; set; }
        public Guid ReinusrerId { get; set; }
    }
    public class ReinsurerStaff 
    {
        public Guid ReinusrerId { get; set; }
        public Guid UserId { get; set; }

    }
}
