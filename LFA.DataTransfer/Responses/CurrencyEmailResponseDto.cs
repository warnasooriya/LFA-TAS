using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class CurrencyEmailResponseDto
    {
        public Guid Id { get; set; }
        public string TPAEmail { get; set; }
        public string AdminEmail { get; set; }
        public int FirstEmailDuration { get; set; }
        public string FirstDurationType { get; set; }
        public string FirstMailSubject { get; set; }
        public string FirstMailBody { get; set; }
        public int SecoundEmailDuration { get; set; }
        public string SecoundDurationType { get; set; }
        public string SecoundMailSubject { get; set; }
        public string SecoundMailBody { get; set; }
        public int LastEmailDuration { get; set; }
        public string LastDurationType { get; set; }
        public string LastMailSubject { get; set; }
        public string LastMailBody { get; set; }

        public bool IsCurrencyEmailExists { get; set; }

    }
}
