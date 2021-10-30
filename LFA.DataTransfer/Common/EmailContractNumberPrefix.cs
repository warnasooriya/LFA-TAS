using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Common
{
    public class EmailContractNumberPrefix
    {
        public  List<EmailPrefix> emailPrefixes { get; set; }
        public EmailContractNumberPrefix() {
            emailPrefixes = new List<EmailPrefix>();
            emailPrefixes.Add(new EmailPrefix { TpaName = "continental", Prefix = "CP Contract No : " , SerialNo =1 });
            emailPrefixes.Add(new EmailPrefix { TpaName = "gargash", Prefix = "Service Contract No : " , SerialNo = 2 });
            emailPrefixes.Add(new EmailPrefix { TpaName = "default", Prefix = "Contract No : " , SerialNo = 3});
        }
    }

    public class EmailPrefix{
        public string TpaName { get; set; }
        public string Prefix { get; set; }
        public int SerialNo { get; set; }
    }
}
