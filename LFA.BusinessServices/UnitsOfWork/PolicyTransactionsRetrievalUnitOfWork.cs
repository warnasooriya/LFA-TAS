using TAS.Services.Entities.Management;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Entities.Persistence;
using TAS.Services.Entities;

using TAS.Services.Common;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class PolicyTransactionsRetrievalUnitOfWork : UnitOfWork
    {
       

        public PolicyTransactionsResponseDto Result
        {
            get;
            private set;
        }
        public override bool PreExecute()
        {
             try
            {
                Common.JWTHelper JWTHelper = new Common.JWTHelper(SecurityContext);
                Dictionary<string, object> str = JWTHelper.DecodeAuthenticationToken();
                string dbName = str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
                if (dbName != "")
                {
                    TASEntitySessionManager.OpenSession();
                    string tasTpaConnString = TASTPAEntityManager.GetTPAConnStringBydbName(dbName);
                    TASEntitySessionManager.CloseSession();
                    if (tasTpaConnString != "")
                    {
                        dbConnectionString = tasTpaConnString;
                        EntitySessionManager.OpenSession(dbConnectionString);
                        if (JWTHelper.checkTokenValidity(Convert.ToInt32(ConfigurationData.tasTokenValidTime.ToString())))
                        {
                            return true;
                        }
                        EntitySessionManager.CloseSession();
                    }
                }
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
            return false;
        }

        public override void Execute()
        {

            try
            {
                //temp section can remove  //**  lines once preexecute jwt and db name working fine
                if (dbConnectionString != null)   //**
                {     //**
                    EntitySessionManager.OpenSession(dbConnectionString);
                }     //**
                else     //**
                {     //**
                    EntitySessionManager.OpenSession();     //**
                }     //**
                PolicyEntityManager PolicyEndorsementEntityManager = new PolicyEntityManager();
                List<PolicyTransaction> PolicyEndorsementEntities = PolicyEndorsementEntityManager.GetPolicyEndorsements();


                PolicyTransactionsResponseDto result = new PolicyTransactionsResponseDto();
                result.Policies = new List<PolicyTransactionResponseDto>();
                foreach (var PolicyEndorsement in PolicyEndorsementEntities)
                {
                    PolicyTransactionResponseDto pr = new PolicyTransactionResponseDto();
                    pr.Id = PolicyEndorsement.Id;
                    pr.PolicyId = PolicyEndorsement.PolicyId;
                    pr.AddnSerialNo = PolicyEndorsement.AddnSerialNo;
                    pr.Address1 = PolicyEndorsement.Address1;
                    pr.Address2 = PolicyEndorsement.Address2;
                    pr.Address3 = PolicyEndorsement.Address3;
                    pr.Address4 = PolicyEndorsement.Address4;
                    pr.AspirationId = PolicyEndorsement.AspirationId;
                    pr.BodyTypeId = PolicyEndorsement.BodyTypeId;
                    pr.BusinessAddress1 = PolicyEndorsement.BusinessAddress1;
                    pr.BusinessAddress2 = PolicyEndorsement.BusinessAddress2;
                    pr.BusinessAddress3 = PolicyEndorsement.BusinessAddress3;
                    pr.BusinessAddress4 = PolicyEndorsement.BusinessAddress4;
                    pr.BusinessName = PolicyEndorsement.BusinessName;
                    pr.BusinessTelNo = PolicyEndorsement.BusinessTelNo;
                    pr.CategoryId = PolicyEndorsement.CategoryId;
                    pr.CityId = PolicyEndorsement.CityId;
                    pr.Comment = PolicyEndorsement.Comment;
                    pr.CommodityTypeId = PolicyEndorsement.CommodityTypeId;
                    pr.ContractId = PolicyEndorsement.ContractId;
                    pr.CountryId = PolicyEndorsement.CountryId;
                    pr.CoverTypeId = PolicyEndorsement.CoverTypeId;
                    pr.CustomerPayment = PolicyEndorsement.CustomerPayment;
                    pr.CustomerPaymentCurrencyTypeId = PolicyEndorsement.CustomerPaymentCurrencyTypeId;
                    pr.CustomerTypeId = PolicyEndorsement.CustomerTypeId;
                    pr.CylinderCountId = PolicyEndorsement.CylinderCountId;
                    pr.RegistrationDate = PolicyEndorsement.RegistrationDate;
                    pr.DateOfBirth = PolicyEndorsement.DateOfBirth;
                    pr.DealerId = PolicyEndorsement.DealerId;
                    pr.DealerLocationId = PolicyEndorsement.DealerLocationId;
                    pr.DealerPayment = PolicyEndorsement.DealerPayment;
                    pr.DealerPaymentCurrencyTypeId = PolicyEndorsement.DealerPaymentCurrencyTypeId;
                    pr.DealerPrice = PolicyEndorsement.DealerPrice;
                    pr.DLIssueDate = PolicyEndorsement.DLIssueDate;
                    pr.DriveTypeId = PolicyEndorsement.DriveTypeId;
                    pr.Email = PolicyEndorsement.Email;
                    pr.EngineCapacityId = PolicyEndorsement.EngineCapacityId;
                    pr.ExtensionTypeId = PolicyEndorsement.ExtensionTypeId;
                    pr.FirstName = PolicyEndorsement.FirstName;
                    pr.FuelTypeId = PolicyEndorsement.FuelTypeId;
                    pr.Gender = PolicyEndorsement.Gender;
                    pr.HrsUsedAtPolicySale = PolicyEndorsement.HrsUsedAtPolicySale;
                    pr.IDNo = PolicyEndorsement.IDNo;
                    pr.IDTypeId = PolicyEndorsement.IDTypeId;
                    pr.InvoiceNo = PolicyEndorsement.InvoiceNo;
                    pr.IsActive = PolicyEndorsement.IsActive;
                    pr.IsApproved = PolicyEndorsement.IsApproved;
                    pr.IsPartialPayment = PolicyEndorsement.IsPartialPayment;
                    pr.IsPreWarrantyCheck = PolicyEndorsement.IsPreWarrantyCheck;
                    pr.IsSpecialDeal = PolicyEndorsement.IsSpecialDeal;
                    pr.ItemPrice = PolicyEndorsement.ItemPrice;
                    pr.ItemPurchasedDate = PolicyEndorsement.ItemPurchasedDate;
                    pr.ItemStatusId = PolicyEndorsement.ItemStatusId;
                    pr.LastName = PolicyEndorsement.LastName;
                    pr.MakeId = PolicyEndorsement.MakeId;
                    pr.MobileNo = PolicyEndorsement.MobileNo;
                    pr.ModelCode = PolicyEndorsement.ModelCode;
                    pr.ModelId = PolicyEndorsement.ModelId;
                    pr.ModelYear = PolicyEndorsement.ModelYear;
                    pr.NationalityId = PolicyEndorsement.NationalityId;
                    pr.OtherTelNo = PolicyEndorsement.OtherTelNo;
                    pr.Password = PolicyEndorsement.Password;
                    pr.PaymentModeId = PolicyEndorsement.PaymentModeId;
                    pr.PlateNo = PolicyEndorsement.PlateNo;
                    pr.PolicyNo = PolicyEndorsement.PolicyNo;
                    pr.PolicySoldDate = PolicyEndorsement.PolicySoldDate;
                    pr.Premium = PolicyEndorsement.Premium;
                    pr.PremiumCurrencyTypeId = PolicyEndorsement.PremiumCurrencyTypeId;
                    pr.ProductId = PolicyEndorsement.ProductId;
                    pr.ProfilePicture = PolicyEndorsement.ProfilePicture;
                    pr.RefNo = PolicyEndorsement.RefNo;
                    pr.SalesPersonId = PolicyEndorsement.SalesPersonId;
                    pr.SerialNo = PolicyEndorsement.SerialNo;
                    pr.TransmissionId = PolicyEndorsement.TransmissionId;
                    pr.UsageTypeId = PolicyEndorsement.UsageTypeId;
                    pr.UserName = PolicyEndorsement.UserName;
                    pr.Variant = PolicyEndorsement.Variant;
                    pr.VehiclePrice = PolicyEndorsement.VehiclePrice;
                    pr.VINNo = PolicyEndorsement.VINNo;                 
                    pr.VehicleId = PolicyEndorsement.VehicleId;
                    pr.CustomerId = PolicyEndorsement.CustomerId;
                    pr.BAndWId = PolicyEndorsement.BAndWId;
                    pr.TransactionTypeId = PolicyEndorsement.TransactionTypeId;
                    pr.CancelationComment = PolicyEndorsement.CancelationComment;
                    pr.IsRecordActive = PolicyEndorsement.IsRecordActive;
                    pr.PolicyStartDate = PolicyEndorsement.PolicyStartDate;
                    pr.PolicyEndDate = PolicyEndorsement.PolicyEndDate;
                    pr.Discount = PolicyEndorsement.Discount;
                    pr.ModifiedUser = PolicyEndorsement.ModifiedUser;
                    pr.PolicyBundleId = PolicyEndorsement.PolicyBundleId;
                    pr.TransferFee = PolicyEndorsement.TransferFee;
                    pr.DealerPolicy = PolicyEndorsement.DealerPolicy;
                    //need to write other fields
                    result.Policies.Add(pr);
                }
                this.Result = result;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }

    }
}
