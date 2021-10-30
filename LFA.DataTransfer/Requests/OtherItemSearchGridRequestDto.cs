using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
     public class OtherItemSearchGridRequestDto
    {
         public PaginationOptionsOtherItemSearchGrid paginationOptionsOtherSearchGrid { get; set; }
         public OtherItemSearchGridSearchCriterias OtherSearchGridSearchCriterias { get; set; }
    }


     public class PaginationOptionsOtherItemSearchGrid
     {
         public int pageNumber { get; set; }
         public int pageSize { get; set; }
         public string sort { get; set; }
     }

     public class OtherItemSearchGridSearchCriterias
     {
         public String serialNo { get; set; }
         public Guid make { get; set; }
         public Guid model { get; set; }

     }
}
