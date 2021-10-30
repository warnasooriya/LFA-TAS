using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class PolicyApprovalInizializeResponseDto
    {
        public object getDocumentTypesByPageName { get; set; }

        public object GetAllCommodityUsageTypes { get; set; }
        public object GetPremiumBasedOns { get; set; }
        public object GetCurrencies { get; set; }
        public object GetUsers { get; set; }
        public object GetAllDealers { get; set; }
        public object GetAllPaymentModes { get; set; }
        public object GetAllCountries { get; set; }
        public object GetAllNationalities { get; set; }
        public object GetAllCustomerTypes { get; set; }
        public object GetAllUsageTypes { get; set; }
        public object GetAllIdTypes { get; set; }
        public object GetAllCustomers { get; set; }
        public object GetAllVehicleDetails { get; set; }
        public object GetAllBrownAndWhiteDetails { get; set; }
        public object GetAllItemStatuss { get; set; }
        public object GetAllCylinderCounts { get; set; }
        public object GetAllDriveTypes { get; set; }
        public object GetAllEngineCapacities { get; set; }
        public object GetAllFuelTypes { get; set; }
        public object GetAllVehicleBodyTypes { get; set; }
        public object GetAllTransmissionTypes { get; set; }
        public object GetAllVehicleAspirationTypes { get; set; }
    }
}
