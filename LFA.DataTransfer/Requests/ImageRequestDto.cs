using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace TAS.DataTransfer.Requests
{
    public class ImageRequestDto
    {
        public Guid Id { get; set; }
        public string ImageName { get; set; }
        public byte[] ImageByte { get; set; }
        public string Description { get; set; }
        public bool ImageStatus { get; set; }
        public DateTime DateUploaded { get; set; }
    }
}
