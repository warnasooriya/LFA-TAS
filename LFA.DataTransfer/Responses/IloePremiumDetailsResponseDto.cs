namespace TAS.DataTransfer.Responses
{
    public class IloePremiumDetailsResponseDto
    {
        public int LoanPeriod { get; set; }
        public double AnnualInterestRate { get; set; }
        public double NRPRate { get; set; }
        public double GrossRate { get; set; }
        public double MinimumPayment { get; set; }
        public double MaximumPayment { get; set; }
        public int IncubationPeriod { get; set; }

    }
}
