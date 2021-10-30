using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class SaveTyrePolicySalesRequestDto
    {
        public DealerInvoiceDetails dealerInvoiceDetails { get; set; }
        public dealerInvoiceTireDetails dealerInvoiceTireSaveDetails { get; set; }
        public  Guid LoggedInUserId { get; set; }
        public customerDetails customerDetails { get; set; }
        public Guid customerId { get; set; }
        public Guid tempInvId { get; set; }
        public bool alltyres { get; set; }
        public List<TyreInvoceDetails> tyreDetails { get; set; }
        public List<ContractDetails> contractDetails { get; set; }
    }

    public class TyreDetails
    {
        public List<TyreInvoceDetails> tyreInvoceDetails { get; set; }
    }

    public class TyreInvoceDetails
    {
        public int TireQuantity { get; set; }
        public string Position { get; set; }
        public decimal price { get; set; }
        public List<Tyres> tyres { get; set; }
    }

    public class ContractDetails {
        public Guid ContractId { get; set; }
        public Guid ContractExtensionPremiumId { get; set; }
        public Guid ContractExtensionsId { get; set; }
        public string Position { get; set; }
        public DateTime purchaseDate { get; set; }
    }

    public class Tyres
    {
        public string position { get; set; }
        public string serialNo { get; set; }
        public string dotNumber { get; set; }
        public string width { get; set; }
        public string cross { get; set; }
        public int diameter { get; set; }
        public decimal price { get; set; }
        public string loadSpeed { get; set; }


    }

    public class dealerInvoiceTireDetails
    {
        public Front front { get; set; }
        public Back back { get; set; }
        public SpareWeel down { get; set; }
    }

    public class customerDetails
    {
        public int customerTypeId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string businessName { get; set; }
        public string businessTelNo { get; set; }
        public int usageTypeId { get; set; }
        public string mobileNo { get; set; }
        public string email { get; set; }
        public string invoiceNo { get; set; }
        public string invoiceAttachmentId { get; set; }
        public string commodityUsageType { get; set; }
        public string plateNumber { get; set; }
        public Guid PlateRelatedCityId { get; set; }
        public Guid addMakeId { get; set; }
        public Guid addModelId { get; set; }
        public int addModelYear { get; set; }
        public decimal addMileage { get; set; }
        public Guid tpaId { get; set; }

    }
}
