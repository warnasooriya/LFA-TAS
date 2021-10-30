using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class PriviledgeLevelResponseDto
    {
        public Guid Id { get; set; }
        public string LevelName { get; set; }
        public string LevelDescription { get; set; }

        public bool IsPriviledgeLevelExists { get; set; }

    }
}
