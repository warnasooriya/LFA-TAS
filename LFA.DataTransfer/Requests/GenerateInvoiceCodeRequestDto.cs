using Newtonsoft.Json;
using System;

namespace TAS.DataTransfer.Requests
{
    public class GenerateInvoiceCodeRequestDto
    {
        public DealerInvoiceDetails dealerInvoiceDetails { get; set; }
        public DealerInvoiceTireDetails dealerInvoiceTireDetails { get; set; }
        public Guid loggedInUserId { get; set; }
    }

    public class DealerInvoiceDetails
    {
        public Guid dealerId { get; set; }
        public Guid dealerBranchId { get; set; }
        public Guid countryId { get; set; }
        public Guid cityId { get; set; }
        public int quantity { get; set; }
        public string plateNumber { get; set; }
    }

    public class Front
    {
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string width { get; set; }
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string cross { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int diameter { get; set; }

        public string loadSpeed { get; set; }
        public string serialLeft { get; set; }
        public string serialRight { get; set; }
        public string pattern { get; set; }
        public string dotLeft { get; set; }
        public bool leftcheckbox { get; set; }
        public bool rightcheckbox { get; set; }
        public decimal price { get; set; }
    }

    public class Back
    {
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string width { get; set; }
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string cross { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int diameter { get; set; }

        public string loadSpeed { get; set; }
        public string serialLeft { get; set; }
        public string serialRight { get; set; }
        public string pattern { get; set; }
        public string dotLeft { get; set; }
        public bool leftcheckbox { get; set; }
        public bool rightcheckbox { get; set; }

        public decimal price { get; set; }
    }

    public class SpareWeel
    {
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string width { get; set; }
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string cross { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int diameter { get; set; }

        public string loadSpeed { get; set; }
        public string serialLeft { get; set; }
        public string serialRight { get; set; }
        public string pattern { get; set; }
        public decimal price { get; set; }
    }

    public class DealerInvoiceTireDetails
    {
        public Front front { get; set; }
        public Back back { get; set; }
    }

}
