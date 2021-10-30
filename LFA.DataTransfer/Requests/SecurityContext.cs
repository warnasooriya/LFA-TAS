using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Common
{
    public class SecurityContext
    {
        private string _token;
        private string _menuName;
        private string _userId;

        public string UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }
        
        public string MenuName 
        {
            get { return _menuName; }
            set { _menuName = value; }
        }
        

        public string Token
        {
            get { return _token; }
            set { _token = value; }
        }

       
        public SecurityContext()
        {
            
        }

        public void setToken(string t)
        {
            _token = t;
        }

        
    }
}
