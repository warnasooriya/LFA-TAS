using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
  public  class DealerLocationByUserRequestDto
    {
        public Guid UserId { get; set; }
        public Guid TpaId { get; set; }
    }
}
