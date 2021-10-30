using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class LoginResponseDto
    {
        public string JsonWebToken { get; set; }
        public bool IsValid { get; set; }
        public Guid LoggedInUserId { get; set; }
        public string UserType { get; set; }
        public string LanguageCode { get; set; }
    }
}
