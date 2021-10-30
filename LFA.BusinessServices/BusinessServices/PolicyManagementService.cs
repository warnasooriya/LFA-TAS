using System;
using System.Collections.Generic;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;

namespace TAS.Services.BusinessServices
{
    internal sealed class PolicyManagementService : IPolicyManagementService
    {
        #region Policy
        public PoliciesResponseDto GetPolicys(SecurityContext securityContext, AuditContext auditContext)
        {
            PoliciesResponseDto result = null;
            PoliciesRetrievalUnitOfWork uow = new PoliciesRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }
        public PoliciesResponseDto GetPolicysByCustomerId(Guid customerid, SecurityContext securityContext, AuditContext auditContext)
        {
            PoliciesResponseDto result = null;
            PoliciesRetrievalByCustomerIdUnitOfWork uow = new PoliciesRetrievalByCustomerIdUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.customerid = customerid;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }
        public PolicyRequestDto AddPolicy(PolicyRequestDto Policy, SecurityContext securityContext, AuditContext auditContext)
        {
            PolicyRequestDto result = new PolicyRequestDto();
            PolicyInsertionUnitOfWork uow = new PolicyInsertionUnitOfWork(Policy);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.PolicyInsertion = uow.Policy.PolicyInsertion;
            return result;
        }
        public PolicyResponseDto GetPolicyById(Guid PolicyId, SecurityContext securityContext, AuditContext auditContext)
        {
            PolicyResponseDto result = new PolicyResponseDto();
            PolicyRetrievalUnitOfWork uow = new PolicyRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.PolicyId = PolicyId;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }
        public PolicyRequestDto UpdatePolicy(PolicyRequestDto Policy, SecurityContext securityContext, AuditContext auditContext)
        {
            #region Approval
            if (Policy.IsApproved)
            {
                CommodityManagementService commodity = new CommodityManagementService();
                string Type = commodity.GetAllCommodities(securityContext, auditContext).Commmodities.Find(c => c.CommodityTypeId == Policy.CommodityTypeId).CommodityTypeDescription;

                PolicyHistoryRequestDto originalHistory = new PolicyHistoryRequestDto();

                PolicyTransactionTypesResponseDto resultType = null;
                PolicyTransactionTypesRetrievalUnitOfWork uowType = new PolicyTransactionTypesRetrievalUnitOfWork();
                uowType.SecurityContext = securityContext;
                uowType.AuditContext = auditContext;
                if (uowType.PreExecute())
                {
                    uowType.Execute();
                }
                resultType = uowType.Result;
                originalHistory.TransactionTypeId = resultType.PolicyTransactionTypes.Find(t => t.Code == "Approval").Id;

                CustomerManagementService custMng = new CustomerManagementService();
                VehicleDetailsManagementService vehicleMng = new VehicleDetailsManagementService();
                BrownAndWhiteDetailsManagementService bAndWMng = new BrownAndWhiteDetailsManagementService();
                VehicleDetailsResponseDto vehicle = new VehicleDetailsResponseDto();
                BrownAndWhiteDetailsResponseDto bAndW = new BrownAndWhiteDetailsResponseDto();

                #region Original to History
                originalHistory.Comment = Policy.Comment;
                originalHistory.CommodityTypeId = Policy.CommodityTypeId;
                originalHistory.ContractId = Policy.ContractId;
                originalHistory.CoverTypeId = Policy.CoverTypeId;
                originalHistory.CustomerId = Policy.CustomerId;
                originalHistory.CustomerPayment = Policy.CustomerPayment;
                originalHistory.CustomerPaymentCurrencyTypeId = Policy.CustomerPaymentCurrencyTypeId;
                originalHistory.DealerId = Policy.DealerId;
                originalHistory.DealerLocationId = Policy.DealerLocationId;
                originalHistory.DealerPayment = Policy.DealerPayment;
                originalHistory.DealerPaymentCurrencyTypeId = Policy.DealerPaymentCurrencyTypeId;
                originalHistory.ExtensionTypeId = Policy.ExtensionTypeId;
                originalHistory.HrsUsedAtPolicySale = Policy.HrsUsedAtPolicySale;
                originalHistory.PolicyId = Policy.Id;
                originalHistory.IsApproved = Policy.IsApproved;
                originalHistory.IsPartialPayment = Policy.IsPartialPayment;
                originalHistory.IsPreWarrantyCheck = Policy.IsPreWarrantyCheck;
                originalHistory.IsSpecialDeal = Policy.IsSpecialDeal;
                originalHistory.PaymentModeId = Policy.PaymentModeId;
                originalHistory.PolicyNo = Policy.PolicyNo;
                originalHistory.PolicySoldDate = Policy.PolicySoldDate;
                originalHistory.Premium = Policy.Premium;
                originalHistory.PremiumCurrencyTypeId = Policy.PremiumCurrencyTypeId;
                originalHistory.ProductId = Policy.ProductId;
                originalHistory.RefNo = Policy.RefNo;
                originalHistory.SalesPersonId = Policy.SalesPersonId;

                #endregion
                CustomerResponseDto customer = custMng.GetCustomers(securityContext, auditContext).Customers.Find(c => c.Id.ToString() == Policy.CustomerId.ToString());
                #region Original to History
                originalHistory.Address1 = customer.Address1;
                originalHistory.Address2 = customer.Address2;
                originalHistory.Address3 = customer.Address3;
                originalHistory.Address4 = customer.Address4;
                originalHistory.BusinessAddress1 = customer.BusinessAddress1;
                originalHistory.BusinessAddress2 = customer.BusinessAddress2;
                originalHistory.BusinessAddress3 = customer.BusinessAddress3;
                originalHistory.BusinessAddress4 = customer.BusinessAddress4;
                originalHistory.BusinessName = customer.BusinessName;
                originalHistory.BusinessTelNo = customer.BusinessTelNo;
                originalHistory.CityId = customer.CityId;
                originalHistory.CountryId = customer.CountryId;
                originalHistory.CustomerTypeId = customer.CustomerTypeId;
                originalHistory.DateOfBirth = customer.DateOfBirth;
                originalHistory.DLIssueDate = customer.DLIssueDate;
                originalHistory.Email = customer.Email;
                originalHistory.FirstName = customer.FirstName;
                originalHistory.Gender = customer.Gender;
                originalHistory.CustomerId = Guid.Parse(customer.Id);
                originalHistory.IDNo = customer.IDNo;
                originalHistory.IDTypeId = customer.IDTypeId;
                originalHistory.LastName = customer.LastName;
                originalHistory.MobileNo = customer.MobileNo;
                originalHistory.NationalityId = customer.NationalityId;
                originalHistory.Password = customer.Password;
                originalHistory.ProfilePicture = customer.ProfilePicture;
                originalHistory.UsageTypeId = customer.UsageTypeId;
                originalHistory.UserName = customer.UserName;
                #endregion
                if (Type == "Automobile")
                {
                    vehicle = vehicleMng.GetVehicleDetailsById(Policy.ItemId, securityContext, auditContext);
                    if (vehicle.IsVehicleDetailsExists)
                    {
                        #region Original to History
                        originalHistory.AspirationId = vehicle.AspirationId;
                    }

                    originalHistory.BodyTypeId = vehicle.BodyTypeId;
                    originalHistory.CategoryId = vehicle.CategoryId;
                    originalHistory.CylinderCountId = vehicle.CylinderCountId;
                    originalHistory.DealerPrice = vehicle.DealerPrice;
                    originalHistory.DriveTypeId = vehicle.DriveTypeId;
                    originalHistory.EngineCapacityId = vehicle.EngineCapacityId;
                    originalHistory.FuelTypeId = vehicle.FuelTypeId;
                    originalHistory.VehicleId = vehicle.Id;
                    originalHistory.ItemPurchasedDate = vehicle.ItemPurchasedDate;
                    originalHistory.ItemStatusId = vehicle.ItemStatusId;
                    originalHistory.MakeId = vehicle.MakeId;
                    originalHistory.ModelId = vehicle.ModelId;
                    originalHistory.ModelYear = vehicle.ModelYear;
                    originalHistory.PlateNo = vehicle.PlateNo;
                    originalHistory.TransmissionId = vehicle.TransmissionId;
                    originalHistory.Variant = vehicle.Variant;
                    originalHistory.VehiclePrice = vehicle.VehiclePrice;
                    originalHistory.VINNo = vehicle.VINNo;
                    #endregion
                }
                else
                {
                    bAndW = bAndWMng.GetBrownAndWhiteDetailsById(Policy.ItemId, securityContext, auditContext);
                    if (bAndW.IsBrownAndWhiteDetailsExists)
                    {
                        #region Original to History
                        originalHistory.AddnSerialNo = bAndW.AddnSerialNo;
                    }

                    originalHistory.CategoryId = bAndW.CategoryId;
                    originalHistory.DealerPrice = bAndW.DealerPrice;
                    originalHistory.BAndWId = bAndW.Id;
                    originalHistory.InvoiceNo = bAndW.InvoiceNo;
                    originalHistory.ItemPrice = bAndW.ItemPrice;
                    originalHistory.ItemPurchasedDate = bAndW.ItemPurchasedDate;
                    originalHistory.ItemStatusId = bAndW.ItemStatusId;
                    originalHistory.MakeId = bAndW.MakeId;
                    originalHistory.ModelCode = bAndW.ModelCode;
                    originalHistory.ModelId = bAndW.ModelId;
                    originalHistory.ModelYear = bAndW.ModelYear;
                    originalHistory.SerialNo = bAndW.SerialNo;
                    #endregion
                }
                //Original policy details saved in history
                PolicyHistoryRequestDto resultO = AddPolicyHistory(originalHistory, securityContext, auditContext);
            }
            #endregion

            #region Renewal
            PolicyResponseDto old = GetPolicyById(Policy.Id, securityContext, auditContext);
            if (old.PolicyStartDate != Policy.PolicyStartDate && old.PolicyEndDate != Policy.PolicyEndDate)
            {
                CommodityManagementService commodity = new CommodityManagementService();
                string Type = commodity.GetAllCommodities(securityContext, auditContext).Commmodities.Find(c => c.CommodityTypeId == Policy.CommodityTypeId).CommodityTypeDescription;

                PolicyHistoryRequestDto originalHistory = new PolicyHistoryRequestDto();

                PolicyTransactionTypesResponseDto resultType = null;
                PolicyTransactionTypesRetrievalUnitOfWork uowType = new PolicyTransactionTypesRetrievalUnitOfWork();
                uowType.SecurityContext = securityContext;
                uowType.AuditContext = auditContext;
                if (uowType.PreExecute())
                {
                    uowType.Execute();
                }
                resultType = uowType.Result;
                originalHistory.TransactionTypeId = resultType.PolicyTransactionTypes.Find(t => t.Code == "Renewed").Id;

                CustomerManagementService custMng = new CustomerManagementService();
                VehicleDetailsManagementService vehicleMng = new VehicleDetailsManagementService();
                BrownAndWhiteDetailsManagementService bAndWMng = new BrownAndWhiteDetailsManagementService();
                VehicleDetailsResponseDto vehicle = new VehicleDetailsResponseDto();
                BrownAndWhiteDetailsResponseDto bAndW = new BrownAndWhiteDetailsResponseDto();

                #region Original to History
                originalHistory.Comment = Policy.Comment;
                originalHistory.CommodityTypeId = Policy.CommodityTypeId;
                originalHistory.ContractId = Policy.ContractId;
                originalHistory.CoverTypeId = Policy.CoverTypeId;
                originalHistory.CustomerId = Policy.CustomerId;
                originalHistory.CustomerPayment = Policy.CustomerPayment;
                originalHistory.CustomerPaymentCurrencyTypeId = Policy.CustomerPaymentCurrencyTypeId;
                originalHistory.DealerId = Policy.DealerId;
                originalHistory.DealerLocationId = Policy.DealerLocationId;
                originalHistory.DealerPayment = Policy.DealerPayment;
                originalHistory.DealerPaymentCurrencyTypeId = Policy.DealerPaymentCurrencyTypeId;
                originalHistory.ExtensionTypeId = Policy.ExtensionTypeId;
                originalHistory.HrsUsedAtPolicySale = Policy.HrsUsedAtPolicySale;
                originalHistory.PolicyId = Policy.Id;
                originalHistory.IsApproved = Policy.IsApproved;
                originalHistory.IsPartialPayment = Policy.IsPartialPayment;
                originalHistory.IsPreWarrantyCheck = Policy.IsPreWarrantyCheck;
                originalHistory.IsSpecialDeal = Policy.IsSpecialDeal;
                originalHistory.PaymentModeId = Policy.PaymentModeId;
                originalHistory.PolicyNo = Policy.PolicyNo;
                originalHistory.PolicySoldDate = Policy.PolicySoldDate;
                originalHistory.Premium = Policy.Premium;
                originalHistory.PremiumCurrencyTypeId = Policy.PremiumCurrencyTypeId;
                originalHistory.ProductId = Policy.ProductId;
                originalHistory.RefNo = Policy.RefNo;
                originalHistory.SalesPersonId = Policy.SalesPersonId;
                #endregion
                CustomerResponseDto customer = custMng.GetCustomers(securityContext, auditContext).Customers.Find(c => c.Id == Policy.CustomerId.ToString());
                #region Original to History
                originalHistory.Address1 = customer.Address1;
                originalHistory.Address2 = customer.Address2;
                originalHistory.Address3 = customer.Address3;
                originalHistory.Address4 = customer.Address4;
                originalHistory.BusinessAddress1 = customer.BusinessAddress1;
                originalHistory.BusinessAddress2 = customer.BusinessAddress2;
                originalHistory.BusinessAddress3 = customer.BusinessAddress3;
                originalHistory.BusinessAddress4 = customer.BusinessAddress4;
                originalHistory.BusinessName = customer.BusinessName;
                originalHistory.BusinessTelNo = customer.BusinessTelNo;
                originalHistory.CityId = customer.CityId;
                originalHistory.CountryId = customer.CountryId;
                originalHistory.CustomerTypeId = customer.CustomerTypeId;
                originalHistory.DateOfBirth = customer.DateOfBirth;
                originalHistory.DLIssueDate = customer.DLIssueDate;
                originalHistory.Email = customer.Email;
                originalHistory.FirstName = customer.FirstName;
                originalHistory.Gender = customer.Gender;
                originalHistory.CustomerId = Guid.Parse(customer.Id);
                originalHistory.IDNo = customer.IDNo;
                originalHistory.IDTypeId = customer.IDTypeId;
                originalHistory.LastName = customer.LastName;
                originalHistory.MobileNo = customer.MobileNo;
                originalHistory.NationalityId = customer.NationalityId;
                originalHistory.Password = customer.Password;
                originalHistory.ProfilePicture = customer.ProfilePicture;
                originalHistory.UsageTypeId = customer.UsageTypeId;
                originalHistory.UserName = customer.UserName;
                #endregion
                if (Type == "Automobile")
                {
                    vehicle = vehicleMng.GetVehicleDetailsById(Policy.ItemId, securityContext, auditContext);
                    if (vehicle.IsVehicleDetailsExists)
                    {
                        #region Original to History
                        originalHistory.AspirationId = vehicle.AspirationId;
                    }

                    originalHistory.BodyTypeId = vehicle.BodyTypeId;
                    originalHistory.CategoryId = vehicle.CategoryId;
                    originalHistory.CylinderCountId = vehicle.CylinderCountId;
                    originalHistory.DealerPrice = vehicle.DealerPrice;
                    originalHistory.DriveTypeId = vehicle.DriveTypeId;
                    originalHistory.EngineCapacityId = vehicle.EngineCapacityId;
                    originalHistory.FuelTypeId = vehicle.FuelTypeId;
                    originalHistory.VehicleId = vehicle.Id;
                    originalHistory.ItemPurchasedDate = vehicle.ItemPurchasedDate;
                    originalHistory.ItemStatusId = vehicle.ItemStatusId;
                    originalHistory.MakeId = vehicle.MakeId;
                    originalHistory.ModelId = vehicle.ModelId;
                    originalHistory.ModelYear = vehicle.ModelYear;
                    originalHistory.PlateNo = vehicle.PlateNo;
                    originalHistory.TransmissionId = vehicle.TransmissionId;
                    originalHistory.Variant = vehicle.Variant;
                    originalHistory.VehiclePrice = vehicle.VehiclePrice;
                    originalHistory.VINNo = vehicle.VINNo;
                    #endregion
                }
                else
                {
                    bAndW = bAndWMng.GetBrownAndWhiteDetailsById(Policy.ItemId, securityContext, auditContext);
                    if (bAndW.IsBrownAndWhiteDetailsExists)
                    {
                        #region Original to History
                        originalHistory.AddnSerialNo = bAndW.AddnSerialNo;
                    }

                    originalHistory.CategoryId = bAndW.CategoryId;
                    originalHistory.DealerPrice = bAndW.DealerPrice;
                    originalHistory.BAndWId = bAndW.Id;
                    originalHistory.InvoiceNo = bAndW.InvoiceNo;
                    originalHistory.ItemPrice = bAndW.ItemPrice;
                    originalHistory.ItemPurchasedDate = bAndW.ItemPurchasedDate;
                    originalHistory.ItemStatusId = bAndW.ItemStatusId;
                    originalHistory.MakeId = bAndW.MakeId;
                    originalHistory.ModelCode = bAndW.ModelCode;
                    originalHistory.ModelId = bAndW.ModelId;
                    originalHistory.ModelYear = bAndW.ModelYear;
                    originalHistory.SerialNo = bAndW.SerialNo;
                    #endregion
                }
                //Original policy details saved in history
                PolicyHistoryRequestDto resultO = AddPolicyHistory(originalHistory, securityContext, auditContext);
            }
            #endregion

            PolicyRequestDto result = new PolicyRequestDto();
            PolicyUpdationUnitOfWork uow = new PolicyUpdationUnitOfWork(Policy);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result.PolicyInsertion = uow.Policy.PolicyInsertion;
            return result;
        }
        public OnlinePurchaseRequestDto SaveOnlinePurchase(OnlinePurchaseRequestDto Policy, SecurityContext securityContext, AuditContext auditContext)
        {
            OnlinePurchaseRequestDto result = new OnlinePurchaseRequestDto();
            OnlinePurchaseInsertionUnitOfWork uow = new OnlinePurchaseInsertionUnitOfWork(Policy);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.PolicyInsertion = uow.Policy.PolicyInsertion;
            return result;
        }
        public string SavePolicy(SavePolicyRequestDto SavePolicyRequest, SecurityContext securityContext, AuditContext auditContext)
        {
            string result = null;
            PolicyInsertionUnitOfWorkV2 uow = new PolicyInsertionUnitOfWorkV2(SavePolicyRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }
        public SerialNumberCheckResponseDto SerialNumberCheck(string SerialNumber, string CommodityCode, Guid LoggedInUserId, Guid DealerId, SecurityContext securityContext, AuditContext auditContext)
        {
            SerialNumberCheckResponseDto Response = new SerialNumberCheckResponseDto();
            SerialNumberCheckUnitOfWork uow = new SerialNumberCheckUnitOfWork(SerialNumber, CommodityCode, LoggedInUserId, DealerId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            Response = uow.Result;
            return Response;
        }
        public string UpdatePolicyV2(SavePolicyRequestDto SavePolicyRequest, SecurityContext securityContext, AuditContext auditContext)
        {
            string result = null;
            PolicyUpdationUnitOfWorkV2 uow = new PolicyUpdationUnitOfWorkV2(SavePolicyRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public object GetPoliciesByBordxIdForGrid(GetPoliciesByBordxIdRequestDto GetPoliciesByBordxIdRequestDto, SecurityContext securityContext, AuditContext auditContext)
        {
            PloicyRetrievalByBordxIdGirdUnitOfWork uow = new PloicyRetrievalByBordxIdGirdUnitOfWork(GetPoliciesByBordxIdRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;

        }
        public object GetPoliciesByBordxIdForViewGrid(BordxViewGridRequestDto BordxViewGridRequestDto, SecurityContext securityContext, AuditContext auditContext)
        {
            PloicyRetrievalByBordxIdViewUnitOfWork uow = new PloicyRetrievalByBordxIdViewUnitOfWork(BordxViewGridRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }
        public BordxExportResponseDto ExportPoliciesToExcelByBordxId(ExportPoliciesToExcelByBordxIdRequestDto ExportPoliciesToExcelByBordxIdRequestDto, SecurityContext securityContext, AuditContext auditContext)
        {
            PloicyRetrievalByBordxIdExportUnitOfWork uow = new PloicyRetrievalByBordxIdExportUnitOfWork(ExportPoliciesToExcelByBordxIdRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetPoliciesForSearchGrid(PolicySearchGridRequestDto PolicySearchGridRequestDto, SecurityContext securityContext, AuditContext auditContext)
        {
            PloicyRetrievalForSearchGrid uow = new PloicyRetrievalForSearchGrid(PolicySearchGridRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetPoliciesForSearchGridReneval(PolicySearchGridRequestDto PolicySearchGridRequestDto, SecurityContext securityContext, AuditContext auditContext)
        {
            PloicyRetrievalForSearchGridReneval uow = new PloicyRetrievalForSearchGridReneval(PolicySearchGridRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetPoliciesForSearchGridInquiry(PolicySearchInquiryGridRequestDto PolicySearchInquiryGridRequestDto, SecurityContext securityContext, AuditContext auditContext)
        {
            PloicyRetrievalForSearchGridInquiry uow = new PloicyRetrievalForSearchGridInquiry(PolicySearchInquiryGridRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public AttachmentsResponseDto GetAttachmentsByPolicyId(Guid policyId, SecurityContext securityContext, AuditContext auditContext)
        {
            AttachmentsRetrivalByPolicyIdUnitOfWork uow = new AttachmentsRetrivalByPolicyIdUnitOfWork(policyId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetClaimHistorysByPolicyId(Guid policyId, SecurityContext securityContext, AuditContext auditContext)
        {
            ClaimHistoryByPolicyIdUnitOfWork uow = new ClaimHistoryByPolicyIdUnitOfWork(policyId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public EligibilityCheckResponse EligibilityCheckRequest(EligibilityCheckRequest eligibilityCheckRequest, SecurityContext securityContext, AuditContext auditContext)
        {
            EligibilityCheckUnitOfWork uow = new EligibilityCheckUnitOfWork(eligibilityCheckRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetDealerAccessByUserId(Guid LoggedInUserId, SecurityContext securityContext, AuditContext auditContext)
        {
            DealerAccessRetrivalByUserIdUnitOfWork uow = new DealerAccessRetrivalByUserIdUnitOfWork(LoggedInUserId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetPremiumBreakdown(PremiumBreakdownRequestDto premiumBreakdownRequest, SecurityContext securityContext, AuditContext auditContext)
        {
            GetPremiumBreakdownUnitOfWork uow = new GetPremiumBreakdownUnitOfWork(premiumBreakdownRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }


        #endregion

        #region Bundle
        public PolicyBundlesResponseDto GetPolicyBundles(SecurityContext securityContext, AuditContext auditContext)
        {
            PolicyBundlesResponseDto result = null;
            PolicyBundlesRetrievalUnitOfWork uow = new PolicyBundlesRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }
        public PolicyBundleRequestDto AddPolicyBundle(PolicyBundleRequestDto Policy, SecurityContext securityContext, AuditContext auditContext)
        {
            PolicyBundleRequestDto result = new PolicyBundleRequestDto();
            PolicyBundleInsertionUnitOfWork uow = new PolicyBundleInsertionUnitOfWork(Policy);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Policy;
            return result;
        }
        public PolicyBundleResponseDto GetPolicyBundleById(Guid PolicyId, SecurityContext securityContext, AuditContext auditContext)
        {
            PolicyBundleResponseDto result = new PolicyBundleResponseDto();
            PolicyBundleRetrievalUnitOfWork uow = new PolicyBundleRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.PolicyId = PolicyId;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }
        public PolicyBundleRequestDto UpdatePolicyBundle(PolicyBundleRequestDto Policy, SecurityContext securityContext, AuditContext auditContext)
        {
            PolicyBundleRequestDto result = new PolicyBundleRequestDto();
            PolicyBundleUpdationUnitOfWork uow = new PolicyBundleUpdationUnitOfWork(Policy);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Policy;
            return result;
        }
        #endregion

        #region Bundle History
        public object GetPolicyTransferHistoryById(Guid policyBundleId, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = null;
            GetPolicyTransferHistoryByIdUnitOfWork uow = new GetPolicyTransferHistoryByIdUnitOfWork(policyBundleId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            Response = uow.Result;
            return Response;
        }

        public PolicyBundleHistoriesResponseDto GetPolicyBundleHistories(SecurityContext securityContext, AuditContext auditContext)
        {
            PolicyBundleHistoriesResponseDto result = null;
            PolicyBundleHistoriesRetrievalUnitOfWork uow = new PolicyBundleHistoriesRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }
        public PolicyBundleHistoryRequestDto AddPolicyBundleHistory(PolicyBundleHistoryRequestDto Policy, SecurityContext securityContext, AuditContext auditContext)
        {
            PolicyBundleHistoryRequestDto result = new PolicyBundleHistoryRequestDto();
            PolicyBundleHistoryInsertionUnitOfWork uow = new PolicyBundleHistoryInsertionUnitOfWork(Policy);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Policy;
            return result;
        }
        #endregion

        #region Bundle Transaction
        public PolicyBundleTransactionsResponseDto GetPolicyBundleTransactions(SecurityContext securityContext, AuditContext auditContext)
        {
            PolicyBundleTransactionsResponseDto result = null;
            PolicyBundleTransactionsRetrievalUnitOfWork uow = new PolicyBundleTransactionsRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }
        public PolicyBundleTransactionResponseDto GetPolicyBundleTransactionById(Guid PolicyId, SecurityContext securityContext, AuditContext auditContext)
        {
            PolicyBundleTransactionResponseDto result = new PolicyBundleTransactionResponseDto();
            PolicyBundleTransactionRetrievalUnitOfWork uow = new PolicyBundleTransactionRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.PolicyId = PolicyId;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }
        public PolicyBundleTransactionRequestDto AddPolicyBundleTransaction(PolicyBundleTransactionRequestDto Policy, SecurityContext securityContext, AuditContext auditContext)
        {
            PolicyTransactionTypesResponseDto resultType = null;
            PolicyTransactionTypesRetrievalUnitOfWork uowType = new PolicyTransactionTypesRetrievalUnitOfWork();
            uowType.SecurityContext = securityContext;
            uowType.AuditContext = auditContext;
            if (uowType.PreExecute())
            {
                uowType.Execute();
            }
            resultType = uowType.Result;
            Policy.TransactionTypeId = resultType.PolicyTransactionTypes.Find(t => t.Code == "Endorsement").Id;
            PolicyBundleTransactionRequestDto result = new PolicyBundleTransactionRequestDto();
            PolicyBundleTransactionInsertionUnitOfWork uow = new PolicyBundleTransactionInsertionUnitOfWork(Policy);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Policy;
            return result;
        }
        #endregion

        #region Endorsement
        public string EndorsePolicy(SavePolicyRequestDto SavePolicyRequest, SecurityContext securityContext, AuditContext auditContext)
        {
            String Resopnse = "Error";
            PolicyEndorsementUnitOfWork uow = new PolicyEndorsementUnitOfWork(SavePolicyRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Resopnse = uow.Result;
            }
            return Resopnse;
        }

        public object GetAllPolicyInquiryDetails(Guid BundlePolicyId, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = new object();
            GetAllPolicyInquiryDetailsUnitOfWork uow = new GetAllPolicyInquiryDetailsUnitOfWork(BundlePolicyId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }

            return Response;
        }

        public object GetPolicyById2(Guid PolicyId, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = new object();
            GetAllPolicyDetailsByIdUnitOfWork uow = new GetAllPolicyDetailsByIdUnitOfWork(PolicyId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }

            return Response;
        }
        public object GetAllPolicyEndorsementDetailsForApproval(Guid BundlePolicyId, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = new object();
            GetAllPolicyEndorsementDetailsUnitOfWork uow = new GetAllPolicyEndorsementDetailsUnitOfWork(BundlePolicyId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }

            return Response;
        }
        public PolicyTransactionsResponseDto GetPolicyEndorsements(SecurityContext securityContext, AuditContext auditContext)
        {
            PolicyTransactionsResponseDto result = null;
            PolicyTransactionsRetrievalUnitOfWork uow = new PolicyTransactionsRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.Execute();
            result = uow.Result;
            return result;
        }
        public PolicyTransactionRequestDto AddPolicyEndorsement(PolicyTransactionRequestDto Policy, SecurityContext securityContext, AuditContext auditContext)
        {
            PolicyTransactionTypesResponseDto resultType = null;
            PolicyTransactionTypesRetrievalUnitOfWork uowType = new PolicyTransactionTypesRetrievalUnitOfWork();
            uowType.SecurityContext = securityContext;
            uowType.AuditContext = auditContext;
            uowType.Execute();
            resultType = uowType.Result;
            Policy.TransactionTypeId = resultType.PolicyTransactionTypes.Find(t => t.Code == "Endorsement").Id;
            PolicyTransactionRequestDto result = new PolicyTransactionRequestDto();
            PolicyTransactionInsertionUnitOfWork uow = new PolicyTransactionInsertionUnitOfWork(Policy);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.Execute();
            result.PolicyInsertion = uow.PolicyEndorsement.PolicyInsertion;
            return result;
        }
        public PolicyTransactionResponseDto GetPolicyEndorsementById(Guid PolicyId, SecurityContext securityContext, AuditContext auditContext)
        {
            PolicyTransactionResponseDto result = new PolicyTransactionResponseDto();
            PolicyTransactionRetrievalUnitOfWork uow = new PolicyTransactionRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.PolicyEndorsementId = PolicyId;
            uow.Execute();
            result = uow.Result;
            return result;
        }

        public string ApproveEndorsement(Guid PolicyBundleId, Guid UserId, SecurityContext securityContext, AuditContext auditContext)
        {
            string Response = "Error Occured.";
            EndorsementApprovalUnitOfWork uow = new EndorsementApprovalUnitOfWork(PolicyBundleId, UserId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public string RejectEndorsement(Guid PolicyBundleId, Guid UserId, SecurityContext securityContext, AuditContext auditContext)
        {
            string Response = "Error Occured.";
            EndorsementRejectionUnitOfWork uow = new EndorsementRejectionUnitOfWork(PolicyBundleId, UserId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }
        #endregion

        #region Endorsement Approval
        public bool ApprovePolicyEndorsement(Guid Id, bool Status, SecurityContext securityContext, AuditContext auditContext)
        {
            try
            {
                //Get Endorsement
                PolicyBundleTransactionResponseDto endorsement = GetPolicyBundleTransactionById(Id, securityContext, auditContext);
                List<PolicyTransactionResponseDto> endorsementBundle = GetPolicyEndorsements(securityContext, auditContext).Policies.FindAll(e => e.PolicyBundleId == Id);
                if (Status)
                {
                    CommodityManagementService commodity = new CommodityManagementService();
                    string Type = commodity.GetAllCommodities(securityContext, auditContext).Commmodities.Find(c => c.CommodityTypeId == endorsement.CommodityTypeId).CommodityTypeDescription;
                    Guid ItemId = new Guid();

                    CustomerManagementService custMng = new CustomerManagementService();
                    VehicleDetailsManagementService vehicleMng = new VehicleDetailsManagementService();
                    BrownAndWhiteDetailsManagementService bAndWMng = new BrownAndWhiteDetailsManagementService();

                    VehicleDetailsResponseDto vehicle = new VehicleDetailsResponseDto();
                    BrownAndWhiteDetailsResponseDto bAndW = new BrownAndWhiteDetailsResponseDto();

                    //Get Original Policy
                    PolicyBundleResponseDto policy = GetPolicyBundleById(endorsement.PolicyId, securityContext, auditContext);
                    List<PolicyResponseDto> policyBundle = GetPolicys(securityContext, auditContext).Policies.FindAll(p => p.PolicyBundleId == endorsement.PolicyId);
                    CustomerResponseDto customer = custMng.GetCustomers(securityContext, auditContext).Customers.Find(c => c.Id == policy.CustomerId.ToString());
                    if (Type == "Automobile")
                    {
                        vehicle = vehicleMng.GetVehicleDetailsById(policy.ItemId, securityContext, auditContext);
                        ItemId = vehicle.Id;
                        Type = "Vehicle";
                    }
                    else
                    {
                        bAndW = bAndWMng.GetBrownAndWhiteDetailsById(policy.ItemId, securityContext, auditContext);
                        ItemId = bAndW.Id;
                        Type = "B&W";
                    }

                    //Original policy details saved in history
                    PolicyBundleHistoryRequestDto originalHistory = new PolicyBundleHistoryRequestDto();
                    #region Original to History
                    originalHistory.Comment = policy.Comment;
                    originalHistory.CommodityTypeId = policy.CommodityTypeId;
                    originalHistory.CustomerId = policy.CustomerId;
                    originalHistory.CustomerPayment = policy.CustomerPayment;
                    originalHistory.CustomerPaymentCurrencyTypeId = policy.CustomerPaymentCurrencyTypeId;
                    originalHistory.DealerId = policy.DealerId;
                    originalHistory.DealerLocationId = policy.DealerLocationId;
                    originalHistory.DealerPayment = policy.DealerPayment;
                    originalHistory.DealerPaymentCurrencyTypeId = policy.DealerPaymentCurrencyTypeId;
                    originalHistory.HrsUsedAtPolicySale = policy.HrsUsedAtPolicySale;
                    originalHistory.PolicyId = policy.Id;
                    originalHistory.IsApproved = policy.IsApproved;
                    originalHistory.IsPartialPayment = policy.IsPartialPayment;
                    originalHistory.IsPreWarrantyCheck = policy.IsPreWarrantyCheck;
                    originalHistory.IsSpecialDeal = policy.IsSpecialDeal;
                    originalHistory.PaymentModeId = policy.PaymentModeId;
                    originalHistory.PolicySoldDate = policy.PolicySoldDate;
                    originalHistory.ProductId = policy.ProductId;
                    originalHistory.RefNo = policy.RefNo;
                    originalHistory.SalesPersonId = policy.SalesPersonId;
                    #endregion
                    PolicyBundleHistoryRequestDto resultO = AddPolicyBundleHistory(originalHistory, securityContext, auditContext);
                    #region Original Bundle to History
                    foreach (var item in policyBundle)
                    {
                        PolicyHistoryRequestDto originalHistoryBundle = new PolicyHistoryRequestDto();
                        originalHistoryBundle.Comment = policy.Comment;
                        originalHistoryBundle.CommodityTypeId = policy.CommodityTypeId;
                        originalHistoryBundle.CustomerId = policy.CustomerId;
                        originalHistoryBundle.CustomerPayment = policy.CustomerPayment;
                        originalHistoryBundle.CustomerPaymentCurrencyTypeId = policy.CustomerPaymentCurrencyTypeId;
                        originalHistoryBundle.DealerId = policy.DealerId;
                        originalHistoryBundle.DealerLocationId = policy.DealerLocationId;
                        originalHistoryBundle.DealerPayment = policy.DealerPayment;
                        originalHistoryBundle.DealerPaymentCurrencyTypeId = policy.DealerPaymentCurrencyTypeId;
                        originalHistoryBundle.HrsUsedAtPolicySale = policy.HrsUsedAtPolicySale;
                        originalHistoryBundle.PolicyId = policy.Id;
                        originalHistoryBundle.IsApproved = policy.IsApproved;
                        originalHistoryBundle.IsPartialPayment = policy.IsPartialPayment;
                        originalHistoryBundle.IsPreWarrantyCheck = policy.IsPreWarrantyCheck;
                        originalHistoryBundle.IsSpecialDeal = policy.IsSpecialDeal;
                        originalHistoryBundle.PaymentModeId = policy.PaymentModeId;
                        originalHistoryBundle.PolicySoldDate = policy.PolicySoldDate;
                        originalHistoryBundle.ProductId = policy.ProductId;
                        originalHistoryBundle.RefNo = policy.RefNo;
                        originalHistoryBundle.SalesPersonId = policy.SalesPersonId;

                        originalHistoryBundle.Address1 = customer.Address1;
                        originalHistoryBundle.Address2 = customer.Address2;
                        originalHistoryBundle.Address3 = customer.Address3;
                        originalHistoryBundle.Address4 = customer.Address4;
                        originalHistoryBundle.BusinessAddress1 = customer.BusinessAddress1;
                        originalHistoryBundle.BusinessAddress2 = customer.BusinessAddress2;
                        originalHistoryBundle.BusinessAddress3 = customer.BusinessAddress3;
                        originalHistoryBundle.BusinessAddress4 = customer.BusinessAddress4;
                        originalHistoryBundle.BusinessName = customer.BusinessName;
                        originalHistoryBundle.BusinessTelNo = customer.BusinessTelNo;
                        originalHistoryBundle.CityId = customer.CityId;
                        originalHistoryBundle.CountryId = customer.CountryId;
                        originalHistoryBundle.CustomerTypeId = customer.CustomerTypeId;
                        originalHistoryBundle.DateOfBirth = customer.DateOfBirth;
                        originalHistoryBundle.DLIssueDate = customer.DLIssueDate;
                        originalHistoryBundle.Email = customer.Email;
                        originalHistoryBundle.FirstName = customer.FirstName;
                        originalHistoryBundle.Gender = customer.Gender;
                        originalHistoryBundle.CustomerId = Guid.Parse(customer.Id);
                        originalHistoryBundle.IDNo = customer.IDNo;
                        originalHistoryBundle.IDTypeId = customer.IDTypeId;
                        originalHistoryBundle.LastName = customer.LastName;
                        originalHistoryBundle.MobileNo = customer.MobileNo;
                        originalHistoryBundle.NationalityId = customer.NationalityId;
                        originalHistoryBundle.Password = customer.Password;
                        originalHistoryBundle.ProfilePicture = customer.ProfilePicture;
                        originalHistoryBundle.UsageTypeId = customer.UsageTypeId;
                        originalHistoryBundle.UserName = customer.UserName;
                        if (Type == "Vehicle")
                        {
                            originalHistoryBundle.AspirationId = vehicle.AspirationId;
                            originalHistoryBundle.BodyTypeId = vehicle.BodyTypeId;
                            originalHistoryBundle.CategoryId = vehicle.CategoryId;
                            originalHistoryBundle.CylinderCountId = vehicle.CylinderCountId;
                            originalHistoryBundle.DealerPrice = vehicle.DealerPrice;
                            originalHistoryBundle.DriveTypeId = vehicle.DriveTypeId;
                            originalHistoryBundle.EngineCapacityId = vehicle.EngineCapacityId;
                            originalHistoryBundle.FuelTypeId = vehicle.FuelTypeId;
                            originalHistoryBundle.VehicleId = vehicle.Id;
                            originalHistoryBundle.ItemPurchasedDate = vehicle.ItemPurchasedDate;
                            originalHistoryBundle.ItemStatusId = vehicle.ItemStatusId;
                            originalHistoryBundle.MakeId = vehicle.MakeId;
                            originalHistoryBundle.ModelId = vehicle.ModelId;
                            originalHistoryBundle.ModelYear = vehicle.ModelYear;
                            originalHistoryBundle.PlateNo = vehicle.PlateNo;
                            originalHistoryBundle.TransmissionId = vehicle.TransmissionId;
                            originalHistoryBundle.Variant = vehicle.Variant;
                            originalHistoryBundle.VehiclePrice = vehicle.VehiclePrice;
                            originalHistoryBundle.VINNo = vehicle.VINNo;
                            ItemId = vehicle.Id;
                        }
                        else
                        {
                            originalHistoryBundle.AddnSerialNo = bAndW.AddnSerialNo;
                            originalHistoryBundle.CategoryId = bAndW.CategoryId;
                            originalHistoryBundle.DealerPrice = bAndW.DealerPrice;
                            originalHistoryBundle.BAndWId = bAndW.Id;
                            originalHistoryBundle.InvoiceNo = bAndW.InvoiceNo;
                            originalHistoryBundle.ItemPrice = bAndW.ItemPrice;
                            originalHistoryBundle.ItemPurchasedDate = bAndW.ItemPurchasedDate;
                            originalHistoryBundle.ItemStatusId = bAndW.ItemStatusId;
                            originalHistoryBundle.MakeId = bAndW.MakeId;
                            originalHistoryBundle.ModelCode = bAndW.ModelCode;
                            originalHistoryBundle.ModelId = bAndW.ModelId;
                            originalHistoryBundle.ModelYear = bAndW.ModelYear;
                            originalHistoryBundle.SerialNo = bAndW.SerialNo;
                            ItemId = bAndW.Id;
                        }
                        PolicyHistoryRequestDto resultOB = AddPolicyHistory(originalHistoryBundle, securityContext, auditContext);
                    }
                    #endregion

                    //Endorsement policy details saved in history
                    PolicyBundleHistoryRequestDto endorsmentHitsory = new PolicyBundleHistoryRequestDto();
                    #region Endorsement to History
                    endorsmentHitsory.Comment = endorsement.Comment;
                    endorsmentHitsory.CommodityTypeId = endorsement.CommodityTypeId;
                    endorsmentHitsory.ContractId = endorsement.ContractId;
                    endorsmentHitsory.CoverTypeId = endorsement.CoverTypeId;
                    endorsmentHitsory.CustomerId = endorsement.CustomerId;
                    endorsmentHitsory.CustomerPayment = endorsement.CustomerPayment;
                    endorsmentHitsory.CustomerPaymentCurrencyTypeId = endorsement.CustomerPaymentCurrencyTypeId;
                    endorsmentHitsory.DealerId = endorsement.DealerId;
                    endorsmentHitsory.DealerLocationId = endorsement.DealerLocationId;
                    endorsmentHitsory.DealerPayment = endorsement.DealerPayment;
                    endorsmentHitsory.DealerPaymentCurrencyTypeId = endorsement.DealerPaymentCurrencyTypeId;
                    endorsmentHitsory.ExtensionTypeId = endorsement.ExtensionTypeId;
                    endorsmentHitsory.HrsUsedAtPolicySale = endorsement.HrsUsedAtPolicySale;
                    endorsmentHitsory.PolicyId = endorsement.Id;
                    endorsmentHitsory.IsApproved = endorsement.IsApproved;
                    endorsmentHitsory.IsPartialPayment = endorsement.IsPartialPayment;
                    endorsmentHitsory.IsPreWarrantyCheck = endorsement.IsPreWarrantyCheck;
                    endorsmentHitsory.IsSpecialDeal = endorsement.IsSpecialDeal;
                    endorsmentHitsory.PaymentModeId = endorsement.PaymentModeId;
                    endorsmentHitsory.PolicyNo = endorsement.PolicyNo;
                    endorsmentHitsory.PolicySoldDate = endorsement.PolicySoldDate;
                    endorsmentHitsory.Premium = endorsement.Premium;
                    endorsmentHitsory.PremiumCurrencyTypeId = endorsement.PremiumCurrencyTypeId;
                    endorsmentHitsory.ProductId = endorsement.ProductId;
                    endorsmentHitsory.RefNo = endorsement.RefNo;
                    endorsmentHitsory.SalesPersonId = endorsement.SalesPersonId;
                    endorsmentHitsory.CustomerId = endorsement.Id;
                    #endregion
                    PolicyBundleHistoryRequestDto resultE = AddPolicyBundleHistory(endorsmentHitsory, securityContext, auditContext);
                    #region Endorsement Bundle to History
                    foreach (var item in endorsementBundle)
                    {
                        PolicyHistoryRequestDto endorsmentHitsoryBundle = new PolicyHistoryRequestDto();
                        endorsmentHitsoryBundle.Comment = item.Comment;
                        endorsmentHitsoryBundle.CommodityTypeId = item.CommodityTypeId;
                        endorsmentHitsoryBundle.ContractId = item.ContractId;
                        endorsmentHitsoryBundle.CoverTypeId = item.CoverTypeId;
                        endorsmentHitsoryBundle.CustomerId = item.CustomerId;
                        endorsmentHitsoryBundle.CustomerPayment = item.CustomerPayment;
                        endorsmentHitsoryBundle.CustomerPaymentCurrencyTypeId = item.CustomerPaymentCurrencyTypeId;
                        endorsmentHitsoryBundle.DealerId = item.DealerId;
                        endorsmentHitsoryBundle.DealerLocationId = item.DealerLocationId;
                        endorsmentHitsoryBundle.DealerPayment = item.DealerPayment;
                        endorsmentHitsoryBundle.DealerPaymentCurrencyTypeId = item.DealerPaymentCurrencyTypeId;
                        endorsmentHitsoryBundle.ExtensionTypeId = item.ExtensionTypeId;
                        endorsmentHitsoryBundle.HrsUsedAtPolicySale = item.HrsUsedAtPolicySale;
                        endorsmentHitsoryBundle.PolicyId = item.Id;
                        endorsmentHitsoryBundle.IsApproved = item.IsApproved;
                        endorsmentHitsoryBundle.IsPartialPayment = item.IsPartialPayment;
                        endorsmentHitsoryBundle.IsPreWarrantyCheck = item.IsPreWarrantyCheck;
                        endorsmentHitsoryBundle.IsSpecialDeal = item.IsSpecialDeal;
                        endorsmentHitsoryBundle.PaymentModeId = item.PaymentModeId;
                        endorsmentHitsoryBundle.PolicyNo = item.PolicyNo;
                        endorsmentHitsoryBundle.PolicySoldDate = item.PolicySoldDate;
                        endorsmentHitsoryBundle.Premium = item.Premium;
                        endorsmentHitsoryBundle.PremiumCurrencyTypeId = item.PremiumCurrencyTypeId;
                        endorsmentHitsoryBundle.ProductId = item.ProductId;
                        endorsmentHitsoryBundle.RefNo = item.RefNo;
                        endorsmentHitsoryBundle.SalesPersonId = item.SalesPersonId;
                        endorsmentHitsoryBundle.IsRecordActive = item.IsRecordActive;
                        endorsmentHitsoryBundle.Address1 = item.Address1;
                        endorsmentHitsoryBundle.Address2 = item.Address2;
                        endorsmentHitsoryBundle.Address3 = item.Address3;
                        endorsmentHitsoryBundle.Address4 = item.Address4;
                        endorsmentHitsoryBundle.BusinessAddress1 = item.BusinessAddress1;
                        endorsmentHitsoryBundle.BusinessAddress2 = item.BusinessAddress2;
                        endorsmentHitsoryBundle.BusinessAddress3 = item.BusinessAddress3;
                        endorsmentHitsoryBundle.BusinessAddress4 = item.BusinessAddress4;
                        endorsmentHitsoryBundle.BusinessName = item.BusinessName;
                        endorsmentHitsoryBundle.BusinessTelNo = item.BusinessTelNo;
                        endorsmentHitsoryBundle.CityId = item.CityId;
                        endorsmentHitsoryBundle.CountryId = item.CountryId;
                        endorsmentHitsoryBundle.CustomerTypeId = item.CustomerTypeId;
                        endorsmentHitsoryBundle.DateOfBirth = item.DateOfBirth;
                        endorsmentHitsoryBundle.DLIssueDate = item.DLIssueDate;
                        endorsmentHitsoryBundle.Email = item.Email;
                        endorsmentHitsoryBundle.FirstName = item.FirstName;
                        endorsmentHitsoryBundle.Gender = item.Gender;
                        endorsmentHitsoryBundle.CustomerId = item.Id;
                        endorsmentHitsoryBundle.IDNo = item.IDNo;
                        endorsmentHitsoryBundle.IDTypeId = item.IDTypeId;
                        endorsmentHitsoryBundle.LastName = item.LastName;
                        endorsmentHitsoryBundle.MobileNo = item.MobileNo;
                        endorsmentHitsoryBundle.NationalityId = item.NationalityId;
                        endorsmentHitsoryBundle.Password = item.Password;
                        endorsmentHitsoryBundle.ProfilePicture = item.ProfilePicture;
                        endorsmentHitsoryBundle.UsageTypeId = item.UsageTypeId;
                        endorsmentHitsoryBundle.UserName = item.UserName;
                        endorsmentHitsoryBundle.AspirationId = item.AspirationId;
                        endorsmentHitsoryBundle.BodyTypeId = item.BodyTypeId;
                        endorsmentHitsoryBundle.CategoryId = item.CategoryId;
                        endorsmentHitsoryBundle.CylinderCountId = item.CylinderCountId;
                        endorsmentHitsoryBundle.DealerPrice = item.DealerPrice;
                        endorsmentHitsoryBundle.DriveTypeId = item.DriveTypeId;
                        endorsmentHitsoryBundle.EngineCapacityId = item.EngineCapacityId;
                        endorsmentHitsoryBundle.FuelTypeId = item.FuelTypeId;
                        endorsmentHitsoryBundle.VehicleId = item.Id;
                        endorsmentHitsoryBundle.ItemPurchasedDate = item.ItemPurchasedDate;
                        endorsmentHitsoryBundle.ItemStatusId = item.ItemStatusId;
                        endorsmentHitsoryBundle.MakeId = item.MakeId;
                        endorsmentHitsoryBundle.ModelId = item.ModelId;
                        endorsmentHitsoryBundle.ModelYear = item.ModelYear;
                        endorsmentHitsoryBundle.PlateNo = item.PlateNo;
                        endorsmentHitsoryBundle.TransmissionId = item.TransmissionId;
                        endorsmentHitsoryBundle.Variant = item.Variant;
                        endorsmentHitsoryBundle.VehiclePrice = item.VehiclePrice;
                        endorsmentHitsoryBundle.VINNo = item.VINNo;
                        endorsmentHitsoryBundle.AddnSerialNo = item.AddnSerialNo;
                        endorsmentHitsoryBundle.BAndWId = item.Id;
                        endorsmentHitsoryBundle.InvoiceNo = item.InvoiceNo;
                        endorsmentHitsoryBundle.ItemPrice = item.ItemPrice;
                        endorsmentHitsoryBundle.ModelCode = item.ModelCode;
                        endorsmentHitsoryBundle.SerialNo = item.SerialNo;
                        PolicyHistoryRequestDto resultEB = AddPolicyHistory(endorsmentHitsoryBundle, securityContext, auditContext);
                    }
                    #endregion

                    //Update policy table with endorsment
                    #region Original Policy Update
                    PolicyBundleRequestDto updatePolicy = new PolicyBundleRequestDto();
                    updatePolicy.Comment = endorsement.Comment;
                    updatePolicy.CommodityTypeId = endorsement.CommodityTypeId;
                    updatePolicy.ContractId = endorsement.ContractId;
                    updatePolicy.CoverTypeId = endorsement.CoverTypeId;
                    updatePolicy.CustomerId = endorsement.CustomerId;
                    updatePolicy.CustomerPayment = endorsement.CustomerPayment;
                    updatePolicy.CustomerPaymentCurrencyTypeId = endorsement.CustomerPaymentCurrencyTypeId;
                    updatePolicy.DealerId = endorsement.DealerId;
                    updatePolicy.DealerLocationId = endorsement.DealerLocationId;
                    updatePolicy.DealerPayment = endorsement.DealerPayment;
                    updatePolicy.DealerPaymentCurrencyTypeId = endorsement.DealerPaymentCurrencyTypeId;
                    updatePolicy.ExtensionTypeId = endorsement.ExtensionTypeId;
                    updatePolicy.HrsUsedAtPolicySale = endorsement.HrsUsedAtPolicySale;
                    updatePolicy.Id = endorsement.PolicyId;
                    updatePolicy.IsApproved = endorsement.IsApproved;
                    updatePolicy.IsPartialPayment = endorsement.IsPartialPayment;
                    updatePolicy.IsPreWarrantyCheck = endorsement.IsPreWarrantyCheck;
                    updatePolicy.IsSpecialDeal = endorsement.IsSpecialDeal;
                    updatePolicy.PaymentModeId = endorsement.PaymentModeId;
                    updatePolicy.PolicyNo = endorsement.PolicyNo;
                    updatePolicy.PolicySoldDate = endorsement.PolicySoldDate;
                    updatePolicy.Premium = endorsement.Premium;
                    updatePolicy.PremiumCurrencyTypeId = endorsement.PremiumCurrencyTypeId;
                    updatePolicy.ProductId = endorsement.ProductId;
                    updatePolicy.RefNo = endorsement.RefNo;
                    updatePolicy.SalesPersonId = endorsement.SalesPersonId;
                    updatePolicy.EntryDateTime = policy.EntryDateTime;
                    updatePolicy.EntryUser = policy.EntryUser;
                    updatePolicy.ItemId = ItemId;
                    PolicyBundleRequestDto reaultP = UpdatePolicyBundle(updatePolicy, securityContext, auditContext);
                    #endregion
                    #region Original Policy Bundle Updateforeach (var item in endorsementBundle)
                    {
                        PolicyRequestDto updatePolicyBundle = new PolicyRequestDto();
                        updatePolicyBundle.Comment = endorsement.Comment;
                        updatePolicyBundle.CommodityTypeId = endorsement.CommodityTypeId;
                        updatePolicyBundle.ContractId = endorsement.ContractId;
                        updatePolicyBundle.CoverTypeId = endorsement.CoverTypeId;
                        updatePolicyBundle.CustomerId = endorsement.CustomerId;
                        updatePolicyBundle.CustomerPayment = endorsement.CustomerPayment;
                        updatePolicyBundle.CustomerPaymentCurrencyTypeId = endorsement.CustomerPaymentCurrencyTypeId;
                        updatePolicyBundle.DealerId = endorsement.DealerId;
                        updatePolicyBundle.DealerLocationId = endorsement.DealerLocationId;
                        updatePolicyBundle.DealerPayment = endorsement.DealerPayment;
                        updatePolicyBundle.DealerPaymentCurrencyTypeId = endorsement.DealerPaymentCurrencyTypeId;
                        updatePolicyBundle.ExtensionTypeId = endorsement.ExtensionTypeId;
                        updatePolicyBundle.HrsUsedAtPolicySale = endorsement.HrsUsedAtPolicySale;
                        updatePolicyBundle.Id = endorsement.PolicyId;
                        updatePolicyBundle.IsApproved = endorsement.IsApproved;
                        updatePolicyBundle.IsPartialPayment = endorsement.IsPartialPayment;
                        updatePolicyBundle.IsPreWarrantyCheck = endorsement.IsPreWarrantyCheck;
                        updatePolicyBundle.IsSpecialDeal = endorsement.IsSpecialDeal;
                        updatePolicyBundle.PaymentModeId = endorsement.PaymentModeId;
                        updatePolicyBundle.PolicyNo = endorsement.PolicyNo;
                        updatePolicyBundle.PolicySoldDate = endorsement.PolicySoldDate;
                        updatePolicyBundle.Premium = endorsement.Premium;
                        updatePolicyBundle.PremiumCurrencyTypeId = endorsement.PremiumCurrencyTypeId;
                        updatePolicyBundle.ProductId = endorsement.ProductId;
                        updatePolicyBundle.RefNo = endorsement.RefNo;
                        updatePolicyBundle.SalesPersonId = endorsement.SalesPersonId;
                        updatePolicyBundle.EntryDateTime = policy.EntryDateTime;
                        updatePolicyBundle.EntryUser = policy.EntryUser;
                        updatePolicyBundle.PolicyBundleId = updatePolicy.Id;
                        updatePolicyBundle.ItemId = ItemId;
                        PolicyRequestDto reaultPb = UpdatePolicy(updatePolicyBundle, securityContext, auditContext);
                    }
                    #endregion
                    #region Original Customet Update
                    //Update customer details with endorsement
                    CustomerRequestDto updateCustomer = new CustomerRequestDto();
                    updateCustomer.Address1 = endorsementBundle[0].Address1;
                    updateCustomer.Address2 = endorsementBundle[0].Address2;
                    updateCustomer.Address3 = endorsementBundle[0].Address3;
                    updateCustomer.Address4 = endorsementBundle[0].Address4;
                    updateCustomer.BusinessAddress1 = endorsementBundle[0].BusinessAddress1;
                    updateCustomer.BusinessAddress2 = endorsementBundle[0].BusinessAddress2;
                    updateCustomer.BusinessAddress3 = endorsementBundle[0].BusinessAddress3;
                    updateCustomer.BusinessAddress4 = endorsementBundle[0].BusinessAddress4;
                    updateCustomer.BusinessName = endorsementBundle[0].BusinessName;
                    updateCustomer.BusinessTelNo = endorsementBundle[0].BusinessTelNo;
                    updateCustomer.CityId = endorsementBundle[0].CityId;
                    updateCustomer.CountryId = endorsementBundle[0].CountryId;
                    updateCustomer.CustomerTypeId = endorsementBundle[0].CustomerTypeId;
                    updateCustomer.DateOfBirth = endorsementBundle[0].DateOfBirth;
                    updateCustomer.DLIssueDate = endorsementBundle[0].DLIssueDate;
                    updateCustomer.Email = endorsementBundle[0].Email;
                    updateCustomer.EntryDateTime = customer.EntryDateTime;
                    updateCustomer.EntryUserId = customer.EntryUserId;
                    updateCustomer.FirstName = endorsementBundle[0].FirstName;
                    updateCustomer.Gender = endorsementBundle[0].Gender;
                    updateCustomer.Id = endorsementBundle[0].CustomerId.ToString();
                    updateCustomer.IDNo = endorsementBundle[0].IDNo;
                    updateCustomer.IDTypeId = endorsementBundle[0].IDTypeId;
                    updateCustomer.IsActive = endorsementBundle[0].IsActive;
                    updateCustomer.LastModifiedDateTime = DateTime.Now;
                    updateCustomer.LastName = endorsementBundle[0].LastName;
                    updateCustomer.MobileNo = endorsementBundle[0].MobileNo;
                    updateCustomer.NationalityId = endorsementBundle[0].NationalityId;
                    updateCustomer.OtherTelNo = endorsementBundle[0].OtherTelNo;
                    updateCustomer.Password = endorsementBundle[0].Password;
                    updateCustomer.UsageTypeId = endorsementBundle[0].UsageTypeId;
                    updateCustomer.UserName = endorsementBundle[0].UserName;
                    CustomerRequestDto resultC = new CustomerRequestDto();
                    if (updateCustomer.Id.ToString() == "00000000-0000-0000-0000-000000000000")
                    {
                        resultC = custMng.AddCustomer(updateCustomer, securityContext, auditContext);
                    }
                    else
                    {
                        resultC = custMng.UpdateCustomer(updateCustomer, securityContext, auditContext);
                    }
                    #endregion
                    #region Original Vehicle Update
                    if (Type == "Vehicle")
                    {
                        VehicleDetailsRequestDto updateVehicle = new VehicleDetailsRequestDto();
                        updateVehicle.AspirationId = endorsementBundle[0].AspirationId;
                        updateVehicle.BodyTypeId = endorsementBundle[0].BodyTypeId;
                        updateVehicle.CategoryId = endorsementBundle[0].CategoryId;
                        updateVehicle.CylinderCountId = endorsementBundle[0].CylinderCountId;
                        updateVehicle.DealerPrice = endorsementBundle[0].DealerPrice;
                        updateVehicle.DriveTypeId = endorsementBundle[0].DriveTypeId;
                        updateVehicle.EngineCapacityId = endorsementBundle[0].EngineCapacityId;
                        updateVehicle.EntryDateTime = vehicle.EntryDateTime;
                        updateVehicle.EntryUser = vehicle.EntryUser;
                        updateVehicle.FuelTypeId = endorsementBundle[0].FuelTypeId;
                        updateVehicle.Id = endorsementBundle[0].VehicleId;
                        updateVehicle.ItemPurchasedDate = endorsementBundle[0].ItemPurchasedDate;
                        updateVehicle.ItemStatusId = endorsementBundle[0].ItemStatusId;
                        updateVehicle.MakeId = endorsementBundle[0].MakeId;
                        updateVehicle.ModelId = endorsementBundle[0].ModelId;
                        updateVehicle.ModelYear = endorsementBundle[0].ModelYear;
                        updateVehicle.PlateNo = endorsementBundle[0].PlateNo;
                        updateVehicle.TransmissionId = endorsementBundle[0].TransmissionId;
                        updateVehicle.Variant = endorsementBundle[0].Variant;
                        updateVehicle.VehiclePrice = endorsementBundle[0].VehiclePrice;
                        updateVehicle.VINNo = endorsementBundle[0].VINNo;
                        updateVehicle.RegistrationDate = endorsementBundle[0].RegistrationDate;
                        VehicleDetailsRequestDto resultV = new VehicleDetailsRequestDto();
                        if (endorsementBundle[0].VehicleId.ToString() == "00000000-0000-0000-0000-000000000000")
                        {
                            resultV = vehicleMng.AddVehicleDetails(updateVehicle, securityContext, auditContext);
                        }
                        else
                        {
                            resultV = vehicleMng.UpdateVehicleDetails(updateVehicle, securityContext, auditContext);
                        }
                    }
                    #endregion
                    #region Original Electronic Update
                    else
                    {
                        BrownAndWhiteDetailsRequestDto updateBAndW = new BrownAndWhiteDetailsRequestDto();
                        updateBAndW.AddnSerialNo = endorsementBundle[0].AddnSerialNo;
                        updateBAndW.CategoryId = endorsementBundle[0].CategoryId;
                        updateBAndW.DealerPrice = endorsementBundle[0].DealerPrice;
                        updateBAndW.EntryDateTime = bAndW.EntryDateTime;
                        updateBAndW.EntryUser = bAndW.EntryUser;
                        updateBAndW.Id = endorsementBundle[0].BAndWId;
                        updateBAndW.InvoiceNo = endorsementBundle[0].InvoiceNo;
                        updateBAndW.ItemPrice = endorsementBundle[0].ItemPrice;
                        updateBAndW.ItemPurchasedDate = endorsementBundle[0].ItemPurchasedDate;
                        updateBAndW.ItemStatusId = endorsementBundle[0].ItemStatusId;
                        updateBAndW.MakeId = endorsementBundle[0].MakeId;
                        updateBAndW.ModelCode = endorsementBundle[0].ModelCode;
                        updateBAndW.ModelId = endorsementBundle[0].ModelId;
                        updateBAndW.ModelYear = endorsementBundle[0].ModelYear;
                        updateBAndW.SerialNo = endorsementBundle[0].SerialNo;
                        BrownAndWhiteDetailsRequestDto resultB = new BrownAndWhiteDetailsRequestDto();
                        if (endorsementBundle[0].BAndWId.ToString() == "00000000-0000-0000-0000-000000000000")
                        {
                            resultB = bAndWMng.AddBrownAndWhiteDetails(updateBAndW, securityContext, auditContext);
                        }
                        else
                        {
                            resultB = bAndWMng.UpdateBrownAndWhiteDetails(updateBAndW, securityContext, auditContext);
                        }
                    }
                    #endregion
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region History
        public PolicyHistoryRequestDto AddPolicyHistory(PolicyHistoryRequestDto Policy, SecurityContext securityContext, AuditContext auditContext)
        {
            PolicyHistoryRequestDto result = new PolicyHistoryRequestDto();
            PolicyHistoryInsertionUnitOfWork uow = new PolicyHistoryInsertionUnitOfWork(Policy);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.PolicyInsertion = uow.Policy.PolicyInsertion;
            return result;
        }
        public PolicyHistoriesResponseDto GetPolicyHistories(SecurityContext securityContext, AuditContext auditContext)
        {
            PolicyHistoriesResponseDto result = null;
            PolicyHistoriesRetrievalUnitOfWork uow = new PolicyHistoriesRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }
        #endregion

        #region Cancellation Bundle
        public PolicyBundleTransactionRequestDto AddPolicyBundleCancellation(PolicyBundleTransactionRequestDto Policy, SecurityContext securityContext, AuditContext auditContext)
        {
            PolicyTransactionTypesResponseDto resultType = null;
            PolicyTransactionTypesRetrievalUnitOfWork uowType = new PolicyTransactionTypesRetrievalUnitOfWork();
            uowType.SecurityContext = securityContext;
            uowType.AuditContext = auditContext;
            if (uowType.PreExecute())
            {
                uowType.Execute();
            }
            resultType = uowType.Result;
            Policy.TransactionTypeId = resultType.PolicyTransactionTypes.Find(t => t.Code == "Cancellation").Id;
            PolicyBundleTransactionRequestDto result = new PolicyBundleTransactionRequestDto();
            PolicyBundleTransactionInsertionUnitOfWork uow = new PolicyBundleTransactionInsertionUnitOfWork(Policy);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.PolicyBundleInsertion = uow.Policy.PolicyBundleInsertion;
            return result;
        }
        #endregion

        #region Cancellation
        public string PolicyCancellationReject(Guid PolicyBundleId, Guid UserId, SecurityContext securityContext, AuditContext auditContext)
        {
            String Response = String.Empty;
            PolicyCancellationRejectUnitOfWork uow = new PolicyCancellationRejectUnitOfWork(PolicyBundleId, UserId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            Response = uow.Result;
            return Response;
        }

        public string PolicyCancellationApproval(Guid PolicyBundleId, Guid UserId, SecurityContext securityContext, AuditContext auditContext)
        {
            String Response = String.Empty;
            PolicyCancellationApprovalUnitOfWork uow = new PolicyCancellationApprovalUnitOfWork(PolicyBundleId, UserId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            Response = uow.Result;
            return Response;
        }
        public string GetPolicyCancellationCommentByPolicyBundleId(Guid PolicyBundleId, SecurityContext securityContext, AuditContext auditContext)
        {
            String Response = String.Empty;
            PolicyCancellationCommentByPolicyBundleIdUnitOfWork uow = new PolicyCancellationCommentByPolicyBundleIdUnitOfWork(PolicyBundleId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            Response = uow.Result;
            return Response;
        }

        public string PolicyCancellation(Guid PolicyBundleId, string CancellationComment, Guid UserId, SecurityContext securityContext, AuditContext auditContext)
        {
            String Response = "Policy Cancellation Request Failed.";
            PolicyCancellationRequestUnitOfWork uow = new PolicyCancellationRequestUnitOfWork(PolicyBundleId, UserId, CancellationComment);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            Response = uow.Result;
            return Response;
        }

        public PolicyTransactionRequestDto AddPolicyCancellation(PolicyTransactionRequestDto Policy, SecurityContext securityContext, AuditContext auditContext)
        {
            PolicyTransactionTypesResponseDto resultType = null;
            PolicyTransactionTypesRetrievalUnitOfWork uowType = new PolicyTransactionTypesRetrievalUnitOfWork();
            uowType.SecurityContext = securityContext;
            uowType.AuditContext = auditContext;
            if (uowType.PreExecute())
            {
                uowType.Execute();
            }
            resultType = uowType.Result;
            Policy.TransactionTypeId = resultType.PolicyTransactionTypes.Find(t => t.Code == "Cancellation").Id;
            PolicyTransactionRequestDto result = new PolicyTransactionRequestDto();
            PolicyTransactionInsertionUnitOfWork uow = new PolicyTransactionInsertionUnitOfWork(Policy);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.PolicyInsertion = uow.PolicyEndorsement.PolicyInsertion;
            return result;
        }
        #endregion

        #region Inquiry
        public PolicyTransactionRequestDto GetPolicyInquiryData(SecurityContext securityContext, AuditContext auditContext)
        {
            try
            {
            }
            catch (Exception)
            {

            }
            //read data from sources and add to one object

            //original policy
            //endorsement
            //---- approvals
            // --- endorsements
            //--- cancelations
            return new PolicyTransactionRequestDto();
        }
        #endregion

        #region "Mics"
        public CustomerCheckResponseDto CheckCustomerById(Guid CustomerId, Guid LoggedInUserId, SecurityContext securityContext, AuditContext auditContext)
        {
            CustomerCheckResponseDto Response = new CustomerCheckResponseDto();
            CustomerCheckUnitOfWork uow = new CustomerCheckUnitOfWork(CustomerId, LoggedInUserId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            Response = uow.Result;
            return Response;
        }

        public object GetDocumentTypesByPageName(string PageName, SecurityContext securityContext, AuditContext auditContext)
        {
            DocumentTypesRetrievalByPageNameUnitOfWork uow = new DocumentTypesRetrievalByPageNameUnitOfWork(PageName);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }
        #endregion "Mics"

        public PolicyTransactionTypesResponseDto GetPolicyTransactionTypes(SecurityContext securityContext, AuditContext auditContext)
        {
            PolicyTransactionTypesResponseDto result = null;
            PolicyTransactionTypesRetrievalUnitOfWork uow = new PolicyTransactionTypesRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }


        public string TransferPolicy(SavePolicyRequestDto SavePolicyRequest, SecurityContext securityContext, AuditContext auditContext)
        {
            String response = String.Empty;
            PolicyTransferUnitOfWork uow = new PolicyTransferUnitOfWork(SavePolicyRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            response = uow.Result;
            return response;
        }


        public string RenewPolicy(SavePolicyRequestDto SavePolicyRequest, SecurityContext securityContext, AuditContext auditContext)
        {
            String response = String.Empty;
            PolicyRenewalUnitOfWork uow = new PolicyRenewalUnitOfWork(SavePolicyRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            response = uow.Result;
            return response;
        }

        public byte[] GetPolicyAttachmentById(Guid policyid, SecurityContext securityContext, AuditContext auditContext)
        {
            byte[] response;
            PolicyGetAttachmentByIdUnitOfWork uow = new PolicyGetAttachmentByIdUnitOfWork(policyid);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            response = uow.Result;
            return response;
        }

        public object RetrivePolicySectionData(Guid loggedInUserId, SecurityContext securityContext, AuditContext auditContext)
        {
            object response = null;
            PolicySectionDataRetrievalForDashboardUnitOfWork uow = new PolicySectionDataRetrievalForDashboardUnitOfWork(loggedInUserId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            response = uow.Result;
            return response;
        }

        public string GetPolicyNumber(Guid branchId, Guid dealerId, Guid productId,Guid tpaId,Guid commodityTypeId, SecurityContext securityContext,
            AuditContext auditContext)
        {
            string response = string.Empty;
            GetPolicyNumberUnitOfWork uow = new GetPolicyNumberUnitOfWork(branchId, dealerId, productId, tpaId, commodityTypeId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            response = uow.Result;
            return response;
        }

        public object ManufacturerWarrentyAvailabilityCheckOnPolicySave(ManufacturerWarrentyAvailabilityCheckDto policyDetails,
            SecurityContext securityContext, AuditContext auditContext)
        {
            object response = null;
            ManufacturerWarrentyAvailabilityCheckUnitOfWork uow = new ManufacturerWarrentyAvailabilityCheckUnitOfWork(policyDetails);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            response = uow.Result;
            return response;
        }

        public object GetPolicesByCustomerId(Guid CustomerId, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = new object();
            GetAllPolicyDetailsByCustomerIdUnitOfWork uow = new GetAllPolicyDetailsByCustomerIdUnitOfWork(CustomerId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }

            return Response;
        }

        public object GetOtherTirePolicyById(Guid PolicyId, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = new object();
            GetAllOtherTirePolicyDetailsByIdUnitOfWork uow = new GetAllOtherTirePolicyDetailsByIdUnitOfWork(PolicyId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }

            return Response;
        }

        public InvoiceCodeRequestDto UpdateInvoiceCode(InvoiceCodeRequestDto InvoiceCode,PolicyRequestDto p, SecurityContext securityContext, AuditContext auditContext)
        {

            InvoiceCodeRequestDto result = new InvoiceCodeRequestDto();
            InvoiceCodeUpdationUnitOfWork uow = new InvoiceCodeUpdationUnitOfWork(InvoiceCode,p);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result.InvoiceCodeInsertion = uow.InvoiceCode.InvoiceCodeInsertion;
            return result;
        }

        #region Extenal Product Actions
        public object GetAllAdditionalFieldDetailsByProductCode(Guid tpaId, string productCode)
        {
            object Response = new object();
            GetAllAdditionalFieldDetailsByProductCodeUnitOfWork uow = new GetAllAdditionalFieldDetailsByProductCodeUnitOfWork(tpaId, productCode);

            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }

            return Response;
        }

        public object GetTireDetailsByInvoiceCode(Guid tpaId, string invoiceCode)
        {
            object Response = new object();
            GetAllTireDetailsByInvoiceCodeUnitOfWork uow = new GetAllTireDetailsByInvoiceCodeUnitOfWork(tpaId, invoiceCode);
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object LoadSavedCustomerInvoiceDataById(Guid tpaId, Guid tempInvoiceId)
        {
            object Response = new object();
            LoadSavedCustomerInvoiceDataByIdUnitOfWork uow = new LoadSavedCustomerInvoiceDataByIdUnitOfWork(tpaId, tempInvoiceId);
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object SaveCustomerEnterdInvoiceDetails(SaveCustomerPolicyInfoRequestDto saveCustomerPolicyDto)
        {
            object Response = new object();
            SaveCustomerEnterdInvoiceDetailsUnitOfWork uow = new SaveCustomerEnterdInvoiceDetailsUnitOfWork(saveCustomerPolicyDto);
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object GetAllCountries(Guid tpaId)
        {
            object Response = new object();
            GetAllNationalitiesByTpaIdUnitOfWork uow = new GetAllNationalitiesByTpaIdUnitOfWork(tpaId);
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object GetAllCitiesByCountryId(Guid tpaId, Guid countryId)
        {
            object Response = new object();
            GetAllCitiesByTpaIdAndCountryIdUnitOfWork uow = new GetAllCitiesByTpaIdAndCountryIdUnitOfWork(tpaId, countryId);
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object GetAllUsageTypesByTpaId(Guid tpaId)
        {
            object Response = new object();
            GetAllUsageTypesByTpaIdUnitOfWork uow = new GetAllUsageTypesByTpaIdUnitOfWork(tpaId);
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object GetAllCustomerTypes(Guid tpaId)
        {
            object Response = new object();
            GetAllCustomerTypesTpaIdUnitOfWork uow = new GetAllCustomerTypesTpaIdUnitOfWork(tpaId);
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object SaveCustomerEnterdPolicy(SaveCustomerEnterdPolicyRequestDto saveCustomerPolicyDto)
        {
            object Response = new object();
            SaveCustomerEnterdPolicyByTpaIdUnitOfWork uow = new SaveCustomerEnterdPolicyByTpaIdUnitOfWork(saveCustomerPolicyDto);
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;

        }

        public object DownloadPolicyStatementforTYER(Guid policyId, SecurityContext securityContext, AuditContext auditContext)
        {
            GetPolicyStatementforTYERUnitOfWork uow = new GetPolicyStatementforTYERUnitOfWork(policyId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object checkCustomerExist(String mobile, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = null;
            CheckCustomerExistUnitOfWork uow = new CheckCustomerExistUnitOfWork(mobile);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public object GetEMIValue(decimal loneAmount, Guid ContractId, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = null;
            GetEMIValueBYProductPriceUnitOfWork uow = new GetEMIValueBYProductPriceUnitOfWork(loneAmount, ContractId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public List<PolicyResponseDto> GetPolicysByIdAndType(Guid policyId, string policyType, SecurityContext securityContext, AuditContext auditContext)
        {
            List<PolicyResponseDto> result = null;
            PoliciesByIdAndTypeRetrievalUnitOfWork uow = new PoliciesByIdAndTypeRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.policyId = policyId;
            uow.polityType= policyType;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public bool addPolicyAttachement(Guid id, List<Guid> uploadAttachments, SecurityContext securityContext, AuditContext auditContext)
        {
            bool Response = false;
            AddPolicyAttachementUnitOfWork uow = new AddPolicyAttachementUnitOfWork(id, uploadAttachments);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public AttachmentsByUsersResponseDto GetAttachmentsByPolicyIdByUserType(Guid policyId, SecurityContext securityContext, AuditContext auditContext)
        {
            AttachmentsRetrivalByPolicyIdByUserTypeUnitOfWork uow = new AttachmentsRetrivalByPolicyIdByUserTypeUnitOfWork(policyId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }
        #endregion
    }


}
