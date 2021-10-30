using TAS.DataTransfer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAS.Web.Common
{
    internal static class SecurityHelper
    {
        private static SecurityContext sc  = new SecurityContext();

        public static SecurityContext Context
        {
            get
            {
                return sc;
            }
            set
            {
                sc = value;
            }
        }


    }
}