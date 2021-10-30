using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class YellowGoodSearchGridRequestDto
    {
        public PaginationOptionsYellowGoodsSearchGrid paginationOptionsYellowGoodsSearchGrid { get; set; }
        public YellowGoodSearchGridSearchCriterias YellowGoodSearchGridSearchCriterias { get; set; }
    }


    public class PaginationOptionsYellowGoodsSearchGrid
     {
         public int pageNumber { get; set; }
         public int pageSize { get; set; }
         public string sort { get; set; }
     }

    public class YellowGoodSearchGridSearchCriterias
     {
         public String serialNo { get; set; }
         public Guid make { get; set; }
         public Guid model { get; set; }

     }
}
