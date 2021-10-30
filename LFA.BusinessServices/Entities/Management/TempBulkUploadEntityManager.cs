using NHibernate;
using NHibernate.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TAS.DataTransfer.Requests;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.Entities.Management
{
    public class TempBulkUploadEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        internal List<TempBulkUpload> AddTempBulkUpload(TempBulkHeaderRequestDto tempBulkUploadData)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                int lineid = 1;
                Guid tempbulkheaderId;
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        TempBulkHeader thd = new Entities.TempBulkHeader();

                        thd.UserId = tempBulkUploadData.UserId;
                        thd.StartDate = tempBulkUploadData.StartDate;
                        thd.EndDate = tempBulkUploadData.EndDate;
                        thd.FileName = tempBulkUploadData.FileName;
                        thd.EntryDateTime = DateTime.Today.ToUniversalTime();
                        thd.TPAId = tempBulkUploadData.TPAId;
                        thd.CommodityTypeId = tempBulkUploadData.CommodityTypeId;
                        thd.ProductId = tempBulkUploadData.ProductId;

                        session.SaveOrUpdate(thd);
                        tempbulkheaderId = thd.TempBulkHeaderId;

                        foreach (TempBulkUploadRequestDto tempData in tempBulkUploadData.TempBulkUploads)
                        {
                            TempBulkUpload td = new Entities.TempBulkUpload();


                            td.TempBulkHeaderId = thd.TempBulkHeaderId;
                            td.FirstName = tempData.FirstName;
                            td.LastName = tempData.LastName;
                            td.Title = tempData.Title;
                            td.Occupation = tempData.Occupation;
                            td.MaritalStatus = tempData.MaritalStatus;
                            td.Country = tempData.Country;
                            td.City = tempData.City;
                            td.Nationality = tempData.Nationality;
                            td.PostalCode = tempData.PostalCode;
                            td.Email = tempData.Email;
                            td.MobilePhone = tempData.MobilePhone;
                            td.OtherPhone = tempData.OtherPhone;

                            //DateTime dob;
                            //DateTime.TryParse(tempData.DateOfBirth, out dob);
                            //if ((dob != DateTime.Parse("01/01/0001"))) td.DateOfBirth = dob;

                            if (tempData.DateOfBirth != null && tempData.DateOfBirth !="")
                                td.DateOfBirth = DateTime.FromOADate(double.Parse(tempData.DateOfBirth));

                            td.Gender = tempData.Gender;
                            td.CustomerType = tempData.CustomerType;
                            td.UsageType = tempData.UsageType;
                            td.Address1 = tempData.Address1;
                            td.Address2 = tempData.Address2;
                            td.Address3 = tempData.Address3;
                            td.Address4 = tempData.Address4;
                            td.IDNo = tempData.IDNo;
                            td.IDType = tempData.IDType;

                            //DateTime drivingIssueDate;
                            //DateTime.TryParse(tempData.DrivingLicenseIssueDate, out drivingIssueDate);
                            //if ((drivingIssueDate != DateTime.Parse("01/01/0001"))) td.DrivingIssueDate = drivingIssueDate;
                            if (tempData.DrivingLicenseIssueDate != null && tempData.DrivingLicenseIssueDate != "")
                                td.DrivingIssueDate = DateTime.FromOADate(double.Parse(tempData.DrivingLicenseIssueDate));

                            td.BusinessName = tempData.BusinessName;
                            td.BusinessAddress1 = tempData.BusinessAddress1;
                            td.BusinessAddress2 = tempData.BusinessAddress2;
                            td.BusinessAddress3 = tempData.BusinessAddress3;
                            td.BusinessAddress4 = tempData.BusinessAddress4;
                            td.BusinessTelephoneNo = tempData.BusinessTelephoneNo;
                            td.CommodityTypeCode = tempData.CommodityTypeCode;
                            td.ProductCode = tempData.ProductCode;

                            td.DealerCode = tempData.DealerCode;
                            td.DealerLocationCode = tempData.DealerLocation;

                            td.ContractCode = tempData.ContractCode;

                            td.ExtensionTypeCode = tempData.ExtensionType;

                            td.CoverTypeCode = tempData.CoverTypeCode;

                            td.SalesPersonCode = tempData.SalesPerson;

                            td.KmAtPolicySale = tempData.KmAtPolicySale;
                            td.HrsAtPolicySale = tempData.HrsAtPolicySale;
                            td.Comment = tempData.Comment;

                            decimal.TryParse(tempData.Premium, out decimal Premium);
                            td.Premium = Premium;

                            //DateTime policySoldDate;
                            //DateTime.TryParse(tempData.PolicySoldDate, out policySoldDate);
                            //if ((policySoldDate != DateTime.Parse("01/01/0001"))) td.PolicySoldDate = policySoldDate;
                            if (tempData.PolicySoldDate != null)
                                td.PolicySoldDate = DateTime.FromOADate(double.Parse(tempData.PolicySoldDate));


                            //DateTime policyStartDate;
                            //DateTime.TryParse(tempData.PolicyStartDate, out policyStartDate);
                            //if ((policyStartDate != DateTime.Parse("01/01/0001"))) td.PolicyStartDate = policyStartDate;
                            if (tempData.PolicyStartDate != null)
                                td.PolicyStartDate = DateTime.FromOADate(double.Parse(tempData.PolicyStartDate));

                            //DateTime policyEndDate;
                            //DateTime.TryParse(tempData.PolicyEndDate, out policyEndDate);
                            //if ((policyEndDate != DateTime.Parse("01/01/0001"))) td.PolicyEndDate = policyEndDate;
                            if (tempData.PolicyEndDate != null)
                                td.PolicyEndDate = DateTime.FromOADate(double.Parse(tempData.PolicyEndDate));

                            decimal.TryParse(tempData.Discount, out decimal discount);
                            td.Discount = discount;

                            td.ProviderCode = tempData.ProviderCode;
                            td.SerialNo = tempData.SerialNo;
                            td.VINNo = tempData.VINNo;
                            td.MakeCode = tempData.MakeCode;
                            td.ModelCode = tempData.ModelCode;
                            td.CategoryCode = tempData.CategoryCode;
                            td.ItemStatusCode = tempData.ItemStatusCode;
                            td.CylinderCountCode = tempData.CylinderCountCode;
                            td.BodyTypeCode = tempData.BodyTypeCode;
                            td.FuelTypeCode = tempData.FuelTypeCode;
                            td.AspirationCode = tempData.AspirationCode;
                            td.TransmissionCode = tempData.TransmissionCode;

                            //DateTime itemPurchasedDate;
                            //DateTime.TryParse(tempData.ItemPurchasedDate, out itemPurchasedDate);
                            //if ((itemPurchasedDate != DateTime.Parse("01/01/0001"))) td.ItemPurchasedDate = itemPurchasedDate;
                            if (tempData.ItemPurchasedDate != null)
                                td.ItemPurchasedDate = DateTime.FromOADate(double.Parse(tempData.ItemPurchasedDate));

                            td.EngineCapacityCode = tempData.EngineCapacityCode;
                            td.DriveTypeCode = tempData.DriveTypeCode;
                            td.CommodityUsageTypeCode = tempData.CommodityUsageTypeCode;
                            td.VariantCode = tempData.VariantCode;
                            td.PlateNo = tempData.PlateNo;
                            td.ModelYear = tempData.ModelYear;

                            decimal.TryParse(tempData.VehiclePrice, out decimal vehiclePrice);
                            td.VehiclePrice = vehiclePrice;

                            td.EntryDateTime = tempData.EntryDateTime;
                            td.EntryUserId = tempData.EntryUserId;
                            td.IsUploaded = tempData.IsUploaded;
                            td.lineId = lineid;//tempData.lineId;
                            //td.OrderId = 0; // tempData.OrderId; identity

                            td.ValidationError = null;
                            td.Colour = "green";

                            td.IsSpecialDeal = tempData.IsSpecialDeal;
                            td.DealerPolicy = tempData.DealerPolicy;


                            //DateTime MWStartDate;
                            //DateTime.TryParse(tempData.MWStartDate, out MWStartDate);
                            //if ((MWStartDate != DateTime.Parse("01/01/0001"))) td.MWStartDate = MWStartDate;

                            if (tempData.MWStartDate != null && tempData.MWStartDate!="")
                                td.MWStartDate = DateTime.FromOADate(double.Parse(tempData.MWStartDate));

                            td.PolicyNo = tempData.PolicyNo;


                            decimal.TryParse(tempData.GrossWeight, out decimal grossWeight);
                            td.GrossWeight = grossWeight;


                            int.TryParse(tempData.ManufacturerWarrantyMonths, out int MWWarrantyMonths);
                            td.ManufacturerWarrantyMonths = MWWarrantyMonths;

                            td.ManufacturerWarrantyKm = tempData.ManufacturerWarrantyKm;


                            int.TryParse(tempData.ExtensionPeriod, out int ExPeriod);
                            td.ExtensionPeriod = ExPeriod;

                            //DateTime mwApplicableFrom;
                            //DateTime.TryParse(tempData.ManufacturerWarrantyApplicableFrom, out mwApplicableFrom);
                            //if ((mwApplicableFrom != DateTime.Parse("01/01/0001"))) td.MWStartDate = mwApplicableFrom;

                            if (tempData.ManufacturerWarrantyApplicableFrom != null && tempData.ManufacturerWarrantyApplicableFrom !="")
                                td.ManufacturerWarrantyApplicableFrom = DateTime.FromOADate(double.Parse(tempData.ManufacturerWarrantyApplicableFrom));

                            td.ExtensionMileage = tempData.ExtensionMileage;
                            td.ContactPersonFirstName = tempData.ContactPersonFirstName;
                            td.ContactPersonLastName = tempData.ContactPersonLastName;
                            td.ContactPersonMobileNo = tempData.ContactPersonMobileNo;
                            td.Dealer = tempData.Dealer;

                            decimal.TryParse(tempData.Deposit, out decimal deposit);
                            td.Deposit = deposit;

                            decimal.TryParse(tempData.FinanceAmount, out decimal financeAmount);
                            td.FinanceAmount = financeAmount;

                            int.TryParse(tempData.LoanPeriod, out int LoanPeriod);
                            td.LoanPeriod = LoanPeriod;

                            decimal.TryParse(tempData.PeriodOfCover, out decimal PeriodOfCover);
                            td.PeriodOfCover = PeriodOfCover;

                            decimal.TryParse(tempData.MonthlyEMI, out decimal MonthlyEMI);
                            td.MonthlyEMI = MonthlyEMI;

                            decimal.TryParse(tempData.InterestRate, out decimal InterestRate);
                            td.InterestRate = InterestRate;

                            decimal.TryParse(tempData.InterestRate, out decimal GrossPremiumExcludingTAX);
                            td.GrossPremiumExcludingTAX = GrossPremiumExcludingTAX;

                            td.Make = tempData.Make;
                            td.Model = tempData.Model;

                            //DateTime vehicleRegistrationDate;
                            //DateTime.TryParse(tempData.VehicleRegistrationDate, out vehicleRegistrationDate);
                            //if ((vehicleRegistrationDate != DateTime.Parse("01/01/0001"))) td.VehicleRegistrationDate = vehicleRegistrationDate;

                            if (tempData.VehicleRegistrationDate != null && tempData.VehicleRegistrationDate != "")
                                td.VehicleRegistrationDate = DateTime.FromOADate(double.Parse(tempData.VehicleRegistrationDate));

                            /*

                            td.FirstName = tempData.FirstName;
                            td.LastName = tempData.LastName;
                            td.Title = tempData.Title;
                            td.Occupation = tempData.Occupation;
                            td.MaritalStatus = tempData.MaritalStatus;
                            td.Country = tempData.Country;
                            td.City = tempData.City;
                            td.Nationality = tempData.Nationality;
                            td.PostalCode = tempData.PostalCode;
                            td.Email = tempData.Email;
                            td.MobilePhone = tempData.MobilePhone;
                            td.OtherPhone = tempData.OtherPhone;



                            td.Gender = tempData.Gender;
                            td.CustomerType = tempData.CustomerType;
                            td.UsageType = tempData.UsageType;
                            td.Address1 = tempData.Address1;
                            td.Address2 = tempData.Address2;
                            td.Address3 = tempData.Address3;
                            td.Address4 = tempData.Address4;
                            td.IDType = tempData.IDType;
                            td.IDNo = tempData.IDNo;



                            td.BusinessName = tempData.BusinessName;
                            td.BusinessTelephoneNo = tempData.BusinessTelephoneNo;
                            td.BusinessAddress1 = tempData.BusinessAddress1;
                            td.BusinessAddress2 = tempData.BusinessAddress2;
                            td.BusinessAddress3 = tempData.BusinessAddress3;
                            td.BusinessAddress4 = tempData.BusinessAddress4;
                            td.ContactPersonFirstName = tempData.ContactPersonFirstName;
                            td.ContactPersonLastName = tempData.ContactPersonLastName;
                            td.ContactPersonMobileNo = tempData.ContactPersonMobileNo;

                            td.CommodityTypeCode = tempData.CommodityTypeCode;
                            td.ProductCode = tempData.ProductCode;
                            td.DealerCode = tempData.Dealer;
                            td.DealerLocationCode = tempData.DealerLocation;
                            td.ContractCode = tempData.ContractCode;
                            td.ExtensionTypeCode = tempData.ExtensionType;
                            td.CoverTypeCode = tempData.CoverTypeCode;
                            td.SalesPersonCode = tempData.SalesPerson;
                            td.KmAtPolicySale = tempData.KmAtPolicySale;
                            td.HrsAtPolicySale = tempData.HrsAtPolicySale;
                            td.Comment = tempData.Comment;

                            decimal Premium;
                            decimal.TryParse(tempData.Premium, out Premium);
                            td.Premium = Premium;

                            DateTime policySoldDate;
                            DateTime.TryParse(tempData.PolicySoldDate, out policySoldDate);
                            if ((policySoldDate != DateTime.Parse("01/01/0001"))) td.PolicySoldDate = policySoldDate;


                            DateTime policyStartDate;
                            DateTime.TryParse(tempData.PolicyStartDate, out policyStartDate);
                            if ((policyStartDate != DateTime.Parse("01/01/0001"))) td.PolicyStartDate = policyStartDate;

                            DateTime policyEndDate;
                            DateTime.TryParse(tempData.PolicyEndDate, out policyEndDate);
                            if ((policyEndDate != DateTime.Parse("01/01/0001"))) td.PolicyEndDate = policyEndDate;

                            decimal discount;
                            decimal.TryParse(tempData.Discount, out discount);
                            td.Discount = discount;

                            td.ProviderCode = tempData.ProviderCode;
                            td.SerialNo = tempData.SerialNo;
                            td.VINNo = tempData.VINNo;
                            td.MakeCode = tempData.MakeCode;
                            td.ModelCode = tempData.ModelCode;
                            td.CategoryCode = tempData.CategoryCode;
                            td.ItemStatusCode = tempData.ItemStatusCode;
                            td.CylinderCountCode = tempData.CylinderCountCode;
                            td.BodyTypeCode = tempData.BodyTypeCode;
                            td.FuelTypeCode = tempData.FuelTypeCode;
                            td.AspirationCode = tempData.AspirationCode;
                            td.TransmissionCode = tempData.TransmissionCode;

                            DateTime itemPurchasedDate;
                            DateTime.TryParse(tempData.ItemPurchasedDate, out itemPurchasedDate);
                            if ((itemPurchasedDate != DateTime.Parse("01/01/0001"))) td.ItemPurchasedDate = itemPurchasedDate;



                            td.EngineCapacityCode = tempData.EngineCapacityCode;
                            td.DriveTypeCode = tempData.DriveTypeCode;
                            td.CommodityUsageTypeCode = tempData.CommodityUsageTypeCode;
                            td.VariantCode = tempData.VariantCode;
                            td.PlateNo = tempData.PlateNo;
                            td.ModelYear = tempData.ModelYear;


                            decimal vehiclePrice;
                            decimal.TryParse(tempData.VehiclePrice, out vehiclePrice);
                            td.VehiclePrice = vehiclePrice;

                            td.PolicyNo = tempData.PolicyNo;

                            td.KmAtPolicySale = tempData.KmAtPolicySale;


                            td.ManufacturerWarrantyKm = tempData.ManufacturerWarrantyKm;
                            td.ExtensionMileage = tempData.ExtensionMileage;




                            td.Dealer = tempData.Dealer;



                            td.Make = tempData.Make;

                            td.Model = tempData.Model;


                            td.EntryDateTime = DateTime.Today.ToUniversalTime();
                            td.EntryUserId = thd.UserId;
                            td.IsUploaded = tempData.IsUploaded;
                            td.lineId = lineid;
                            td.ValidationError = null;
                            td.OrderId = 0;
                            td.Colour = "green";
                            td.IsSpecialDeal = tempData.IsSpecialDeal;
                            td.DealerPolicy = tempData.DealerPolicy;

                            DateTime mwStartDate;
                            DateTime.TryParse(tempData.MWStartDate, out mwStartDate);
                            if ((mwStartDate != DateTime.Parse("01/01/0001"))) td.MWStartDate = mwStartDate;
                            */

                            // additional policy details

                            td.NumberOfTyreCover = tempData.NumberOfTyreCover;
                            td.TyreBrand = tempData.TyreBrand;
                            td.FrontWidth = tempData.FrontWidth;
                            td.FrontTyreProfile = tempData.FrontTyreProfile;
                            td.FrontRadius = tempData.FrontRadius;
                            td.FrontSpeedRating = tempData.FrontSpeedRating;
                            td.FrontDot = tempData.FrontDot;

                            td.RearWidth = tempData.RearWidth;
                            td.RearTyreProfile = tempData.RearTyreProfile;
                            td.RearRadius = tempData.RearRadius;
                            td.RearSpeedRating = tempData.RearSpeedRating;
                            td.RearDot = tempData.RearDot;


                            session.SaveOrUpdate(td);
                            lineid++;

                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                        transaction.Rollback();
                        return null;
                    }
                }

                List<TempBulkUpload> entities = null;
                IQueryable<TempBulkUpload> bulkData = session.Query<TempBulkUpload>().Where(a => a.TempBulkHeaderId == tempbulkheaderId && a.IsUploaded == false); //
                entities = bulkData.ToList();

                return entities;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        internal bool SaveTempBulkData(List<TempBulkUpload> tempBulkData, string CommodityType, string ProductCode)
        {
            try
            {
                tempBulkData = tempBulkData.OrderBy(a => a.PolicySoldDate).ToList();
                ISession session = EntitySessionManager.GetSession();
                bool hasError = false;

                foreach (var item in tempBulkData)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Update(item);
                        transaction.Commit();
                    }

                    using (ITransaction transaction = session.BeginTransaction())
                    {

                        if (ProductCode.ToUpper() == "ADT")
                        {

                            var val = session.CreateSQLQuery("exec UploadBulkPolicyADT @TempBulkUploadId=:TempBulkUploadId")
                                               .AddScalar("ReturnValue", NHibernateUtil.Int32)
                                               .SetGuid("TempBulkUploadId", item.TempBulkUploadId)
                                               .List<Int32>();
                            if (val[0] != 0)
                            {
                                transaction.Commit();
                            }
                            else transaction.Rollback();

                            if (!hasError && val.First() == 2) { hasError = true; }

                        }
                        else {

                        var val = session.CreateSQLQuery("exec UploadBulkPolicyAutomobile @TempBulkUploadId=:TempBulkUploadId")
                                           .AddScalar("ReturnValue", NHibernateUtil.Int32)
                                           .SetGuid("TempBulkUploadId", item.TempBulkUploadId)
                                           .List<Int32>();
                            if (val[0] != 0)
                            {
                                transaction.Commit();
                            }
                            else transaction.Rollback();

                            if (!hasError && val.First() == 2) { hasError = true; }
                        }


                    }
                }


                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                throw ex;
            }
        }

        internal List<TempBulkUpload> GetValidationFailedTempBulkData(Guid tempBulkHederId, string CommodityType, string ProductCode)
        {
            ISession session = EntitySessionManager.GetSession();
            List<TempBulkUpload> entities = null;
            IQueryable<TempBulkUpload> bulkData = session.Query<TempBulkUpload>().Where(a => a.TempBulkHeaderId == tempBulkHederId && a.IsUploaded == false); //a.TempBulkHeaderId == tempBulkData.First().TempBulkHeaderId &&
            entities = bulkData.ToList();
            if (entities.Count > 0) {
                RollbackPolicyInsertion(tempBulkHederId, CommodityType, ProductCode);
            }
            return entities;
        }

        internal void RollbackPolicyInsertion(Guid tempBulkHederId, string CommodityType, string ProductCode) {
            ISession session = EntitySessionManager.GetSession();

            var uploadedData = session.Query<TempBulkUpload>().Where(a => a.TempBulkHeaderId == tempBulkHederId && a.IsUploaded == true)
                .Select(g => new { g.PolicyNo, refNo= int.Parse(g.PolicyNo.Substring(g.PolicyNo.Length-4)) })
                .OrderByDescending(o=> o.refNo)
                .ToList();

                using (ITransaction transaction = session.BeginTransaction())
                {
                try
                {
                    foreach (var upd in uploadedData)
                    {

                        if (ProductCode.ToUpper() == "ADT")
                        {
                            session.CreateSQLQuery("exec RemovePoliciesADT :policyNumber")
                            .SetString("policyNumber", upd.PolicyNo)
                            .ExecuteUpdate();
                        }
                        else
                        {
                            session.CreateSQLQuery("exec RemovePoliciesAutomobile :policyNumber")
                            .SetString("policyNumber", upd.PolicyNo)
                            .ExecuteUpdate();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }


        }

        internal bool IsAlreadyUpdloaded(TempBulkHeaderRequestDto tempBulkUploadData)
        {
            bool IsAlreadyUpdloaded = false;
            ISession session = EntitySessionManager.GetSession();
            try
            {
                int count = 0;
                count = session.Query<TempBulkHeader>().Where(a => a.UserId == tempBulkUploadData.UserId && a.StartDate < tempBulkUploadData.EndDate && a.EndDate >= tempBulkUploadData.EndDate && a.CommodityTypeId == tempBulkUploadData.CommodityTypeId && a.IsUploaded == true).Count();
                if (count > 0)
                {
                    IsAlreadyUpdloaded = true;
                }
                else
                {
                    count = session.Query<TempBulkHeader>().Where(a => a.UserId == tempBulkUploadData.UserId && a.StartDate <= tempBulkUploadData.StartDate && a.EndDate > tempBulkUploadData.StartDate && a.CommodityTypeId == tempBulkUploadData.CommodityTypeId && a.IsUploaded == true).Count();
                    if (count > 0) IsAlreadyUpdloaded = true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return IsAlreadyUpdloaded;
        }

        internal byte[] GetBulkUplaodTemplate(string commodityCode ,string productCode, string reportPath)
        {
            string fileName = "";
            byte[] template = null;
            switch (commodityCode)
            {
                case "A": fileName = GetAutomobileTemplateFileName(productCode);  break;
                case "B": fileName = "BulkUploadBank.xlsx"; break;
                case "E": fileName = "BulkUploadElectronic.xlsx"; break;
            }
            if (fileName != "")
            {
                string path = reportPath + "\\BulkUploadFormat\\" + fileName;
                String ExactHTMLPath = System.Web.HttpContext.Current.Server.MapPath(path);
                if (File.Exists(ExactHTMLPath))
                {
                    template = File.ReadAllBytes(ExactHTMLPath);
                }
            }
            return template;
        }

        private string GetAutomobileTemplateFileName(string productCode)
        {

            if (productCode.ToUpper() == "ADT")
            {
                return "BulkUploadTyres.xlsx";
            }
            else
            {
                return "BulkUploadAutomobile.xlsx";
            }
        }
    }
}


