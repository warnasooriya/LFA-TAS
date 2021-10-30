using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class AllArticleNoByTyreSizesResponseDto
    {
        public  Guid Id { get; set; }
        public  string ArticleNo { get; set; }
        public  Guid AvailableTireSizeId { get; set; }
    }
}
