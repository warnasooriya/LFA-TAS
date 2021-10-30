using TAS.DataTransfer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAS.Web.Common
{
    internal static class AuditHelper
    {

        public static AuditContext Context
        {
            get
            {
                return new AuditContext();
            }
        }

    }
}