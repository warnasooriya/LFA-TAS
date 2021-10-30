using System;
using System.Collections.Generic;

namespace TAS.DataTransfer.Requests
{
    public class PushNotificationsRequestDto
    {
        public List<UserDetail> userDetails { get; set; }
        public string message { get; set; }
        public string link { get; set; }
        public string profilePic { get; set; }
        public string messageFrom { get; set; }
        public DateTime generatedTime { get; set; }
    }

    public class UserDetail
    {
        public Guid userId { get; set; }
        public Guid tpaId { get; set; }

    }
}
