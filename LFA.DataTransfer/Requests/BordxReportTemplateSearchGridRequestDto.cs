using System;

namespace TAS.DataTransfer.Requests
{
    public class BordxReportTemplateSearchGridRequestDto
    {

        public PaginationOptionsSearchGrid paginationOptionsSearchGrid { get; set; }
        public BordxReportTemplateSearchGridSearchCriterias bordxReportTemplateSearchGridSearchCriterias { get; set; }
        public string type { get; set; }
    }

    public class BordxReportTemplateSearchGridSearchCriterias
    {
        public string Name { get; set; }
        public string TemplateName { get; set; }
        public Guid ProductType { get; set; }
    }
}
