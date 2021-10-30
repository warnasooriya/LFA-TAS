using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ConfirmedBordxForGridRequestDto
    {
        public PaginationOptionsbordxSearchGrid PaginationOptionsbordxSearchGrid { get; set; }
        public BordxSearchGridSearchCriterias BordxSearchGridSearchCriterias { get; set; }
        public string action { get; set; }
    }

    public class BordxViewGridRequestDto
    {
        public PaginationOptionsbordxSearchGrid PaginationOptionsbordxSearchGrid { get; set; }
        public BordxViewGridSearchCriterias BordxViewGridSearchCriterias { get; set; }
    }
    public class PaginationOptionsbordxSearchGrid
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string sort { get; set; }
    }

    public class BordxSearchGridSearchCriterias
    {
        public Guid userId { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public Guid insureId { get; set; }
        public Guid reinsureId { get; set; }
        //public Guid countryId { get; set; }
        public Guid commodityTypeId { get; set; }
        public Guid productId { get; set; }
    }

    public class BordxViewGridSearchCriterias
    {
        public Guid userId { get; set; }
       // public int year { get; set; }
       // public int month { get; set; }
        public Guid bordxId { get; set; }
    }
}
