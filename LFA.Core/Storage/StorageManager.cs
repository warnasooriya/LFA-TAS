using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace TAS.Core.Storage
{
    public static class StorageManager
    {

        private static bool IsWebApplication
        {
            get
            {
                return (HttpContext.Current != null);
            }
        }

        public static object GetData(string key)
        {
            object value = null;

            if (StorageManager.IsWebApplication)
            {
                value = HttpContext.Current.Items[key];
            }
            else
            {
                // TODO: implement using Thread classes
            }

            return value;
        }

        public static void SetData(string key, object value)
        {
            if (StorageManager.IsWebApplication)
            {
                HttpContext.Current.Items[key] = value;
            }
            else
            {
                // TODO: implement using Thread classes
            }
        }

    }
}
