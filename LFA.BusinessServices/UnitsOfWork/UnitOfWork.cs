using TAS.DataTransfer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer;

namespace TAS.Services.UnitsOfWork
{
    internal abstract class UnitOfWork
    {
        public string dbConnectionString
        {
            get;
            set;
        }


        public SecurityContext SecurityContext
        {
            get;
            set;
        }

        public AuditContext AuditContext
        {
            get;
            set;
        }

        public LoggingContext LoggingContext
        {
            get;
            set;
        }
        public abstract bool PreExecute();
        public abstract void Execute();

    }
}
