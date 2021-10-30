
namespace TAS.DataTransfer.Responses
{
    public class ValidateContractWithTaxResponseDto
    {
        public bool Status { get; set; }
        public CountryTaxesResponseDto CountryTax { get; set; }
    }
}
