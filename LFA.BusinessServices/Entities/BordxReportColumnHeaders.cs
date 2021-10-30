using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class BordxReportColumnHeaders
    {
        public virtual Guid Id { get; set; }
        public virtual string HeaderName { get; set; }
        public virtual int Sequance { get; set; }
        public virtual bool GenarateSum { get; set; }

    }
}
