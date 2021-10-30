using System;

namespace TAS.DataTransfer.Responses
{
    public class AddBordxSummeryRequestDto
    {

        public Int64 SNo { get; set; }
        public DateTime BDXExtractDate { get; set; }
        public string UnderWriterYear { get; set; }
        public string ReinsurerName { get; set; }
        public string PolicyNo { get; set; }
        public string CedentName { get; set; }
        public string InsuredDetailsCountry { get; set; }
        public DateTime SystemPolicyTransactionDate { get; set; }
        public int SystemPolicyTransactionID { get; set; }
        public string BDXMTH { get; set; }
        public string BordxMonth { get; set; }
        public int BordxYear { get; set; }
        public string DealName { get; set; }
        public string DealType { get; set; }
        public string Status { get; set; }
        public string CoverType { get; set; }
        public string WarrantyType { get; set; }
        public string AmtUsedAtPolicySale { get; set; }
        public string DealerName { get; set; }
        public string Location { get; set; }
        public string Broker { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string CoBuyer { get; set; }
        public string Address { get; set; }
        public string POBox { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string MobileNumber { get; set; }

        public string BusinessName { get; set; }
        public string BusinessTel { get; set; }
        public string BusinessAddress { get; set; }
        public string BusinessCountry { get; set; }
        public string BusinessCity { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonTel { get; set; }



        public string VINNo { get; set; }
        public string RegNo { get; set; }
        public string Category { get; set; }
        public string Manufacture { get; set; }
        public string Model { get; set; }
        public string Variant { get; set; }

        public string CylinderCount { get; set; }
       
        public string FourByFour { get; set; }
        public string EngineCapacity { get; set; }
        public string Gvw { get; set; }
        public string GVWCategory { get; set; }
        public string ModelYear { get; set; }
       
       
        public string BookletNo { get; set; }
        public string SystemGeneratedNum { get; set; }

        public DateTime PolicySoldDate { get; set; }
        public DateTime VehiclePurcheseDate { get; set; }
        public DateTime VehicleRegistrationDate { get; set; }

        public DateTime ManfWarrantyStartDate { get; set; }
        public DateTime ManfWarrantyTerminationDate { get; set; }

        public string ManfCoverInMonths { get; set; }
        public string MileageLimitationInKMs { get; set; }
        public DateTime DateOfInsuranceRiskStart { get; set; }
        public DateTime DateOfInsuranceRiskTermination { get; set; }
        public string ExtensionPeriodInMonths { get; set; }
        public string ExtensionPeriodInKms { get; set; }
        public string CutOffKm { get; set; }
        public Decimal SumInsured { get; set; }

        public Decimal GrossPremium { get; set; }
        public Decimal GrossPremiumExTax { get; set; }

        
        public Decimal SalesCommission { get; set; }
        public Decimal GrossPremiumLessSalesCommission { get; set; }
        
        public Decimal MarketingFee { get; set; }
        public Decimal InsurerFee { get; set; }
        public Decimal AdminFee { get; set; }
        public Decimal InternalGoodWill { get; set; }
        public Decimal DealerCommission { get; set; }
        public Decimal DocumentFee { get; set; }
        public Decimal ClientBrokerage { get; set; }
        public Decimal InsurerNRPRetention { get; set; }
        public Decimal ManufactureCommission { get; set; }




        public Decimal GrossPremiumLessCommission { get; set; }
        public Decimal NRPIncludingBrokerage { get; set; }
        public Decimal Brokerage { get; set; }
        public Decimal NetAbsoluteRiskPremium { get; set; }
        
       
        

        public Decimal NRPIncludingBrokerageUS { get; set; }
        public Decimal BrokerageUS { get; set; }
        public Decimal NetAbsoluteRiskPremiumUS { get; set; }
        public Decimal ConversionRateUS { get; set; }

        public string StartDateRSA { get; set; }
        public string TerminationDateRSA { get; set; }
        public string PeriodInMonthsRSA { get; set; }
        public string CardNumberRSA { get; set; }
        public string GrossPremiumRSA { get; set; }
        public string NRPRSA { get; set; }
        public string InsurancePolicyNoOther { get; set; }
        public string SalesmanOther { get; set; }
        public string CommentOther { get; set; }



        //references
        public Guid BaseCountryId { get; set; }
        public string BaseCountry { get; set; }
        public string BaseCurrencyName { get; set; }
        public Guid BaseCurrencyId { get; set; }
        public Guid BaseCurrencyPeriodId { get; set; }
        public decimal CurrencyConversionRate { get; set; }
        public string TransactionTypeCode { get; set; }
        public Guid TransactionTypeId { get; set; }
        public Guid ContractId { get; set; }
        public Decimal GrossPremiumBeforeTax { get; set; }
        public Decimal NRP { get; set; }
        public Guid PolicyId { get; set; }
        public int autoId { get; set; }
        public string InvoiceCode { get; set; }												
        public string InvoiceNumber	{ get; set; }
		public string PlateNumber	{ get; set; }
		public string MakeName	{ get; set; }	
		public string ModelName	{ get; set; }
		public int AModelYear	{ get; set; }							  
		public Decimal AdditionalMileage	{ get; set; }																			
		public int NoofTires	{ get; set; }					       
        public string ArticleNumber { get; set; }

      

        // new 


        public string ContractNo { get; set; }
        public Decimal Deposit { get; set; }
        public Decimal FinanceAmount { get; set; }
        public int LonePeriod { get; set; }
        public int PeriodOfCover { get; set; }
        public Decimal MonthlyEMI { get; set; }
        public Decimal AnnualInterestRate { get; set; }

        //public string SerialNumber { get; set; }       




        //public int SNo { get; set; }
        //public string ContractYear { get; set; }
        //public string Reinsurer { get; set; }
        //public string Insurer { get; set; }
        //public string CountryOrignCode { get; set; }
        //public string ReportingMonth { get; set; }
        //public string Program { get; set; }
        //public string RIPremiumType { get; set; }
        //public string Status { get; set; }
        //public string CoverType { get; set; }
        //public string ItemStatus { get; set; }
        //public string Mileage { get; set; }
        //public string Seller { get; set; }
        //public string PointOfSale { get; set; }
        //public string FirstName { get; set; }
        //public string Surname { get; set; }
        //public string Address { get; set; }
        //public string City { get; set; }
        //public string Tel { get; set; }
        //public string VINNo { get; set; }
        //public string Registration { get; set; }
        //public string Category { get; set; }
        //public string VehicleManufacturer { get; set; }
        //public string MakeNModel { get; set; }
        //public string CylinderCount { get; set; }
        //public string EngineCapacity { get; set; }
        //public string ModelYear { get; set; }
        //public string SystemGeneratedIdentification { get; set; }
        //public string DatePurchaseRegistration { get; set; }
        //public string ManufacturersExpirey { get; set; }
        //public string ManufacturerCoverInMonth { get; set; }
        //public string LimitationsHoursMileagePeriod { get; set; }
        //public string RiskStartDate { get; set; }
        //public string RiskTerminationDate { get; set; }
        //public string CoverPeriodMonths { get; set; }
        //public string ExtensonPeriodHoursMileage { get; set; }
        //public string GrossPremium { get; set; }
        //public string SalesmanCommission { get; set; }
        //public string GrossPremiumLessSalesCommission { get; set; }
        //public string InsurerFee { get; set; }
        //public string Retention { get; set; }
        //public string ManufactureCommission { get; set; }
        //public string IPT { get; set; }
        //public string LocalTax { get; set; }
        //public string VAT { get; set; }
        //public string DocumentFee { get; set; }
        //public string SalesTax { get; set; }
        //public string Total { get; set; }
        //public string ClientBrokerage { get; set; }
        //public string GrossPremiumLessCommission { get; set; }
        //public string NRPIncludingBrokerage { get; set; }
        //public string Brokerage { get; set; }
        //public string NetAbsoluteRiskPremium { get; set; }
        //public string AdministrationFee { get; set; }
        //public string SumInsuredInLocalCurrency { get; set; }
        //public string NRPIncludingBrokerageUSD { get; set; }
        //public string BrokerageUSD { get; set; }
        //public string NetAbsoluteRiskPremiumUSD { get; set; }
        //public string Comment { get; set; }
        //public string Salesman { get; set; }
        //public string InsurencePolicyNo { get; set; }
        //public string AccidentandHealthMedicalExpensesCountryofTreatment { get; set; }
        //public string AccidentandHealthMedicalExpensesDateofTreatment { get; set; }
        //public string AccidentandHealthMedicalExpensesPatientName { get; set; }
        //public string AccidentandHealthMedicalExpensesPlan { get; set; }
        //public string AccidentandHealthMedicalExpensesTreatmentType { get; set; }
        //public string ClaimDetailsCatastropheName { get; set; }
        //public string ClaimDetailsCauseofLossCode { get; set; }
        //public string ClaimDetailsClaimStatus { get; set; }
        //public string ClaimDetailsClaimantName { get; set; }
        //public string ClaimDetailsDateClaimMade { get; set; }
        //public string ClaimDetailsDateClosed { get; set; }
        //public string ClaimDetailsDateFirstAdvisedNotificationDate { get; set; }
        //public string ClaimDetailsDateofLossFrom { get; set; }
        //public string ClaimDetailsDateofLossto { get; set; }
        //public string ClaimDetailsDenial { get; set; }
        //public string ClaimDetailsLloydsCatCode { get; set; }
        //public string ClaimDetailsLossDescription { get; set; }
        //public string ClaimDetailsRefertoUnderwriters { get; set; }
        //public string ClaimNotesDateclaimclosed { get; set; }
        //public string ClaimNotesAmountClaimed { get; set; }
        //public string ClaimNotesClaimnotpaidaswithinexcess { get; set; }
        //public string ClaimNotesComplaintReason { get; set; }
        //public string ClaimNotesDateClaimDenied { get; set; }
        //public string ClaimNotesDateclaimwithdrawn { get; set; }
        //public string ClaimNotesDateofComplaint { get; set; }
        //public string ClaimNotesExgratiapayment { get; set; }
        //public string ClaimNotesInLitigation { get; set; }
        //public string ClaimNotesReasonforDenial { get; set; }
        //public string ClaimStatusChangesDateFeesPaid { get; set; }
        //public string ClaimStatusChangesDateClaimOpened { get; set; }
        //public string ClaimStatusChangesDateClaimsPaid { get; set; }
        //public string ClaimStatusChangesDateCoverageConfirmed { get; set; }
        //public string ClaimStatusChangesDateofSubrogation { get; set; }
        //public string ClaimStatusChangesDateReopened { get; set; }
        //public string ClaimStatusChangesDateClaimAmountAgreed { get; set; }
        //public string ClaimantAddressClaimantAddress { get; set; }
        //public string ClaimantAddressClaimantCountry { get; set; }
        //public string ClaimantAddressClaimantPostcode { get; set; }
        //public string ClassofBusinessSpecificNameorRegNoofAircraftVehicleVesseletc { get; set; }
        //public string ClassofBusinessSpecificPercentCededReinsurance { get; set; }
        //public string ContractDetailsAgreementNo { get; set; }
        //public string ContractDetailsClassofBusiness { get; set; }
        //public string ContractDetailsContractExpiry { get; set; }
        //public string ContractDetailsContractInception { get; set; }
        //public string ContractDetailsCoverholderName { get; set; }
        //public string ContractDetailsCoverholderPIN { get; set; }
        //public string ContractDetailsLloydsRiskCode { get; set; }
        //public string ContractDetailsOriginalCurrency { get; set; }
        //public string ContractDetailsRateofExchange { get; set; }
        //public string ContractDetailsReportingPeriodEndDate { get; set; }
        //public string ContractDetailsReportingPeriodStartDate { get; set; }
        //public string ContractDetailsSectionNo { get; set; }
        //public string ContractDetailsSettlementCurrency { get; set; }
        //public string ContractDetailsTPAName { get; set; }
        //public string ContractDetailsTypeofInsuranceDirectorTypeofRI { get; set; }
        //public string ContractDetailsUniqueMarketReferenceUMR { get; set; }
        //public string ExpertsLawyerAdjusterAttorneyetcAddress { get; set; }
        //public string ExpertsLawyerAdjusterAttorneyetcCountry { get; set; }
        //public string ExpertsLawyerAdjusterAttorneyetcFirmCompanyName { get; set; }
        //public string ExpertsLawyerAdjusterAttorneyetcNotes { get; set; }
        //public string ExpertsLawyerAdjusterAttorneyetcPostcodeZipCodeorsimilar { get; set; }
        //public string ExpertsLawyerAdjusterAttorneyetcReferenceNoetc { get; set; }
        //public string ExpertsLawyerAdjusterAttorneyetcStateProvinceTerritoryCantonetc { get; set; }
        //public string ExpertsLawyerAdjusterAttorneyetcRole { get; set; }
        //public string InsuredDetailsAddress { get; set; }
        //public string InsuredDetailsCountry { get; set; }
        //public string InsuredDetailsFullNameorCompanyName { get; set; }
        //public string InsuredDetailsPostcodeZipCodeorsimilar { get; set; }
        //public string InsuredDetailsStateProvinceTerritoryCantonetc { get; set; }
        //public string LocationofLossAddress { get; set; }
        //public string LocationofLossCountry { get; set; }
        //public string LocationofLossPostcodeZipCodeorsimilar { get; set; }
        //public string LocationofLossStateProvinceTerritoryCantonetc { get; set; }
        //public string LocationofRiskAddress { get; set; }
        //public string LocationofRiskCountry { get; set; }
        //public string LocationofRiskLocationID { get; set; }
        //public string LocationofRiskPostcodeZipCodeorsimilar { get; set; }
        //public string LocationofRiskStateProvinceTerritoryCantonetc { get; set; }
        //public string ReferencesCertificateReference { get; set; }
        //public string ReferencesClaimReference { get; set; }
        //public string RefsPolicyorGroupRef { get; set; }
        //public string RiskDetailsRiskExpiryDate { get; set; }
        //public string RiskDetailsDeductibleAmount { get; set; }
        //public string RiskDetailsDeductibleBasis { get; set; }
        //public string RiskDetailsPeriodofCoverNarrative { get; set; }
        //public string RiskDetailsRiskInceptionDate { get; set; }
        //public string RiskDetailsSumsInsuredAmount { get; set; }
        //public string RowNo { get; set; }
        //public string TransactionDetailsChangethismonthFees { get; set; }
        //public string TransactionDetailsChangethismonthIndemnity { get; set; }
        //public string TransactionDetailsPaidthismonthAdjustersFees { get; set; }
        //public string TransactionDetailsPaidthismonthAttorneyCoverageFees { get; set; }
        //public string TransactionDetailsPaidthismonthDefenceFees { get; set; }
        //public string TransactionDetailsPaidthismonthExpenses { get; set; }
        //public string TransactionDetailsPaidthismonthFees { get; set; }
        //public string TransactionDetailsPaidthismonthIndemnity { get; set; }
        //public string TransactionDetailsPaidthismonthTPAFees { get; set; }
        //public string TransactionDetailsPreviouslyPaidAdjustersFees { get; set; }
        //public string TransactionDetailsPreviouslyPaidAttorneyCoverageFees { get; set; }
        //public string TransactionDetailsPreviouslyPaidDefenceFees { get; set; }
        //public string TransactionDetailsPreviouslyPaidExpenses { get; set; }
        //public string TransactionDetailsPreviouslyPaidFees { get; set; }
        //public string TransactionDetailsPreviouslyPaidIndemnity { get; set; }
        //public string TransactionDetailsPreviouslyPaidTPAFees { get; set; }
        //public string TransactionDetailsReserveAdjustersFees { get; set; }
        //public string TransactionDetailsReserveAttorneyCoverageFees { get; set; }
        //public string TransactionDetailsReserveDefenceFees { get; set; }
        //public string TransactionDetailsReserveExpenses { get; set; }
        //public string TransactionDetailsReserveFees { get; set; }
        //public string TransactionDetailsReserveIndemnity { get; set; }
        //public string TransactionDetailsReserveTPAFees { get; set; }
        //public string TransactionDetailsTotalIncurred { get; set; }
        //public string TransactionDetailsTotalIncurredFees { get; set; }
        //public string TransactionDetailsTotalIncurredIndemnity { get; set; }
        //public string USDetailsLossCounty { get; set; }
        //public string USDetailsMedicareConditionalPayments { get; set; }
        //public string USDetailsMedicareEligibilityCheckPerformance { get; set; }
        //public string USDetailsMedicareMSPComplianceServices { get; set; }
        //public string USDetailsMedicareOutcomeofEligilibilityStatusCheck { get; set; }
        //public string USDetailsMedicareUnitedStatesBodilyInjury { get; set; }
        //public string USDetailsPCSCode { get; set; }
        //public string USDetailsStateofFiling { get; set; }


        //public string ContractDetailsCoverholderName { get; set; }
        //public string ContractDetailsTPAName { get; set; }
        //public string ContractDetailsAgreementNo { get; set; }
        //public string ContractDetailsUniqueMarketReferenceUMR { get; set; }
        //public string ContractDetailsContractInception { get; set; }
        //public string ContractDetailsContractExpiry { get; set; }
        //public string ContractDetailsReportingPeriodEndDate { get; set; }
        //public string ContractDetailsClassofBusiness { get; set; }
        //public string ContractDetailsLloydsRiskCode { get; set; }
        //public string ContractDetailsSectionNo { get; set; }
        //public string ContractDetailsOriginalCurrency { get; set; }
        //public string ContractDetailsSettlementCurrency { get; set; }
        //public string ContractDetailsRateofExchange { get; set; }
        //public string ReferencesCertificateReference { get; set; }
        //public string ReferencesClaimReference { get; set; }
        //public string InsuredDetailsFullNameorCompanyName { get; set; }
        //public string InsuredDetailsStateProvinceTerritoryCantonetc { get; set; }
        //public string InsuredDetailsCountry { get; set; }
        //public string LocationofRiskStateProvinceTerritoryCantonetc { get; set; }
        //public string LocationofRiskCountry { get; set; }
        //public string RiskDetailsRiskInceptionDate { get; set; }
        //public string RiskDetailsRiskExpiryDate { get; set; }
        //public string RiskDetailsPeriodofCoverNarrative { get; set; }

        //public string LocationofLossCountry { get; set; }
        //public string ClaimDetailsCauseofLossCode { get; set; }
        //public string ClaimDetailsLossDescription { get; set; }
        //public string ClaimDetailsDateofLossFrom { get; set; }
        //public string ClaimDetailsDateofLossto { get; set; }
        //public string ClaimDetailsDateClaimMade { get; set; }
        //public string ClaimDetailsClaimStatus { get; set; }
        //public string ClaimDetailsRefertoUnderwriters { get; set; }
        //public string ClaimNotesReasonforDenial { get; set; }

        //public string USDetailsLossCounty { get; set; }
        //public string USDetailsMedicareConditionalPayments { get; set; }
        //public string USDetailsMedicareEligibilityCheckPerformance { get; set; }
        //public string USDetailsMedicareMSPComplianceServices { get; set; }
        //public string USDetailsMedicareOutcomeofEligilibilityStatusCheck { get; set; }
        //public string USDetailsMedicareUnitedStatesBodilyInjury { get; set; }
        //public string USDetailsPCSCode { get; set; }
        //public string USDetailsStateofFiling { get; set; }
        //public string TransactionDetailsChangethismonthFees { get; set; }
        //public string TransactionDetailsChangethismonthIndemnity { get; set; }
        //public string TransactionDetailsPaidthismonthAdjustersFees { get; set; }
        //public string TransactionDetailsPaidthismonthAttorneyCoverageFees { get; set; }
        //public string TransactionDetailsPaidthismonthDefenceFees { get; set; }
        //public string TransactionDetailsPaidthismonthExpenses { get; set; }
        //public string TransactionDetailsPaidthismonthFees { get; set; }
        //public string TransactionDetailsPaidthismonthIndemnity { get; set; }
        //public string TransactionDetailsPaidthismonthTPAFees { get; set; }
        //public string TransactionDetailsPreviouslyPaidAdjustersFees { get; set; }
        //public string TransactionDetailsPreviouslyPaidAttorneyCoverageFees { get; set; }
        //public string TransactionDetailsPreviouslyPaidDefenceFees { get; set; }
        //public string TransactionDetailsPreviouslyPaidExpenses { get; set; }
        //public string TransactionDetailsPreviouslyPaidFees { get; set; }
        //public string TransactionDetailsPreviouslyPaidIndemnity { get; set; }
        //public string TransactionDetailsPreviouslyPaidTPAFees { get; set; }
        //public string TransactionDetailsReserveAdjustersFees { get; set; }
        //public string TransactionDetailsReserveAttorneyCoverageFees { get; set; }
        //public string TransactionDetailsReserveDefenceFees { get; set; }
        //public string TransactionDetailsReserveExpenses { get; set; }
        //public string TransactionDetailsReserveFees { get; set; }
        //public string TransactionDetailsReserveIndemnity { get; set; }
        //public string TransactionDetailsReserveTPAFees { get; set; }
        //public string TransactionDetailsTotalIncurred { get; set; }
        //public string TransactionDetailsTotalIncurredFees { get; set; }
        //public string TransactionDetailsTotalIncurredIndemnity { get; set; }
        //public string ContractDetailsCoverholderPIN { get; set; }
        //public string ContractDetailsReportingPeriodStartDate { get; set; }
        //public string ContractDetailsTypeofInsuranceDirectorTypeofRI { get; set; }
        //public string RefsPolicyorGroupRef { get; set; }
        //public string InsuredDetailsAddress { get; set; }
        //public string InsuredDetailsPostcodeZipCodeorsimilar { get; set; }
        //public string LocationofRiskLocationID { get; set; }
        //public string LocationofRiskAddress { get; set; }
        //public string LocationofRiskPostcodeZipCodeorsimilar { get; set; }
        //public string RiskDetailsDeductibleAmount { get; set; }
        //public string RiskDetailsDeductibleBasis { get; set; }
        //public string RiskDetailsSumsInsuredAmount { get; set; }
        //public string LocationofLossAddress { get; set; }
        //public string ClassofBusinessSpecificNameorRegNoofAircraftVehicleVesseletc { get; set; }
        //public string ClassofBusinessSpecificPercentCededReinsurance { get; set; }
        //public string AccidentandHealthMedicalExpensesDateofTreatment { get; set; }
        //public string AccidentandHealthMedicalExpensesPatientName { get; set; }
        //public string AccidentandHealthMedicalExpensesPlan { get; set; }
        //public string AccidentandHealthMedicalExpensesTreatmentType { get; set; }
        //public string AccidentandHealthMedicalExpensesCountryofTreatment { get; set; }
        //public string ExpertsLawyerAdjusterAttorneyetcAddress { get; set; }
        //public string ExpertsLawyerAdjusterAttorneyetcCountry { get; set; }
        //public string ExpertsLawyerAdjusterAttorneyetcFirmCompanyName { get; set; }
        //public string ExpertsLawyerAdjusterAttorneyetcNotes { get; set; }
        //public string ExpertsLawyerAdjusterAttorneyetcPostcodeZipCodeorsimilar { get; set; }
        //public string ExpertsLawyerAdjusterAttorneyetcReferenceNoetc { get; set; }
        //public string ExpertsLawyerAdjusterAttorneyetcStateProvinceTerritoryCantonetc { get; set; }
        //public string ClaimStatusChangesDateClaimAmountAgreed { get; set; }
        //public string ClaimStatusChangesDateClaimOpened { get; set; }
        //public string ClaimStatusChangesDateClaimsPaid { get; set; }
        //public string ClaimStatusChangesDateCoverageConfirmed { get; set; }
        //public string ClaimStatusChangesDateFeesPaid { get; set; }
        //public string ClaimStatusChangesDateofSubrogation { get; set; }
        //public string ClaimStatusChangesDateReopened { get; set; }
        //public string ClaimantAddressClaimantAddress { get; set; }
        //public string ClaimantAddressClaimantCountry { get; set; }
        //public string ClaimantAddressClaimantPostcode { get; set; }
        //public string ClaimNotesAmountClaimed { get; set; }
        //public string ClaimNotesClaimnotpaidaswithinexcess { get; set; }
        //public string ClaimNotesComplaintReason { get; set; }
        //public string ClaimNotesDateclaimclosed { get; set; }
        //public string ClaimNotesDateClaimDenied { get; set; }
        //public string ClaimNotesDateclaimwithdrawn { get; set; }
        //public string ClaimNotesDateofComplaint { get; set; }
        //public string ClaimNotesExgratiapayment { get; set; }
        //public string ClaimNotesInLitigation { get; set; }





    }
}
