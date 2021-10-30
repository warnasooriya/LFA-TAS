using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class SystemUserResponseDto
    {
        public string Id { get; set; }
        public int SequanceNumber { get; set; }
        public int RecordVersion { get; set; }
        public DateTime EntryDate { get; set; }
        public string EntryBy { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public Guid RoleId { get; set; }
        public Guid UserTypeId { get; set; }
        public Guid LoginMapId { get; set; }
        public bool IsUserExists { get; set; }
        public Guid? LanguageId { get; set; }
    }
}
