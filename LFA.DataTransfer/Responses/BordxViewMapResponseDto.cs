using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class BordxViewMapResponseDto
    {
        public int SNo { get; set; }
        public bool ContractYear { get; set; }
        public bool Reinsurer { get; set; }
        public bool Insurer { get; set; }
        public bool CountryOrignCode { get; set; }
        public bool ReportingMonth { get; set; }
        public bool Program { get; set; }
        public bool RIPremiumType { get; set; }
        public bool Status { get; set; }
        public bool CoverType { get; set; }
        public bool ItemStatus { get; set; }
        public bool Mileage { get; set; }
        public bool Seller { get; set; }
        public bool PointOfSale { get; set; }
        public bool FirstName { get; set; }
        public bool Surname { get; set; }
        public bool Address { get; set; }
        public bool City { get; set; }
        public bool Tel { get; set; }
        public bool VINNo { get; set; }
        public bool Registration { get; set; }
        public bool Category { get; set; }
        public bool VehicleManufacturer { get; set; }
        public bool MakeNModel { get; set; }
        public bool CylinderCount { get; set; }
        public bool EngineCapacity { get; set; }
        public bool ModelYear { get; set; }
        public bool SystemGeneratedIdentification { get; set; }
        public bool DatePurchaseRegistration { get; set; }
        public bool ManufacturersExpirey { get; set; }
        public bool ManufacturerCoverInMonth { get; set; }
        public bool LimitationsHoursMileagePeriod { get; set; }
        public bool RiskStartDate { get; set; }
        public bool RiskTerminationDate { get; set; }
        public bool CoverPeriodMonths { get; set; }
        public bool ExtensonPeriodHoursMileage { get; set; }
        public bool GrossPremium { get; set; }
        public bool SalesmanCommission { get; set; }
        public bool GrossPremiumLessSalesCommission { get; set; }
        public bool InsurerFee { get; set; }
        public bool Retention { get; set; }
        public bool ManufactureCommission { get; set; }
        public bool IPT { get; set; }
        public bool LocalTax { get; set; }
        public bool VAT { get; set; }
        public bool DocumentFee { get; set; }
        public bool SalesTax { get; set; }
        public bool Total { get; set; }
        public bool ClientBrokerage { get; set; }
        public bool GrossPremiumLessCommission { get; set; }
        public bool NRPIncludingBrokerage { get; set; }
        public bool Brokerage { get; set; }
        public bool NetAbsoluteRiskPremium { get; set; }
        public bool AdministrationFee { get; set; }
        public bool SumInsuredInLocalCurrency { get; set; }
        public bool NRPIncludingBrokerageUSD { get; set; }
        public bool BrokerageUSD { get; set; }
        public bool NetAbsoluteRiskPremiumUSD { get; set; }
        public bool Comment { get; set; }
        public bool Salesman { get; set; }
        public bool InsurencePolicyNo { get; set; }
        public bool AccidentandHealthMedicalExpensesCountryofTreatment { get; set; }
        public bool AccidentandHealthMedicalExpensesDateofTreatment { get; set; }
        public bool AccidentandHealthMedicalExpensesPatientName { get; set; }
        public bool AccidentandHealthMedicalExpensesPlan { get; set; }
        public bool AccidentandHealthMedicalExpensesTreatmentType { get; set; }
        public bool ClaimDetailsCatastropheName { get; set; }
        public bool ClaimDetailsCauseofLossCode { get; set; }
        public bool ClaimDetailsClaimStatus { get; set; }
        public bool ClaimDetailsClaimantName { get; set; }
        public bool ClaimDetailsDateClaimMade { get; set; }
        public bool ClaimDetailsDateClosed { get; set; }
        public bool ClaimDetailsDateFirstAdvisedNotificationDate { get; set; }
        public bool ClaimDetailsDateofLossFrom { get; set; }
        public bool ClaimDetailsDateofLossto { get; set; }
        public bool ClaimDetailsDenial { get; set; }
        public bool ClaimDetailsLloydsCatCode { get; set; }
        public bool ClaimDetailsLossDescription { get; set; }
        public bool ClaimDetailsRefertoUnderwriters { get; set; }
        public bool ClaimNotesDateclaimclosed { get; set; }
        public bool ClaimNotesAmountClaimed { get; set; }
        public bool ClaimNotesClaimnotpaidaswithinexcess { get; set; }
        public bool ClaimNotesComplaintReason { get; set; }
        public bool ClaimNotesDateClaimDenied { get; set; }
        public bool ClaimNotesDateclaimwithdrawn { get; set; }
        public bool ClaimNotesDateofComplaint { get; set; }
        public bool ClaimNotesExgratiapayment { get; set; }
        public bool ClaimNotesInLitigation { get; set; }
        public bool ClaimNotesReasonforDenial { get; set; }
        public bool ClaimStatusChangesDateFeesPaid { get; set; }
        public bool ClaimStatusChangesDateClaimOpened { get; set; }
        public bool ClaimStatusChangesDateClaimsPaid { get; set; }
        public bool ClaimStatusChangesDateCoverageConfirmed { get; set; }
        public bool ClaimStatusChangesDateofSubrogation { get; set; }
        public bool ClaimStatusChangesDateReopened { get; set; }
        public bool ClaimStatusChangesDateClaimAmountAgreed { get; set; }
        public bool ClaimantAddressClaimantAddress { get; set; }
        public bool ClaimantAddressClaimantCountry { get; set; }
        public bool ClaimantAddressClaimantPostcode { get; set; }
        public bool ClassofBusinessSpecificNameorRegNoofAircraftVehicleVesseletc { get; set; }
        public bool ClassofBusinessSpecificPercentCededReinsurance { get; set; }
        public bool ContractDetailsAgreementNo { get; set; }
        public bool ContractDetailsClassofBusiness { get; set; }
        public bool ContractDetailsContractExpiry { get; set; }
        public bool ContractDetailsContractInception { get; set; }
        public bool ContractDetailsCoverholderName { get; set; }
        public bool ContractDetailsCoverholderPIN { get; set; }
        public bool ContractDetailsLloydsRiskCode { get; set; }
        public bool ContractDetailsOriginalCurrency { get; set; }
        public bool ContractDetailsRateofExchange { get; set; }
        public bool ContractDetailsReportingPeriodEndDate { get; set; }
        public bool ContractDetailsReportingPeriodStartDate { get; set; }
        public bool ContractDetailsSectionNo { get; set; }
        public bool ContractDetailsSettlementCurrency { get; set; }
        public bool ContractDetailsTPAName { get; set; }
        public bool ContractDetailsTypeofInsuranceDirectorTypeofRI { get; set; }
        public bool ContractDetailsUniqueMarketReferenceUMR { get; set; }
        public bool ExpertsLawyerAdjusterAttorneyetcAddress { get; set; }
        public bool ExpertsLawyerAdjusterAttorneyetcCountry { get; set; }
        public bool ExpertsLawyerAdjusterAttorneyetcFirmCompanyName { get; set; }
        public bool ExpertsLawyerAdjusterAttorneyetcNotes { get; set; }
        public bool ExpertsLawyerAdjusterAttorneyetcPostcodeZipCodeorsimilar { get; set; }
        public bool ExpertsLawyerAdjusterAttorneyetcReferenceNoetc { get; set; }
        public bool ExpertsLawyerAdjusterAttorneyetcStateProvinceTerritoryCantonetc { get; set; }
        public bool ExpertsLawyerAdjusterAttorneyetcRole { get; set; }
        public bool InsuredDetailsAddress { get; set; }
        public bool InsuredDetailsCountry { get; set; }
        public bool InsuredDetailsFullNameorCompanyName { get; set; }
        public bool InsuredDetailsPostcodeZipCodeorsimilar { get; set; }
        public bool InsuredDetailsStateProvinceTerritoryCantonetc { get; set; }
        public bool LocationofLossAddress { get; set; }
        public bool LocationofLossCountry { get; set; }
        public bool LocationofLossPostcodeZipCodeorsimilar { get; set; }
        public bool LocationofLossStateProvinceTerritoryCantonetc { get; set; }
        public bool LocationofRiskAddress { get; set; }
        public bool LocationofRiskCountry { get; set; }
        public bool LocationofRiskLocationID { get; set; }
        public bool LocationofRiskPostcodeZipCodeorsimilar { get; set; }
        public bool LocationofRiskStateProvinceTerritoryCantonetc { get; set; }
        public bool ReferencesCertificateReference { get; set; }
        public bool ReferencesClaimReference { get; set; }
        public bool RefsPolicyorGroupRef { get; set; }
        public bool RiskDetailsRiskExpiryDate { get; set; }
        public bool RiskDetailsDeductibleAmount { get; set; }
        public bool RiskDetailsDeductibleBasis { get; set; }
        public bool RiskDetailsPeriodofCoverNarrative { get; set; }
        public bool RiskDetailsRiskInceptionDate { get; set; }
        public bool RiskDetailsSumsInsuredAmount { get; set; }
        public bool RowNo { get; set; }
        public bool TransactionDetailsChangethismonthFees { get; set; }
        public bool TransactionDetailsChangethismonthIndemnity { get; set; }
        public bool TransactionDetailsPaidthismonthAdjustersFees { get; set; }
        public bool TransactionDetailsPaidthismonthAttorneyCoverageFees { get; set; }
        public bool TransactionDetailsPaidthismonthDefenceFees { get; set; }
        public bool TransactionDetailsPaidthismonthExpenses { get; set; }
        public bool TransactionDetailsPaidthismonthFees { get; set; }
        public bool TransactionDetailsPaidthismonthIndemnity { get; set; }
        public bool TransactionDetailsPaidthismonthTPAFees { get; set; }
        public bool TransactionDetailsPreviouslyPaidAdjustersFees { get; set; }
        public bool TransactionDetailsPreviouslyPaidAttorneyCoverageFees { get; set; }
        public bool TransactionDetailsPreviouslyPaidDefenceFees { get; set; }
        public bool TransactionDetailsPreviouslyPaidExpenses { get; set; }
        public bool TransactionDetailsPreviouslyPaidFees { get; set; }
        public bool TransactionDetailsPreviouslyPaidIndemnity { get; set; }
        public bool TransactionDetailsPreviouslyPaidTPAFees { get; set; }
        public bool TransactionDetailsReserveAdjustersFees { get; set; }
        public bool TransactionDetailsReserveAttorneyCoverageFees { get; set; }
        public bool TransactionDetailsReserveDefenceFees { get; set; }
        public bool TransactionDetailsReserveExpenses { get; set; }
        public bool TransactionDetailsReserveFees { get; set; }
        public bool TransactionDetailsReserveIndemnity { get; set; }
        public bool TransactionDetailsReserveTPAFees { get; set; }
        public bool TransactionDetailsTotalIncurred { get; set; }
        public bool TransactionDetailsTotalIncurredFees { get; set; }
        public bool TransactionDetailsTotalIncurredIndemnity { get; set; }
        public bool USDetailsLossCounty { get; set; }
        public bool USDetailsMedicareConditionalPayments { get; set; }
        public bool USDetailsMedicareEligibilityCheckPerformance { get; set; }
        public bool USDetailsMedicareMSPComplianceServices { get; set; }
        public bool USDetailsMedicareOutcomeofEligilibilityStatusCheck { get; set; }
        public bool USDetailsMedicareUnitedStatesBodilyInjury { get; set; }
        public bool USDetailsPCSCode { get; set; }
        public bool USDetailsStateofFiling { get; set; }

        public BordxViewMapResponseDto()
        {
            ContractYear = false;
            Reinsurer = false;
            Insurer = false;
            CountryOrignCode = false;
            ReportingMonth = false;
            Program = false;
            RIPremiumType = false;
            Status = false;
            CoverType = false;
            ItemStatus = false;
            Mileage = false;
            Seller = false;
            PointOfSale = false;
            FirstName = false;
            Surname = false;
            Address = false;
            City = false;
            Tel = false;
            VINNo = false;
            Registration = false;
            Category = false;
            VehicleManufacturer = false;
            MakeNModel = false;
            CylinderCount = false;
            EngineCapacity = false;
            ModelYear = false;
            SystemGeneratedIdentification = false;
            DatePurchaseRegistration = false;
            ManufacturersExpirey = false;
            ManufacturerCoverInMonth = false;
            LimitationsHoursMileagePeriod = false;
            RiskStartDate = false;
            RiskTerminationDate = false;
            CoverPeriodMonths = false;
            ExtensonPeriodHoursMileage = false;
            GrossPremium = false;
            SalesmanCommission = false;
            GrossPremiumLessSalesCommission = false;
            InsurerFee = false;
            Retention = false;
            ManufactureCommission = false;
            IPT = false;
            LocalTax = false;
            VAT = false;
            DocumentFee = false;
            SalesTax = false;
            Total = false;
            ClientBrokerage = false;
            GrossPremiumLessCommission = false;
            NRPIncludingBrokerage = false;
            Brokerage = false;
            NetAbsoluteRiskPremium = false;
            AdministrationFee = false;
            SumInsuredInLocalCurrency = false;
            NRPIncludingBrokerageUSD = false;
            BrokerageUSD = false;
            NetAbsoluteRiskPremiumUSD = false;
            Comment = false;
            Salesman = false;
            InsurencePolicyNo = false;
            AccidentandHealthMedicalExpensesCountryofTreatment = false;
            AccidentandHealthMedicalExpensesDateofTreatment = false;
            AccidentandHealthMedicalExpensesPatientName = false;
            AccidentandHealthMedicalExpensesPlan = false;
            AccidentandHealthMedicalExpensesTreatmentType = false;
            ClaimDetailsCatastropheName = false;
            ClaimDetailsCauseofLossCode = false;
            ClaimDetailsClaimStatus = false;
            ClaimDetailsClaimantName = false;
            ClaimDetailsDateClaimMade = false;
            ClaimDetailsDateClosed = false;
            ClaimDetailsDateFirstAdvisedNotificationDate = false;
            ClaimDetailsDateofLossFrom = false;
            ClaimDetailsDateofLossto = false;
            ClaimDetailsDenial = false;
            ClaimDetailsLloydsCatCode = false;
            ClaimDetailsLossDescription = false;
            ClaimDetailsRefertoUnderwriters = false;
            ClaimNotesDateclaimclosed = false;
            ClaimNotesAmountClaimed = false;
            ClaimNotesClaimnotpaidaswithinexcess = false;
            ClaimNotesComplaintReason = false;
            ClaimNotesDateClaimDenied = false;
            ClaimNotesDateclaimwithdrawn = false;
            ClaimNotesDateofComplaint = false;
            ClaimNotesExgratiapayment = false;
            ClaimNotesInLitigation = false;
            ClaimNotesReasonforDenial = false;
            ClaimStatusChangesDateFeesPaid = false;
            ClaimStatusChangesDateClaimOpened = false;
            ClaimStatusChangesDateClaimsPaid = false;
            ClaimStatusChangesDateCoverageConfirmed = false;
            ClaimStatusChangesDateofSubrogation = false;
            ClaimStatusChangesDateReopened = false;
            ClaimStatusChangesDateClaimAmountAgreed = false;
            ClaimantAddressClaimantAddress = false;
            ClaimantAddressClaimantCountry = false;
            ClaimantAddressClaimantPostcode = false;
            ClassofBusinessSpecificNameorRegNoofAircraftVehicleVesseletc = false;
            ClassofBusinessSpecificPercentCededReinsurance = false;
            ContractDetailsAgreementNo = false;
            ContractDetailsClassofBusiness = false;
            ContractDetailsContractExpiry = false;
            ContractDetailsContractInception = false;
            ContractDetailsCoverholderName = false;
            ContractDetailsCoverholderPIN = false;
            ContractDetailsLloydsRiskCode = false;
            ContractDetailsOriginalCurrency = false;
            ContractDetailsRateofExchange = false;
            ContractDetailsReportingPeriodEndDate = false;
            ContractDetailsReportingPeriodStartDate = false;
            ContractDetailsSectionNo = false;
            ContractDetailsSettlementCurrency = false;
            ContractDetailsTPAName = false;
            ContractDetailsTypeofInsuranceDirectorTypeofRI = false;
            ContractDetailsUniqueMarketReferenceUMR = false;
            ExpertsLawyerAdjusterAttorneyetcAddress = false;
            ExpertsLawyerAdjusterAttorneyetcCountry = false;
            ExpertsLawyerAdjusterAttorneyetcFirmCompanyName = false;
            ExpertsLawyerAdjusterAttorneyetcNotes = false;
            ExpertsLawyerAdjusterAttorneyetcPostcodeZipCodeorsimilar = false;
            ExpertsLawyerAdjusterAttorneyetcReferenceNoetc = false;
            ExpertsLawyerAdjusterAttorneyetcStateProvinceTerritoryCantonetc = false;
            ExpertsLawyerAdjusterAttorneyetcRole = false;
            InsuredDetailsAddress = false;
            InsuredDetailsCountry = false;
            InsuredDetailsFullNameorCompanyName = false;
            InsuredDetailsPostcodeZipCodeorsimilar = false;
            InsuredDetailsStateProvinceTerritoryCantonetc = false;
            LocationofLossAddress = false;
            LocationofLossCountry = false;
            LocationofLossPostcodeZipCodeorsimilar = false;
            LocationofLossStateProvinceTerritoryCantonetc = false;
            LocationofRiskAddress = false;
            LocationofRiskCountry = false;
            LocationofRiskLocationID = false;
            LocationofRiskPostcodeZipCodeorsimilar = false;
            LocationofRiskStateProvinceTerritoryCantonetc = false;
            ReferencesCertificateReference = false;
            ReferencesClaimReference = false;
            RefsPolicyorGroupRef = false;
            RiskDetailsRiskExpiryDate = false;
            RiskDetailsDeductibleAmount = false;
            RiskDetailsDeductibleBasis = false;
            RiskDetailsPeriodofCoverNarrative = false;
            RiskDetailsRiskInceptionDate = false;
            RiskDetailsSumsInsuredAmount = false;
            RowNo = false;
            TransactionDetailsChangethismonthFees = false;
            TransactionDetailsChangethismonthIndemnity = false;
            TransactionDetailsPaidthismonthAdjustersFees = false;
            TransactionDetailsPaidthismonthAttorneyCoverageFees = false;
            TransactionDetailsPaidthismonthDefenceFees = false;
            TransactionDetailsPaidthismonthExpenses = false;
            TransactionDetailsPaidthismonthFees = false;
            TransactionDetailsPaidthismonthIndemnity = false;
            TransactionDetailsPaidthismonthTPAFees = false;
            TransactionDetailsPreviouslyPaidAdjustersFees = false;
            TransactionDetailsPreviouslyPaidAttorneyCoverageFees = false;
            TransactionDetailsPreviouslyPaidDefenceFees = false;
            TransactionDetailsPreviouslyPaidExpenses = false;
            TransactionDetailsPreviouslyPaidFees = false;
            TransactionDetailsPreviouslyPaidIndemnity = false;
            TransactionDetailsPreviouslyPaidTPAFees = false;
            TransactionDetailsReserveAdjustersFees = false;
            TransactionDetailsReserveAttorneyCoverageFees = false;
            TransactionDetailsReserveDefenceFees = false;
            TransactionDetailsReserveExpenses = false;
            TransactionDetailsReserveFees = false;
            TransactionDetailsReserveIndemnity = false;
            TransactionDetailsReserveTPAFees = false;
            TransactionDetailsTotalIncurred = false;
            TransactionDetailsTotalIncurredFees = false;
            TransactionDetailsTotalIncurredIndemnity = false;
            USDetailsLossCounty = false;
            USDetailsMedicareConditionalPayments = false;
            USDetailsMedicareEligibilityCheckPerformance = false;
            USDetailsMedicareMSPComplianceServices = false;
            USDetailsMedicareOutcomeofEligilibilityStatusCheck = false;
            USDetailsMedicareUnitedStatesBodilyInjury = false;
            USDetailsPCSCode = false;
            USDetailsStateofFiling = false;
        }
    }
}
