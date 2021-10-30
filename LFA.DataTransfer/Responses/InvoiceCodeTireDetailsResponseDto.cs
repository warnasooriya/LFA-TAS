using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class InvoiceCodeTireDetailsResponseDto
    {
        public  Guid Id { get; set; }
        public  Guid InvoiceCodeDetailId { get; set; }
        public  string Position { get; set; }
        public  string ArticleNumber { get; set; }
        public  string SerialNumber { get; set; }
        public string Width { get; set; }
        public string CrossSection { get; set; }
        public  int Diameter { get; set; }
        public  string LoadSpeed { get; set; }
        public  string DotNumber { get; set; }
        public  string Pattern { get; set; }
        public  decimal Price { get; set; }
    }
}
