using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class PriviledgeLevelRequestDto
    {
        public Guid Id { get; set; }
        public string LevelName { get; set; }
        public string LevelDescription { get; set; }

        public bool PriviledgeLevelInsertion
        {
            get;
            set;
        }

    }
}
