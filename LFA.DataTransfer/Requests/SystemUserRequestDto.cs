using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class SystemUserRequestDto
    {
        public string Id { get; set; }
        public int SequanceNumber { get; set; }
        public int RecordVersion { get; set; }
        public DateTime EntryDate { get; set; }
        public string EntryBy { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public Guid RoleId { get; set; }
        public Guid UserTypeId { get; set; }
        public Guid LoginMapId { get; set; }
        public Guid LanguageId { get; set; }

        public bool UserInsertion
        {
            get;
            set;

        }
    }
}
