using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class BrownAndWhiteDetailsRequestDto
    {
        public  Guid Id { get; set; }
        public  DateTime ItemPurchasedDate { get; set; }
        public  Guid MakeId { get; set; }
        public  Guid ModelId { get; set; }
        public  string SerialNo { get; set; }
        public  decimal ItemPrice { get; set; }
        public  Guid CategoryId { get; set; }
        public  string ModelYear { get; set; }
        public  string AddnSerialNo { get; set; }
        public  Guid ItemStatusId { get; set; }
        public  string InvoiceNo { get; set; }
        public  string ModelCode { get; set; }
        public  decimal DealerPrice { get; set; }
        public  DateTime EntryDateTime { get; set; }
        public  Guid EntryUser { get; set; }
        public Guid CommodityUsageTypeId { get; set; }
        public Guid CommodityTypeId { get; set; }
        public Guid CountryId { get; set; }
        public Guid DealerId { get; set; }
        public  Guid currencyPeriodId { get; set; }
        public  Guid DealerCurrencyId { get; set; }
        public  decimal ConversionRate { get; set; }
        public Guid Variant { get; set; }
        


        public bool BrownAndWhiteDetailsInsertion
        {
            get;
            set;
        }

    }
}
