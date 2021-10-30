using TAS.DataTransfer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAS.Web.Common
{
    internal static class LoggingHelper
    {
        public static LoggingContext Context
        {
            get
            {
                return new LoggingContext();
            }
        }
    }
}