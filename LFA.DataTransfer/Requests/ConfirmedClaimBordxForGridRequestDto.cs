using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ConfirmedClaimBordxForGridRequestDto
    {
        public PaginationOptionsClaimbordxSearchGrid PaginationOptionsClaimbordxSearchGrid { get; set; }
        public ClaimBordxSearchGridSearchCriterias ClaimBordxSearchGridSearchCriterias { get; set; }
        public string action { get; set; }
    }

    public class ClaimBordxViewGridRequestDto
    {
        public PaginationOptionsClaimbordxSearchGrid PaginationOptionsClaimbordxSearchGrid { get; set; }
        public ClaimBordxViewGridSearchCriterias ClaimBordxViewGridSearchCriterias { get; set; }
    }
    public class PaginationOptionsClaimbordxSearchGrid
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string sort { get; set; }
    }

    public class ClaimBordxSearchGridSearchCriterias
    {
        public Guid userId { get; set; }
        public int year { get; set; }
        public int month { get; set; }        
        public Guid Insurer { get; set; }
        public Guid Reinsurer { get; set; }
    }

    public class ClaimBordxViewGridSearchCriterias
    {
        public Guid userId { get; set; }
        // public int year { get; set; }
        // public int month { get; set; }
        public Guid bordxId { get; set; }
    }
}
