using System;
using System.Collections.Generic;

namespace TAS.DataTransfer.Requests
{
    public class SaveCustomerPolicyInfoRequestDto
    {
        public TyreProduct product { get; set; }
        public Guid tpaId { get; set; }
        public List<AvailableTireList> availableTireList { get; set; }
        public Guid customerId { get; set; }
        public Guid tempInvId { get; set; }
    }

    public class TyreProduct
    {
        public Guid categoryId { get; set; }
        public string serialNumber { get; set; }
        public Guid makeId { get; set; }
        public Guid modelId { get; set; }
        public string invoiceCode { get; set; }
        public string plateNo { get; set; }
        public string commodityUsageType { get; set; }
        public string invoiceNo { get; set; }
        public decimal addMileage { get; set; }
        public Guid invoiceAttachmentId { get; set; }

        public Guid addMakeId { get; set; }
        public Guid addModelId { get; set; }
        public int addModelYear { get; set; }

    }

    public class AvailableTireList
    {
        public string position { get; set; }
        public decimal price { get; set; }
        public string dot { get; set; }
        public string article { get; set; }
        public Guid id { get; set; }
    }
}
