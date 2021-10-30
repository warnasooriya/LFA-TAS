using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;

namespace TAS.DataTransfer.Responses
{
    public class AllMakeModelDetailsByExtensionIdResponseDto
    {
        public List<SelectedMakeList> selectedMakeList { get; set; }
        public List<SelectedModelsList> selectedModelsList { get; set; }
        public List<SelectedVariantList> selectedVariantList { get; set; }
        public List<SelectedClinderCount> selectedClinderCounts { get; set; }
        public List<SelectedEngineCapacity> selectedEngineCapacities { get; set; }
        public List<SelectedGrossWeight> selectedGrossWeights { get; set; }
        public bool isAllMakesSelected { get; set; }
        public bool isAllModelsSelected { get; set; }
        public bool isAllVariantsSelected { get; set; }
        public bool isAllCCsSelected { get; set; }
        public bool isAllECsSelected { get; set; }
        public bool isAllGVWsSelected { get; set; }

    }
}
