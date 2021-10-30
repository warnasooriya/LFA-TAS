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
    internal sealed class PolicyHistoriesRetrievalUnitOfWork : UnitOfWork
    {
       

        public PolicyHistoriesResponseDto Result
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
                PolicyEntityManager PolicyHistoryEntityManager = new PolicyEntityManager();
                List<PolicyHistory> PolicyHistoryEntities = PolicyHistoryEntityManager.GetPolicyHistories();


                PolicyHistoriesResponseDto result = new PolicyHistoriesResponseDto();
                result.Policies = new List<PolicyHistoryResponseDto>();
                foreach (var PolicyHistory in PolicyHistoryEntities)
                {
                    PolicyHistoryResponseDto pr = new PolicyHistoryResponseDto();
                    pr.Id = PolicyHistory.Id;
                    pr.PolicyId = PolicyHistory.PolicyId;
                    pr.AddnSerialNo = PolicyHistory.AddnSerialNo;
                    pr.Address1 = PolicyHistory.Address1;
                    pr.Address2 = PolicyHistory.Address2;
                    pr.Address3 = PolicyHistory.Address3;
                    pr.Address4 = PolicyHistory.Address4;
                    pr.AspirationId = PolicyHistory.AspirationId;
                    pr.BodyTypeId = PolicyHistory.BodyTypeId;
                    pr.BusinessAddress1 = PolicyHistory.BusinessAddress1;
                    pr.BusinessAddress2 = PolicyHistory.BusinessAddress2;
                    pr.BusinessAddress3 = PolicyHistory.BusinessAddress3;
                    pr.BusinessAddress4 = PolicyHistory.BusinessAddress4;
                    pr.BusinessName = PolicyHistory.BusinessName;
                    pr.BusinessTelNo = PolicyHistory.BusinessTelNo;
                    pr.CategoryId = PolicyHistory.CategoryId;
                    pr.CityId = PolicyHistory.CityId;
                    pr.Comment = PolicyHistory.Comment;
                    pr.CommodityTypeId = PolicyHistory.CommodityTypeId;
                    pr.ContractId = PolicyHistory.ContractId;
                    pr.CountryId = PolicyHistory.CountryId;
                    pr.CoverTypeId = PolicyHistory.CoverTypeId;
                    pr.CustomerPayment = PolicyHistory.CustomerPayment;
                    pr.CustomerPaymentCurrencyTypeId = PolicyHistory.CustomerPaymentCurrencyTypeId;
                    pr.CustomerTypeId = PolicyHistory.CustomerTypeId;
                    pr.CylinderCountId = PolicyHistory.CylinderCountId;
                    pr.DateOfBirth = PolicyHistory.DateOfBirth;
                    pr.DealerId = PolicyHistory.DealerId;
                    pr.DealerLocationId = PolicyHistory.DealerLocationId;
                    pr.DealerPayment = PolicyHistory.DealerPayment;
                    pr.DealerPaymentCurrencyTypeId = PolicyHistory.DealerPaymentCurrencyTypeId;
                    pr.DealerPrice = PolicyHistory.DealerPrice;
                    pr.DLIssueDate = PolicyHistory.DLIssueDate;
                    pr.DriveTypeId = PolicyHistory.DriveTypeId;
                    pr.Email = PolicyHistory.Email;
                    pr.EngineCapacityId = PolicyHistory.EngineCapacityId;
                    pr.ExtensionTypeId = PolicyHistory.ExtensionTypeId;
                    pr.FirstName = PolicyHistory.FirstName;
                    pr.FuelTypeId = PolicyHistory.FuelTypeId;
                    pr.Gender = PolicyHistory.Gender;
                    pr.HrsUsedAtPolicySale = PolicyHistory.HrsUsedAtPolicySale;
                    pr.IDNo = PolicyHistory.IDNo;
                    pr.IDTypeId = PolicyHistory.IDTypeId;
                    pr.InvoiceNo = PolicyHistory.InvoiceNo;
                    pr.IsActive = PolicyHistory.IsActive;
                    pr.IsApproved = PolicyHistory.IsApproved;
                    pr.IsPartialPayment = PolicyHistory.IsPartialPayment;
                    pr.IsPreWarrantyCheck = PolicyHistory.IsPreWarrantyCheck;
                    pr.IsSpecialDeal = PolicyHistory.IsSpecialDeal;
                    pr.ItemPrice = PolicyHistory.ItemPrice;
                    pr.ItemPurchasedDate = PolicyHistory.ItemPurchasedDate;
                    pr.ItemStatusId = PolicyHistory.ItemStatusId;
                    pr.LastName = PolicyHistory.LastName;
                    pr.MakeId = PolicyHistory.MakeId;
                    pr.MobileNo = PolicyHistory.MobileNo;
                    pr.ModelCode = PolicyHistory.ModelCode;
                    pr.ModelId = PolicyHistory.ModelId;
                    pr.ModelYear = PolicyHistory.ModelYear;
                    pr.NationalityId = PolicyHistory.NationalityId;
                    pr.OtherTelNo = PolicyHistory.OtherTelNo;
                    pr.Password = PolicyHistory.Password;
                    pr.PaymentModeId = PolicyHistory.PaymentModeId;
                    pr.PlateNo = PolicyHistory.PlateNo;
                    pr.PolicyNo = PolicyHistory.PolicyNo;
                    pr.PolicySoldDate = PolicyHistory.PolicySoldDate;
                    pr.Premium = PolicyHistory.Premium;
                    pr.PremiumCurrencyTypeId = PolicyHistory.PremiumCurrencyTypeId;
                    pr.ProductId = PolicyHistory.ProductId;
                    pr.ProfilePicture = PolicyHistory.ProfilePicture;
                    pr.RefNo = PolicyHistory.RefNo;
                    pr.SalesPersonId = PolicyHistory.SalesPersonId;
                    pr.SerialNo = PolicyHistory.SerialNo;
                    pr.TransmissionId = PolicyHistory.TransmissionId;
                    pr.UsageTypeId = PolicyHistory.UsageTypeId;
                    pr.UserName = PolicyHistory.UserName;
                    pr.Variant = PolicyHistory.Variant;
                    pr.VehiclePrice = PolicyHistory.VehiclePrice;
                    pr.VINNo = PolicyHistory.VINNo;                 
                    pr.VehicleId = PolicyHistory.VehicleId;
                    pr.CustomerId = PolicyHistory.CustomerId;
                    pr.BAndWId = PolicyHistory.BAndWId;
                    pr.TransactionTypeId = PolicyHistory.TransactionTypeId;
                    pr.CancelationComment = PolicyHistory.CancelationComment;
                    pr.IsRecordActive = PolicyHistory.IsRecordActive;
                    pr.PolicyStartDate = PolicyHistory.PolicyStartDate;
                    pr.PolicyEndDate = PolicyHistory.PolicyEndDate;
                    pr.Discount = PolicyHistory.Discount;
                    pr.ModifiedUser = PolicyHistory.ModifiedUser;
                    pr.PolicyBundleId = PolicyHistory.PolicyBundleId;
                    pr.TransferFee = PolicyHistory.TransferFee;
                    pr.DealerPolicy = PolicyHistory.DealerPolicy;
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
