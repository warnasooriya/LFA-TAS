using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using TAS.DataTransfer.Requests;
using TAS.Services;
using TAS.Services.Entities;
using TAS.Web.Common;

namespace TAS.Web.Controllers
{
    public class TempBulkUploadController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        //private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ITempBulkUploadManagementService TempBulkUploadManagementService;

        #region Bulk Uploader

        public Random r = new Random(1000);
        public static IList<TempBulkUploadRequestDto> dataList = null;
        public static List<TempBulkUpload> savedList = null;
        [HttpPost]
        public string Upload()
        {
            try
            {
                if (HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                    HttpPostedFile File = HttpContext.Current.Request.Files["file"];
                    Byte[] fileByte = null;
                    fileByte = new Byte[File.ContentLength];
                    File.InputStream.Read(fileByte, 0, File.ContentLength);
                    string excelPath = AppDomain.CurrentDomain.BaseDirectory;
                    string filePath = excelPath + r.Next().ToString() + ".xlsx";
                    File.SaveAs(filePath);
                    string commoditycode = HttpContext.Current.Request.Form["CommodityCode"].ToString();
                    if (isCorrectExcel(filePath, commoditycode))
                    {
                        TempBulkHeaderRequestDto uploaddata = new TempBulkHeaderRequestDto();

                        uploaddata.UserId = HttpContext.Current.Request.Form["UserID"].ToGuid();
                        uploaddata.StartDate = HttpContext.Current.Request.Form["StartDate"].ToDateTimeNullable();
                        uploaddata.EndDate = HttpContext.Current.Request.Form["EndDate"].ToDateTimeNullable();
                        uploaddata.TPAId = HttpContext.Current.Request.Form["TPAId"].ToGuid();
                        uploaddata.CommodityTypeId = HttpContext.Current.Request.Form["CommodityTypeId"].ToGuid();
                        uploaddata.ProductId = HttpContext.Current.Request.Form["ProductId"].ToGuid();

                        TempBulkUploadManagementService = ServiceFactory.GetTempBulkUploadManagementService();

                        if (!TempBulkUploadManagementService.isUploaded(uploaddata, SecurityHelper.Context, AuditHelper.Context))
                        {
                            switch (commoditycode)
                            {
                                case "A": dataList = ExcelReader.GetDataToList(filePath, AddAutomobileData); break;
                                case "B": dataList = ExcelReader.GetDataToList(filePath, AddBankData); break;
                                case "E": dataList = ExcelReader.GetDataToList(filePath, AddElectronicData); break;
                                case "Y": dataList = ExcelReader.GetDataToList(filePath, AddCustomerBulkData); break;
                                case "O": dataList = ExcelReader.GetDataToList(filePath, AddCustomerBulkData); break;
                            }
                            if (dataList != null)
                            {
                                List<TempBulkUploadRequestDto> uploaddatalist = new List<TempBulkUploadRequestDto>(dataList);

                                uploaddata.FileName = File.FileName;
                                uploaddata.TempBulkUploads = uploaddatalist;

                                savedList = TempBulkUploadManagementService.tempBulkUploadInsert(uploaddata, SecurityHelper.Context, AuditHelper.Context);
                                logger.Info("Uploaded");
                            }
                        }
                        else
                        {
                            return "Already uploaded for this period!";
                        }
                    }
                    else
                    {
                        return "Invalid Excel !";
                    }
                }
                return "Ok";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Error !";
            }
        }

        public bool isCorrectExcel(string path, string commodityCode)
        {
            bool retVal = false;
            OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0 xml;HDR=YES;'");
            connection.Open();
            DataTable Sheets = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string sheetName = "";
            foreach (DataRow dr in Sheets.Rows)
            {
                sheetName = dr[2].ToString().Replace("'", "").Replace("$", "");
                break;
            }
            connection.Close();

            if (commodityCode == "A" && sheetName == "Automobile - Policy Bulk Upload")
            {
                retVal = true;
            }
            else if (commodityCode == "B" && sheetName == "Bank - Policy Bulk Upload")
            {
                retVal = true;
            }
            else if (commodityCode == "E" && sheetName == "Policy-Electronic")
            {
                retVal = true;
            }
            else if (commodityCode == "Y" && sheetName == "Policy-Yellow Goods")
            {
                retVal = true;
            }
            else if (commodityCode == "O" && sheetName == "Policy-Other")
            {
                retVal = true;
            }

            return retVal;
        }



        public object ConvertData()
        {
            if (savedList == null)
                return null;
            return savedList.ToList();
        }

        public object SaveBulkUpload(JObject data)
        {
            List<TempBulkUpload> retval = null;
            try
            {
                List<TempBulkUpload> bulkData = data["BulkData"].ToObject<List<TempBulkUpload>>();
                string CommodityType = data["CommodityType"].ToString();
                string ProductCode = data["Product"].ToString();

                List<string> ErrorMsgList = new List<string>();
                TempBulkUploadManagementService = ServiceFactory.GetTempBulkUploadManagementService();
                if (TempBulkUploadManagementService.saveTempBulkUpload(bulkData,CommodityType,ProductCode, SecurityHelper.Context, AuditHelper.Context))
                {
                    retval = TempBulkUploadManagementService.getTempBulkValidationFailedData(bulkData.First().TempBulkHeaderId, CommodityType, ProductCode, SecurityHelper.Context, AuditHelper.Context);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Error !";
            }
            return retval;
        }

        private bool EmailValidator(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
                return true;
            else
                return false;
        }

        private static TempBulkUploadRequestDto AddCustomerBulkData(IList<string> rowData, IList<string> columnNames)
        {
            var CustomerBulk = new TempBulkUploadRequestDto()
            {
                FirstName = rowData[columnNames.IndexFor("First Name")],
                LastName = rowData[columnNames.IndexFor("Last Name")],
                Title = rowData[columnNames.IndexFor("Title")],
                Occupation = rowData[columnNames.IndexFor("Occupation")],
                MaritalStatus = rowData[columnNames.IndexFor("Marital Status")],
                Country = rowData[columnNames.IndexFor("Country")],
                City = rowData[columnNames.IndexFor("City")],
                Nationality = rowData[columnNames.IndexFor("Nationality")],
                PostalCode = rowData[columnNames.IndexFor("Postal Code")],
                Email = rowData[columnNames.IndexFor("Email Address")],
                MobilePhone = rowData[columnNames.IndexFor("Mobile Phone")],
                OtherPhone = rowData[columnNames.IndexFor("Other Phone")],
                DateOfBirth = rowData[columnNames.IndexFor("Date Of Birth")],
                Gender = rowData[columnNames.IndexFor("Gender")],
                CustomerType = rowData[columnNames.IndexFor("Customer Type")],
                UsageType = rowData[columnNames.IndexFor("Usage Type")],
                Address1 = rowData[columnNames.IndexFor("Address 1")],
                Address2 = rowData[columnNames.IndexFor("Address 2")],
                Address3 = rowData[columnNames.IndexFor("Address 3")],
                Address4 = rowData[columnNames.IndexFor("Address 4")],
                IDType = rowData[columnNames.IndexFor("ID Type")],
                IDNo = rowData[columnNames.IndexFor("ID No")],
                DrivingLicenseIssueDate = rowData[columnNames.IndexFor("Driving License Issue Date")],
                BusinessName = rowData[columnNames.IndexFor("Business Name")],
                BusinessTelephoneNo = rowData[columnNames.IndexFor("Business Telephone No")],
                BusinessAddress1 = rowData[columnNames.IndexFor("Business Address 1")],
                BusinessAddress2 = rowData[columnNames.IndexFor("Business Address 2")],
                BusinessAddress3 = rowData[columnNames.IndexFor("Business Address 3")],
                BusinessAddress4 = rowData[columnNames.IndexFor("Business Address 4")],
                ContactPersonFirstName = rowData[columnNames.IndexFor("Contact Person First Name")],
                ContactPersonLastName = rowData[columnNames.IndexFor("Contact Person Last Name")],
                ContactPersonMobileNo = rowData[columnNames.IndexFor("Contact Person Mobile No")],
                //Password = rowData[columnNames.IndexFor("Password")],
                //ConfirmPassword = rowData[columnNames.IndexFor("Password")],


                /*

                CommodityTypeCode = rowData[columnNames.IndexFor("CommodityTypeCode")],
                ProductCode = rowData[columnNames.IndexFor("ProductCode")],
                DealerCode = rowData[columnNames.IndexFor("DealerCode")],
                DealerLocationCode = rowData[columnNames.IndexFor("DealerLocationCode")],
                ContractCode = rowData[columnNames.IndexFor("ContractCode")],
                ExtensionTypeCode = rowData[columnNames.IndexFor("ExtensionTypeCode")],
                PremiumCurrencyTypeCode = rowData[columnNames.IndexFor("PremiumCurrencyTypeCode")],
                CoverTypeCode = rowData[columnNames.IndexFor("CoverTypeCode")],
                SalesPersonCode = rowData[columnNames.IndexFor("SalesPersonCode")],
                DealerPaymentCurrencyTypeCode = rowData[columnNames.IndexFor("DealerPaymentCurrencyTypeCode")],
                CustomerPaymentCurrencyTypeCode = rowData[columnNames.IndexFor("CustomerPaymentCurrencyTypeCode")],
                PaymentModeCode = rowData[columnNames.IndexFor("PaymentModeCode")],
                PaymentTypeCode = rowData[columnNames.IndexFor("PaymentTypeCode")],
                CustomerCode = rowData[columnNames.IndexFor("CustomerCode")],
                TPABranchCode = rowData[columnNames.IndexFor("TPABranchCode")],
                BordxCode = rowData[columnNames.IndexFor("BordxCode")],
                BordxCountryCode = rowData[columnNames.IndexFor("BordxCountryCode")],
                HrsUsedAtPolicySale = rowData[columnNames.IndexFor("HrsUsedAtPolicySale")],
                PolicyNo = rowData[columnNames.IndexFor("PolicyNo")],
                RefNo = rowData[columnNames.IndexFor("RefNo")],
                Comment = rowData[columnNames.IndexFor("Comment")],
                Premium = rowData[columnNames.IndexFor("Premium")],
                DealerPayment = rowData[columnNames.IndexFor("DealerPayment")],
                CustomerPayment = rowData[columnNames.IndexFor("CustomerPayment")],
                IsPreWarrantyCheck = rowData[columnNames.IndexFor("IsPreWarrantyCheck")],
                IsSpecialDeal = rowData[columnNames.IndexFor("IsSpecialDeal")],
                IsPartialPayment = rowData[columnNames.IndexFor("IsPartialPayment")],
                EntryUser = rowData[columnNames.IndexFor("EntryUser")],
                PolicySoldDate = rowData[columnNames.IndexFor("PolicySoldDate")],
                IsApproved = rowData[columnNames.IndexFor("IsApproved")],
                IsPolicyCanceled = rowData[columnNames.IndexFor("IsPolicyCanceled")],
                IsPolicyRenewed = rowData[columnNames.IndexFor("IsPolicyRenewed")],
                PolicyStartDate = rowData[columnNames.IndexFor("PolicyStartDate")],
                PolicyEndDate = rowData[columnNames.IndexFor("PolicyEndDate")],
                Discount = rowData[columnNames.IndexFor("Discount")],
                TransferFee = rowData[columnNames.IndexFor("TransferFee")],
                Year = rowData[columnNames.IndexFor("Year")],
                Month = rowData[columnNames.IndexFor("Month")],
                BordxNumber = rowData[columnNames.IndexFor("BordxNumber")],
                ForwardComment = rowData[columnNames.IndexFor("ForwardComment")],
                DealerPolicy = rowData[columnNames.IndexFor("DealerPolicy")],
                PolicyApprovedBy = rowData[columnNames.IndexFor("PolicyApprovedBy")],
                PaymentMethodFee = rowData[columnNames.IndexFor("PaymentMethodFee")],
                PaymentMethodFeePercentage = rowData[columnNames.IndexFor("PaymentMethodFeePercentage")],
                GrossPremiumBeforeTax = rowData[columnNames.IndexFor("GrossPremiumBeforeTax")],
                NRP = rowData[columnNames.IndexFor("NRP")],
                TotalTax = rowData[columnNames.IndexFor("TotalTax")],
                EligibilityFee = rowData[columnNames.IndexFor("EligibilityFee")],

                VINNo = rowData[columnNames.IndexFor("VINNo")],
                MakeCode = rowData[columnNames.IndexFor("MakeCode")],
                ModelCode = rowData[columnNames.IndexFor("ModelCode")],
                CategoryCode = rowData[columnNames.IndexFor("CategoryCode")],
                ItemStatusCode = rowData[columnNames.IndexFor("ItemStatusCode")],
                CylinderCountCode = rowData[columnNames.IndexFor("CylinderCountCode")],
                BodyTypeCode = rowData[columnNames.IndexFor("BodyTypeCode")],
                FuelTypeCode = rowData[columnNames.IndexFor("FuelTypeCode")],
                AspirationCode = rowData[columnNames.IndexFor("AspirationCode")],
                TransmissionCode = rowData[columnNames.IndexFor("TransmissionCode")],
                ItemPurchasedDate = rowData[columnNames.IndexFor("ItemPurchasedDate")],
                EngineCapacityCode = rowData[columnNames.IndexFor("EngineCapacityCode")],
                DriveTypeCode = rowData[columnNames.IndexFor("DriveTypeCode")],
                CommodityUsageTypeCode = rowData[columnNames.IndexFor("CommodityUsageTypeCode")],
                DealerCurrencyCode = rowData[columnNames.IndexFor("DealerCurrencyCode")],
                CountryCode = rowData[columnNames.IndexFor("CountryCode")],
                //DealerCode = rowData[columnNames.IndexFor("DealerCode")],
                VariantCode = rowData[columnNames.IndexFor("VariantCode")],
                DealerPrice = rowData[columnNames.IndexFor("DealerPrice")],
                PlateNo = rowData[columnNames.IndexFor("PlateNo")],
                ModelYear = rowData[columnNames.IndexFor("ModelYear")],
                //ItemPurchasedDate = rowData[columnNames.IndexFor("ItemPurchasedDate")],
                VehiclePrice = rowData[columnNames.IndexFor("VehiclePrice")]
                 */

                CommodityTypeCode = rowData[columnNames.IndexFor("CommodityTypeCode")],
                ProductCode = rowData[columnNames.IndexFor("Product Code")],
                Dealer = rowData[columnNames.IndexFor("Dealer")],
                DealerLocation = rowData[columnNames.IndexFor("Dealer Location")],
                ContractCode = rowData[columnNames.IndexFor("Contract")],
                ExtensionType = rowData[columnNames.IndexFor("Extension Type")],
                CoverTypeCode = rowData[columnNames.IndexFor("Warranty Type")],
                SalesPerson = rowData[columnNames.IndexFor("Sales Person")],
                KmAtPolicySale = rowData[columnNames.IndexFor("KmAtPolicySale")],
                HrsAtPolicySale = rowData[columnNames.IndexFor("HrsAtPolicySale")],
                //Comment = rowData[columnNames.IndexFor("Comment")],
                Premium = rowData[columnNames.IndexFor("Gross Premium USD")],
                PolicySoldDate = rowData[columnNames.IndexFor("Policy Sold Date")],
                PolicyStartDate = rowData[columnNames.IndexFor("Policy Start Date")],
                PolicyEndDate = rowData[columnNames.IndexFor("Policy End Date")],
                //Discount = rowData[columnNames.IndexFor("Discount")],
                //ProviderCode = rowData[columnNames.IndexFor("ProviderCode")],



                SerialNo = rowData[columnNames.IndexFor("SerialNo")],
                VINNo = rowData[columnNames.IndexFor("VINNo")],
                MakeCode = rowData[columnNames.IndexFor("Make")],
                ModelCode = rowData[columnNames.IndexFor("Model")],
                CategoryCode = rowData[columnNames.IndexFor("Category")],
                ItemStatusCode = rowData[columnNames.IndexFor("Item Status")],
                CylinderCountCode = rowData[columnNames.IndexFor("Cylinder Count")],
                BodyTypeCode = rowData[columnNames.IndexFor("Body Type")],
                FuelTypeCode = rowData[columnNames.IndexFor("Fuel Type")],
                AspirationCode = rowData[columnNames.IndexFor("Aspiration")],
                TransmissionCode = rowData[columnNames.IndexFor("TransmissionType")],
                ItemPurchasedDate = rowData[columnNames.IndexFor("Vehicle Purchased Date")],
                EngineCapacityCode = rowData[columnNames.IndexFor("Engine Capacity CC")],
                DriveTypeCode = rowData[columnNames.IndexFor("Drive Type")],
                CommodityUsageTypeCode = rowData[columnNames.IndexFor("Usage Type")],
                VariantCode = rowData[columnNames.IndexFor("Variant")],
                PlateNo = rowData[columnNames.IndexFor("PlateNo")],
                ModelYear = rowData[columnNames.IndexFor("ModelYear")],
                VehiclePrice = rowData[columnNames.IndexFor("Vehicle Price USD")],
                GrossWeight = rowData[columnNames.IndexFor("Gross Vehicle Weight Tonnage")],
                PolicyNo = rowData[columnNames.IndexFor("Policy No")],
                ManufacturerWarrantyMonths = rowData[columnNames.IndexFor("Manufacturer Warranty Months")],
                ManufacturerWarrantyKm = rowData[columnNames.IndexFor("Manufacturer Warranty Km")],
                ManufacturerWarrantyApplicableFrom = rowData[columnNames.IndexFor("Manufacturer Warranty Applicable From")],
                ExtensionPeriod = rowData[columnNames.IndexFor("Extension Period")],
                ExtensionMileage = rowData[columnNames.IndexFor("Extension Mileage")],
                MWStartDate = rowData[columnNames.IndexFor("MW Start Date")]
                //DealerPolicy = rowData[columnNames.IndexFor("DealerPolicy")],


            };
            return CustomerBulk;

        }

        private static TempBulkUploadRequestDto AddAutomobileData(IList<string> rowData, IList<string> columnNames)
        {
            var IsSpecialDe = rowData[columnNames.IndexFor("IsSpecialDeal")];
            bool deal;
            if (IsSpecialDe == "Yes")
            {
                deal = true;
            }
            else
            {
                deal = false;
            }
            //var IsDealerPolicy = rowData[columnNames.IndexFor("DealerPolicy")];
            //bool dealerpolicy;
            //if (IsDealerPolicy == "Yes")
            //{
            //    dealerpolicy = true;
            //}
            //else
            //{
            //    dealerpolicy = false;
            //}
            var CustomerBulk = new TempBulkUploadRequestDto()
            {
                FirstName = rowData[columnNames.IndexFor("First Name")],
                LastName = rowData[columnNames.IndexFor("Last Name")],
                Title = rowData[columnNames.IndexFor("Title")],
                Occupation = rowData[columnNames.IndexFor("Occupation")],
                MaritalStatus = rowData[columnNames.IndexFor("Marital Status")],
                Country = rowData[columnNames.IndexFor("Country")],
                City = rowData[columnNames.IndexFor("City")],
                Nationality = rowData[columnNames.IndexFor("Nationality")],
                PostalCode = rowData[columnNames.IndexFor("Postal Code")],
                Email = rowData[columnNames.IndexFor("Email Address")],
                MobilePhone = rowData[columnNames.IndexFor("Mobile Phone")],
                OtherPhone = rowData[columnNames.IndexFor("Other Phone")],
                DateOfBirth = rowData[columnNames.IndexFor("Date Of Birth")],
                Gender = rowData[columnNames.IndexFor("Gender")],
                CustomerType = rowData[columnNames.IndexFor("Customer Type")],
                UsageType = rowData[columnNames.IndexFor("Usage Type")],
                Address1 = rowData[columnNames.IndexFor("Address 1")],
                Address2 = rowData[columnNames.IndexFor("Address 2")],
                Address3 = rowData[columnNames.IndexFor("Address 3")],
                Address4 = rowData[columnNames.IndexFor("Address 4")],
                IDType = rowData[columnNames.IndexFor("ID Type")],
                IDNo = rowData[columnNames.IndexFor("ID No")],
                DrivingLicenseIssueDate = rowData[columnNames.IndexFor("Driving  License Issue Date")],
                BusinessName = rowData[columnNames.IndexFor("Business Name")],
                BusinessTelephoneNo = rowData[columnNames.IndexFor("Business Telephone No")],
                BusinessAddress1 = rowData[columnNames.IndexFor("Business Address 1")],
                BusinessAddress2 = rowData[columnNames.IndexFor("Business Address 2")],
                BusinessAddress3 = rowData[columnNames.IndexFor("Business Address 3")],
                BusinessAddress4 = rowData[columnNames.IndexFor("Business Address 4")],
                ContactPersonFirstName = rowData[columnNames.IndexFor("Contact Person First Name")],
                ContactPersonLastName = rowData[columnNames.IndexFor("Contact Person Last Name")],
                ContactPersonMobileNo = rowData[columnNames.IndexFor("Contact Person Mobile No")],

                //CommodityTypeCode = rowData[columnNames.IndexFor("CommodityTypeCode")],
                ProductCode = rowData[columnNames.IndexFor("Product Code")],
                DealerCode = rowData[columnNames.IndexFor("Dealer")],
                DealerLocation = rowData[columnNames.IndexFor("Dealer Location")],
                ContractCode = rowData[columnNames.IndexFor("Contract")],
                ExtensionType = rowData[columnNames.IndexFor("Extension Type")],
                CoverTypeCode = rowData[columnNames.IndexFor("Warranty Type")],
                SalesPerson = rowData[columnNames.IndexFor("Sales Person")],
                KmAtPolicySale = rowData[columnNames.IndexFor("KmAtPolicySale")],
                //HrsAtPolicySale = rowData[columnNames.IndexFor("HrsAtPolicySale")],
                //Comment = rowData[columnNames.IndexFor("Comment")],



                Premium = GetGrossPremium(rowData, columnNames),
                PolicySoldDate = rowData[columnNames.IndexFor("Policy Sold Date")],
                PolicyStartDate = rowData[columnNames.IndexFor("Policy Start Date")],
                PolicyEndDate = rowData[columnNames.IndexFor("Policy End Date")],
                //Discount = rowData[columnNames.IndexFor("Discount")],
                //ProviderCode = rowData[columnNames.IndexFor("ProviderCode")],



                //SerialNo = rowData[columnNames.IndexFor("SerialNo")],
                VINNo = rowData[columnNames.IndexFor("VINNo")],
                MakeCode = rowData[columnNames.IndexFor("Make")],
                ModelCode = rowData[columnNames.IndexFor("Model")],
                CategoryCode = rowData[columnNames.IndexFor("Category")],
                ItemStatusCode = rowData[columnNames.IndexFor("Item Status")],
                CylinderCountCode = rowData[columnNames.IndexFor("Cylinder Count")],
                BodyTypeCode = rowData[columnNames.IndexFor("Body Type")],
                FuelTypeCode = rowData[columnNames.IndexFor("Fuel Type")],
                AspirationCode = rowData[columnNames.IndexFor("Aspiration")],
                TransmissionCode = rowData[columnNames.IndexFor("TransmissionType")],
                ItemPurchasedDate = rowData[columnNames.IndexFor("Vehicle Purchased Date")],
                VehicleRegistrationDate = rowData[columnNames.IndexFor("Vehicle Registration Date")],
                EngineCapacityCode = rowData[columnNames.IndexFor("Engine Capacity CC")],
                DriveTypeCode = rowData[columnNames.IndexFor("Drive Type")],
                CommodityUsageTypeCode = rowData[columnNames.IndexFor("Usage Type")],
                VariantCode = rowData[columnNames.IndexFor("Variant")],
                PlateNo = rowData[columnNames.IndexFor("PlateNo")],
                ModelYear = rowData[columnNames.IndexFor("ModelYear")],
                VehiclePrice = rowData[columnNames.IndexFor("Vehicle Price USD")],
                GrossWeight = rowData[columnNames.IndexFor("Gross Vehicle Weight Tonnage")],
                IsSpecialDeal = deal,
                //DealerPolicy = dealerpolicy,
                MWStartDate = rowData[columnNames.IndexFor("MW Start Date")],
                PolicyNo = rowData[columnNames.IndexFor("Policy No")],
                ManufacturerWarrantyMonths = rowData[columnNames.IndexFor("Manufacturer Warranty Months")],
                ManufacturerWarrantyKm = rowData[columnNames.IndexFor("Manufacturer Warranty Km")],
                ManufacturerWarrantyApplicableFrom = rowData[columnNames.IndexFor("Manufacturer Warranty Applicable From")],
                ExtensionPeriod = rowData[columnNames.IndexFor("Extension Period")],
                ExtensionMileage = rowData[columnNames.IndexFor("Extension Mileage")],

                // Additional Policy Details For Gargash (Tyres Details)
                NumberOfTyreCover= GetCustomColumnValue(rowData, columnNames, "NUMBER OF TYRE COVER"),
                TyreBrand= GetCustomColumnValue(rowData, columnNames, "TYRE BRAND"),
                FrontWidth= GetCustomColumnValue(rowData, columnNames, "FRONT WIDTH"),
                FrontTyreProfile= GetCustomColumnValue(rowData, columnNames, "FRONT TYRE PROFILE"),
                FrontRadius= GetCustomColumnValue(rowData, columnNames, "FRONT RADIUS"),
                FrontSpeedRating= GetCustomColumnValue(rowData, columnNames, "FRONT SPEED RATING"),
                FrontDot= GetCustomColumnValue(rowData, columnNames, "FRONT DOT"),

                RearWidth= GetCustomColumnValue(rowData, columnNames, "REAR WIDTH"),
                RearTyreProfile= GetCustomColumnValue(rowData, columnNames, "REAR TYRE PROFILE"),
                RearRadius= GetCustomColumnValue(rowData, columnNames, "REAR RADIUS"),
                RearSpeedRating= GetCustomColumnValue(rowData, columnNames, "REAR SPEED RATING"),
                RearDot= GetCustomColumnValue(rowData, columnNames, "REAR DOT"),

            };
            return CustomerBulk;

        }

        private static string GetCustomColumnValue(IList<string> rowData, IList<string> columnNames, string excelColName)
        {
            string returnValue = String.Empty;
            try
            {
                returnValue = rowData[columnNames.IndexFor(excelColName)];
            }
            catch (Exception)
            {

                returnValue= String.Empty;
            }
            return returnValue;
        }

        private static string GetGrossPremium(IList<string> rowData, IList<string> columnNames)
        {
            string premiumLabel = columnNames.Where(c => c.Contains("grosspremium")).FirstOrDefault();
            return rowData[columnNames.IndexFor(premiumLabel)];
        }

        private static TempBulkUploadRequestDto AddBankData(IList<string> rowData, IList<string> columnNames)
        {
            var CustomerBulk = new TempBulkUploadRequestDto()
            {
                Dealer = rowData[columnNames.IndexFor("Finance Company")],
                FirstName = rowData[columnNames.IndexFor("Customer First Name")],
                LastName = rowData[columnNames.IndexFor("Customer Last Name")],
                DateOfBirth = rowData[columnNames.IndexFor("Date of Birth")],
                IDType = rowData[columnNames.IndexFor("ID Type")],
                IDNo = rowData[columnNames.IndexFor("ID Number")],
                Email = rowData[columnNames.IndexFor("Email Address")],
                Nationality = rowData[columnNames.IndexFor("Nationality")],
                VINNo = rowData[columnNames.IndexFor("VIN No (Chassie No)")],
                Make = rowData[columnNames.IndexFor("Make")],
                Model = rowData[columnNames.IndexFor("Model")],
                Country = rowData[columnNames.IndexFor("Country of Origin")],
                PlateNo = rowData[columnNames.IndexFor("Registration Number")],
                ItemPurchasedDate = rowData[columnNames.IndexFor("Vehicle Purchased Date")],
                VehiclePrice = rowData[columnNames.IndexFor("Vehicle Purchase Price (AED)")],
                Deposit = rowData[columnNames.IndexFor("Deposit (AED)")],
                FinanceAmount = rowData[columnNames.IndexFor("Finance Amount (AED)")],
                LoanPeriod = rowData[columnNames.IndexFor("Loan Period (Months)")],
                PeriodOfCover = rowData[columnNames.IndexFor("Period Of Cover (Months)")],
                MonthlyEMI = rowData[columnNames.IndexFor("Monthly EMI (AED)")],
                InterestRate = rowData[columnNames.IndexFor("Interest Rate(%)")],
                GrossPremiumExcludingTAX = rowData[columnNames.IndexFor("Gross Premium Excluding TAX (AED)")],
            };
            return CustomerBulk;

        }

        private static TempBulkUploadRequestDto AddElectronicData(IList<string> rowData, IList<string> columnNames)
        {
            var CustomerBulk = new TempBulkUploadRequestDto()
            {
                FirstName = rowData[columnNames.IndexFor("First Name")],
                LastName = rowData[columnNames.IndexFor("Last Name")],
                Title = rowData[columnNames.IndexFor("Title")],
                Occupation = rowData[columnNames.IndexFor("Occupation")],
                MaritalStatus = rowData[columnNames.IndexFor("Marital Status")],
                Country = rowData[columnNames.IndexFor("Country")],
                City = rowData[columnNames.IndexFor("City")],
                Nationality = rowData[columnNames.IndexFor("Nationality")],
                PostalCode = rowData[columnNames.IndexFor("Postal Code")],
                Email = rowData[columnNames.IndexFor("Email Address")],
                MobilePhone = rowData[columnNames.IndexFor("Mobile Phone")],
                OtherPhone = rowData[columnNames.IndexFor("Other Phone")],
                DateOfBirth = rowData[columnNames.IndexFor("Date Of Birth")],
                Gender = rowData[columnNames.IndexFor("Gender")],
                CustomerType = rowData[columnNames.IndexFor("Customer Type")],
                UsageType = rowData[columnNames.IndexFor("Usage Type")],
                Address1 = rowData[columnNames.IndexFor("Address 1")],
                Address2 = rowData[columnNames.IndexFor("Address 2")],
                Address3 = rowData[columnNames.IndexFor("Address 3")],
                Address4 = rowData[columnNames.IndexFor("Address 4")],
                IDType = rowData[columnNames.IndexFor("ID Type")],
                IDNo = rowData[columnNames.IndexFor("ID No")],
                DrivingLicenseIssueDate = rowData[columnNames.IndexFor("Driving  License Issue Date")],
                BusinessName = rowData[columnNames.IndexFor("Business Name")],
                BusinessTelephoneNo = rowData[columnNames.IndexFor("Business Telephone No")],
                BusinessAddress1 = rowData[columnNames.IndexFor("Business Address 1")],
                BusinessAddress2 = rowData[columnNames.IndexFor("Business Address 2")],
                BusinessAddress3 = rowData[columnNames.IndexFor("Business Address 3")],
                BusinessAddress4 = rowData[columnNames.IndexFor("Business Address 4")],
                ContactPersonFirstName = rowData[columnNames.IndexFor("Contact Person First Name")],
                ContactPersonLastName = rowData[columnNames.IndexFor("Contact Person Last Name")],
                ContactPersonMobileNo = rowData[columnNames.IndexFor("Contact Person Mobile No")],

                //CommodityTypeCode = rowData[columnNames.IndexFor("CommodityTypeCode")],
                ProductCode = rowData[columnNames.IndexFor("Product Code")],
                DealerCode = rowData[columnNames.IndexFor("Dealer")],
                DealerLocation = rowData[columnNames.IndexFor("Dealer Location")],
                ContractCode = rowData[columnNames.IndexFor("Contract")],
                ExtensionType = rowData[columnNames.IndexFor("Extension Type")],
                CoverTypeCode = rowData[columnNames.IndexFor("Warranty Type")],
                SalesPerson = rowData[columnNames.IndexFor("Sales Person")],
                //KmAtPolicySale = rowData[columnNames.IndexFor("KmAtPolicySale")],
                HrsAtPolicySale = rowData[columnNames.IndexFor("HrsAtPolicySale")],
                //Comment = rowData[columnNames.IndexFor("Comment")],
                Premium = rowData[columnNames.IndexFor("Gross Premium USD")],
                PolicySoldDate = rowData[columnNames.IndexFor("Policy Sold Date")],
                PolicyStartDate = rowData[columnNames.IndexFor("Policy Start Date")],
                PolicyEndDate = rowData[columnNames.IndexFor("Policy End Date")],
                //Discount = rowData[columnNames.IndexFor("Discount")],
                //ProviderCode = rowData[columnNames.IndexFor("ProviderCode")],



                SerialNo = rowData[columnNames.IndexFor("SerialNo")],
                //VINNo = rowData[columnNames.IndexFor("VINNo")],
                MakeCode = rowData[columnNames.IndexFor("Make")],
                ModelCode = rowData[columnNames.IndexFor("Model")],
                CategoryCode = rowData[columnNames.IndexFor("Category")],
                ItemStatusCode = rowData[columnNames.IndexFor("Item Status")],
                //CylinderCountCode = rowData[columnNames.IndexFor("CylinderCountCode")],
                //BodyTypeCode = rowData[columnNames.IndexFor("BodyTypeCode")],
                //FuelTypeCode = rowData[columnNames.IndexFor("FuelTypeCode")],
                //AspirationCode = rowData[columnNames.IndexFor("AspirationCode")],
                //TransmissionCode = rowData[columnNames.IndexFor("TransmissionCode")],
                ItemPurchasedDate = rowData[columnNames.IndexFor("Vehicle Purchased Date")],
                //EngineCapacityCode = rowData[columnNames.IndexFor("EngineCapacityCode")],
                //DriveTypeCode = rowData[columnNames.IndexFor("DriveTypeCode")],
                CommodityUsageTypeCode = rowData[columnNames.IndexFor("CommodityUsageTypeCode")],
                //VariantCode = rowData[columnNames.IndexFor("VariantCode")],
                //PlateNo = rowData[columnNames.IndexFor("PlateNo")],
                ModelYear = rowData[columnNames.IndexFor("ModelYear")],
                VehiclePrice = rowData[columnNames.IndexFor("Vehicle Price USD")]

            };
            return CustomerBulk;

        }

        private static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string dllName = args.Name.Contains(',') ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");
            dllName = dllName.Replace(".", "_");
            if (dllName.EndsWith("_resources")) return null;
            var obj = new Object();
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(obj.GetType().Namespace + ".Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());
            byte[] bytes = (byte[])rm.GetObject(dllName);
            return System.Reflection.Assembly.Load(bytes);
        }

        #endregion

        [HttpPost]
        public HttpResponseMessage GetBulkUploadTemplate(JObject data)
        {
            string commodityCode = data["commodityCode"].ToString();
            string productCode = data["productCode"].ToString();
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            TempBulkUploadManagementService = ServiceFactory.GetTempBulkUploadManagementService();
            var array = TempBulkUploadManagementService.getBulkUplaodTemplate(commodityCode, productCode, SecurityHelper.Context, AuditHelper.Context);

            string fileName = "";
            switch (commodityCode)
            {
                case "A": fileName = GetAutomobileTemplateFileName(productCode); break;
                case "B": fileName = "BulkUploadBank.xlsx"; break;
                case "E": fileName = "BulkUploadElectronic.xlsx"; break;
            }

            if (fileName != "")
            {
                HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(array);
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");
                result.Content.Headers.ContentDisposition.FileName = fileName;
                result.Content.Headers.Add("x-filename", fileName);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");
                return result;
            }
            else
            {
                HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.NotImplemented);
                result.Content = null;
                //result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");
                //result.Content.Headers.ContentDisposition.FileName = fileName;
                //result.Content.Headers.Add("x-filename", fileName);
                //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");
                return result;
            }


        }
        private string GetAutomobileTemplateFileName(string ProductCode)
        {
            if (ProductCode.ToUpper() == "ADT")
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

#region CommentSaveBulk
/*

            #region Service Factory
            ICountryManagementService CountryManagementService = ServiceFactory.GetCountryManagementService();
            ICityManagementService cityManagementService = ServiceFactory.GetCityManagementService();
            ICommodityManagementService CommodityManagementService = ServiceFactory.GetCommodityManagementService();
            IItemStatusManagementService ItemStatusManagementService = ServiceFactory.GetItemStatusManagementService();
            ICommodityCategoryManagementService CommodityCategoryManagementService = ServiceFactory.GetCommodityCategoryManagementService();
            IManufacturerManagementService ManufacturerManagementService = ServiceFactory.GetManufacturerManagementService();
            IManufacturerWarrantyManagementService ManufacturerWarrantyManagementService = ServiceFactory.GetManufacturerWarrantyManagementService();
            IMakeManagementService MakeManagementService = ServiceFactory.GetMakeManagementService();
            IModelManagementService ModelManagementService = ServiceFactory.GetModelManagementService();
            IProductManagementService productManagementService = ServiceFactory.GetProductManagementService();
            IInsurerManagementService InsurerManagementService = ServiceFactory.GetInsurerManagementService();
            IDealerManagementService DealerManagementService = ServiceFactory.GetDealerManagementService();
            IDealerLocationManagementService DealerLocationManagementService = ServiceFactory.GetDealerLocationManagementService();
            ICylinderCountManagementService cylinderCountManagementService = ServiceFactory.GetCylinderCountManagementService();
            IVehicleBodyTypeManagementService vehicleBodyTypeManagementService = ServiceFactory.GetVehicleBodyTypeManagementService();
            IDriveTypeManagementService driveTypeManagementService = ServiceFactory.GetDriveTypeManagementService();
            IEngineCapacityManagementService engineCapacityManagementService = ServiceFactory.GetEngineCapacityManagementService();
            IFuelTypeManagementService fuelTypeManagementService = ServiceFactory.GetFuelTypeManagementService();
            IVehicleColorManagementService VehicleColorManagementService = ServiceFactory.GetVehicleColorManagementService();
            ITransmissionTypeManagementService TransmissionTypeManagementService = ServiceFactory.GetTransmissionTypeManagementService();
            TransmissionTechnologiesResponseDto TransmissionTechData = TransmissionTypeManagementService.GetTransmissionTechnologies(SecurityHelper.Context, AuditHelper.Context);
            IVehicleAspirationTypeManagementService VehicleAspirationTypeManagementService = ServiceFactory.GetVehicleAspirationTypeManagementService();
            IVehicleDetailsManagementService VehicleDetailsManagementService = ServiceFactory.GetVehicleDetailsManagementService();
            ICommodityUsageTypeManagementService CommodityUsageTypeManagementService = ServiceFactory.GetCommodityUsageTypeManagementService();
            IVariantManagementService VariantManagementService = ServiceFactory.GetVariantManagementService();
            ICustomerManagementService customerManagementService = ServiceFactory.GetCustomerManagementService();
            IIdTypeManagementService IdTypeManagementService = ServiceFactory.GetIdTypeManagementService();
            ICustomerTypeManagementService customertypeManagementService = ServiceFactory.GetCustomerTypeManagementService();
            INationalityManagementService nationalityManagementService = ServiceFactory.GetNationalityManagementService();
            IUsageTypeManagementService usageTypeManagementService = ServiceFactory.GetUsageTypeManagementService();
            IExtensionTypeManagementService ExtensionTypeManagementService = ServiceFactory.GetExtensionTypeManagementService();
            IContractManagementService ContractManagementService = ServiceFactory.GetContractManagementService();
            IReinsurerManagementService ReinsurerManagementService = ServiceFactory.GetReinsurerManagementService();
            IWarrantyTypeManagementService WarrantyTypeManagementService = ServiceFactory.GetWarrantyTypeManagementService();
            IPremiumBasedOnManagementService PremiumBasedOnManagementService = ServiceFactory.GetPremiumBasedOnManagementService();
            IPremiumAddonTypeManagementService PremiumAddonTypeManagementService = ServiceFactory.GetPremiumAddonTypeManagementService();
            ICurrencyManagementService CurrencyManagementService = ServiceFactory.GetCurrencyManagementService();
            IRegionManagementService RegionManagementService = ServiceFactory.GetRegionManagementService();
            IRSAProviderManagementService RSAProviderManagementService = ServiceFactory.GetRSAProviderManagementService();
            IPolicyManagementService PolicyManagementService = ServiceFactory.GetPolicyManagementService();
            IUserManagementService userManagementService = ServiceFactory.GetUserManagementService();
            IBrownAndWhiteDetailsManagementService BrownAndWhiteDetailsManagementService = ServiceFactory.GetBrownAndWhiteDetailsManagementService();
            #endregion

            #region Search Data
            CountriesResponseDto CountryData = CountryManagementService.GetAllCountries(SecurityHelper.Context, AuditHelper.Context);
            CitiesResponseDto cityData = cityManagementService.GetAllCities(SecurityHelper.Context, AuditHelper.Context);
            CurrenciesResponseDto CurrencyData = CurrencyManagementService.GetAllCurrencies(SecurityHelper.Context, AuditHelper.Context);
            CommoditiesResponseDto CommoditiesData = CommodityManagementService.GetAllCommodities(SecurityHelper.Context, AuditHelper.Context);
            ItemStatusesResponseDto ItemStatusData = ItemStatusManagementService.GetItemStatuss(SecurityHelper.Context, AuditHelper.Context);
            UsersResponseDto users = userManagementService.GetUsers(SecurityHelper.Context, AuditHelper.Context);
            RSAProvideresResponseDto RSAProvider = RSAProviderManagementService.GetRSAProviders(SecurityHelper.Context, AuditHelper.Context);
            RegionesResponseDto region = RegionManagementService.GetRegions(SecurityHelper.Context, AuditHelper.Context);
            PremiumAddonTypesResponseDto PremiumAddonTypes = PremiumAddonTypeManagementService.GetPremiumAddonTypes(SecurityHelper.Context, AuditHelper.Context);
            PremiumBasedOnesResponseDto PremiumBasedOns = PremiumBasedOnManagementService.GetPremiumBasedOns(SecurityHelper.Context, AuditHelper.Context);
            IdTypesResponseDto idTypeData = IdTypeManagementService.GetAllIdTypes(SecurityHelper.Context, AuditHelper.Context);
            CustomerTypesResponseDto customertypeData = customertypeManagementService.GetAllCustomerTypes(SecurityHelper.Context, AuditHelper.Context);
            MarritalStatusesResponseDto MaritalStatus = customerManagementService.GetMarritalStatuses(SecurityHelper.Context, AuditHelper.Context);
            NationalitiesResponseDto nationalityData = nationalityManagementService.GetAllNationalities(SecurityHelper.Context, AuditHelper.Context);
            OccupationsResponseDto Occupation = customerManagementService.GetOccupations(SecurityHelper.Context, AuditHelper.Context);
            TitlesResponseDto Titles = customerManagementService.GetTitles(SecurityHelper.Context, AuditHelper.Context);
            UsageTypesResponseDto usageTypeData = usageTypeManagementService.GetAllUsageTypes(SecurityHelper.Context, AuditHelper.Context);
            CommodityUsageTypesResponseDto CommodityUsageTypeData = CommodityUsageTypeManagementService.GetCommodityUsageTypes(SecurityHelper.Context, AuditHelper.Context);
            #endregion


            try
            {


                foreach (var item in bulkData)
                {
                    bool isValidRow = true;
                    #region Validation
                    //---------------------------- validation ------------------------------------------

                    #region Customer Validation

                    //----------------------------- Customer -------------------------------------------

                    Guid? countryid = CountryData.Countries.FirstOrDefault(a => a.CountryName.ToLower().Contains(item.Country.ToLower())).Id;
                    if (!(countryid != null && countryid != Guid.Empty))
                    {
                        isValidRow = false;
                        ErrorMsgList.Add("Invalid Country");
                    }
                    Guid? cityid = cityData.Cities.FirstOrDefault(a => a.CityName.ToLower().Contains(item.City.ToLower()) && a.CountryId == countryid).Id;
                    if (!(cityid != null && cityid != Guid.Empty))
                    {
                        isValidRow = false;
                        ErrorMsgList.Add("Invalid City");
                    }

                    Guid? titleId = Titles.Titles.FirstOrDefault(a => a.Name.ToLower().Contains(item.Title.ToLower())).Id;
                    if (!(titleId != null && titleId != Guid.Empty))
                    {
                        isValidRow = false;
                        ErrorMsgList.Add("Invalid Title");
                    }

                    Guid? maritalStatusId = MaritalStatus.MarritalStatuses.FirstOrDefault(a => a.Name.ToLower().Contains(item.MaritalStatus.ToLower())).Id; //.Substring(0, 1)
                    if (!(maritalStatusId != null && maritalStatusId != Guid.Empty))
                    {
                        isValidRow = false;
                        ErrorMsgList.Add("Invalid Marital Status");
                    }

                    Guid? occupationId = Occupation.Occupations.FirstOrDefault(a => a.Name.ToLower().Contains(item.Occupation.ToLower())).Id; //.Substring(0, 1)
                    if (!(occupationId != null && occupationId != Guid.Empty))
                    {
                        isValidRow = false;
                        ErrorMsgList.Add("Invalid Occupation");
                    }

                    //int? customerTypeId =ccupation.Occupations.FirstOrDefault(a => a.Name.ToLower().Contains(item.Occupation.ToLower())).Id; //.Substring(0, 1)
                    //if (!(customerTypeId != null && customerTypeId != Guid.Empty))
                    //{
                    //    isValidRow = false;
                    //    ErrorMsgList.Add("Invalid Occupation");
                    //}

                    //int? occupationId = Occupation.Occupations.FirstOrDefault(a => a.Name.ToLower().Contains(item.Occupation.ToLower())).Id; //.Substring(0, 1)
                    //if (!(occupationId != null && occupationId != Guid.Empty))
                    //{
                    //    isValidRow = false;
                    //    ErrorMsgList.Add("Invalid Occupation");
                    //}

                    //Guid? occupationId = Occupation.Occupations.FirstOrDefault(a => a.Name.ToLower().Contains(item.Occupation.ToLower())).Id; //.Substring(0, 1)
                    //if (!(occupationId != null && occupationId != Guid.Empty))
                    //{
                    //    isValidRow = false;
                    //    ErrorMsgList.Add("Invalid Occupation");
                    //}


                    //----------------------------------------------------------------------------------

                    #endregion

                    #endregion


                    #region Save

                    if (isValidRow)
                    {
                        //------------------------------- Save ---------------------------------------------
                        CustomerRequestDto customer = new CustomerRequestDto();

                        customer.Id = item.IDNo;
                        //customer.NationalityId = item.NationalityId;
                        customer.CountryId = (Guid)countryid;
                        customer.MobileNo = item.MobilePhone;
                        customer.OtherTelNo = item.OtherPhone;
                        customer.FirstName = item.FirstName;
                        customer.LastName = item.LastName;
                        customer.DateOfBirth = item.DateOfBirth;
                        customer.Email = item.Email;
                        //customer.CustomerTypeId = (int)customerTypeId;
                        //customer.UsageTypeId = (int)usageTypeId;
                        customer.CityId = (Guid)cityid;

                        customer.Address1 = item.Address1;
                        customer.Address2 = item.Address2;
                        customer.Address3 = item.Address3;
                        customer.Address4 = item.Address4;
                        customer.IDNo = item.IDNo;
                        //customer.IDTypeId = item.IDTypeId;
                        customer.DLIssueDate = item.DrivingIssueDate;
                        customer.Gender = Convert.ToChar(item.Gender.Substring(0, 1));
                        customer.BusinessName = item.BusinessName;
                        customer.BusinessAddress1 = item.BusinessAddress1;
                        customer.BusinessAddress2 = item.BusinessAddress2;
                        customer.BusinessAddress3 = item.BusinessAddress3;
                        customer.BusinessAddress4 = item.BusinessAddress4;
                        customer.BusinessTelNo = item.BusinessTelephoneNo;
                        customer.TitleId = (Guid)titleId;
                        customer.OccupationId = (Guid)occupationId;
                        customer.MaritalStatusId = (Guid)maritalStatusId;
                        customer.PostalCode = item.PostalCode;
                        customer.Password = "12345";

                        //----------------------------------------------------------------------------------
                    }

                    #endregion
                }
            }
            catch
            {
                return "Error !";
            }


            //--------------------------------------------------------------------------------------

            List<PolicyBulk> PolicyList = data["Policies"].ToObject<List<PolicyBulk>>();

            #region Validate Existing data
            ManufacturesResponseDto ManufacturerData = ManufacturerManagementService.GetAllManufatures(SecurityHelper.Context, AuditHelper.Context);
            MakesResponseDto MakeData = MakeManagementService.GetAllMakes(SecurityHelper.Context, AuditHelper.Context);
            ModelesResponseDto ModelData = ModelManagementService.GetAllModeles(SecurityHelper.Context, AuditHelper.Context);
            ProductsResponseDto productData = productManagementService.GetProducts(SecurityHelper.Context, AuditHelper.Context);
            InsurersResponseDto InsurerData = InsurerManagementService.GetInsurers(SecurityHelper.Context, AuditHelper.Context);
            DealersRespondDto DealerData = DealerManagementService.GetAllDealers(SecurityHelper.Context, AuditHelper.Context);
            DealerLocationsRespondDto DealerLocationData = DealerLocationManagementService.GetAllDealerLocations(SecurityHelper.Context, AuditHelper.Context);
            ManufacturerWarrantiesResponseDto ManufacturerWarrantyData = ManufacturerWarrantyManagementService.GetManufacturerWarranties(SecurityHelper.Context, AuditHelper.Context);
            CustomersResponseDto customerData = customerManagementService.GetCustomers(SecurityHelper.Context, AuditHelper.Context);
            ReinsurersResponseDto ReinsurerData = ReinsurerManagementService.GetReinsurers(SecurityHelper.Context, AuditHelper.Context);
            WarrantyTypesResponseDto WarrantyTypeData = WarrantyTypeManagementService.GetWarrantyTypes(SecurityHelper.Context, AuditHelper.Context);
            ContractsResponseDto ContractData = ContractManagementService.GetContracts(SecurityHelper.Context, AuditHelper.Context);
            CylinderCountsResponseDto cylinderCountData = cylinderCountManagementService.GetCylinderCounts(SecurityHelper.Context, AuditHelper.Context);
            VehicleBodyTypesResponseDto vehicleBodyTypeData = vehicleBodyTypeManagementService.GetVehicleBodyTypes(SecurityHelper.Context, AuditHelper.Context);
            DriveTypesResponseDto driveTypeData = driveTypeManagementService.GetDriveTypes(SecurityHelper.Context, AuditHelper.Context);
            EngineCapacitiesResponseDto engineCapacityData = engineCapacityManagementService.GetEngineCapacities(SecurityHelper.Context, AuditHelper.Context);
            FuelTypesResponseDto fuelTypeData = fuelTypeManagementService.GetFuelTypes(SecurityHelper.Context, AuditHelper.Context);
            TransmissionTypesResponseDto TransmissionTypeData = TransmissionTypeManagementService.GetTransmissionTypes(SecurityHelper.Context, AuditHelper.Context);
            VehicleAspirationTypesResponseDto VehicleAspirationTypeData = VehicleAspirationTypeManagementService.GetVehicleAspirationTypes(SecurityHelper.Context, AuditHelper.Context);
            VariantsResponseDto VariantData = VariantManagementService.GetVariants(SecurityHelper.Context, AuditHelper.Context);
            VehicleAllDetailsResponseDto VehicleDetailsData = VehicleDetailsManagementService.GetVehicleAllDetails(SecurityHelper.Context, AuditHelper.Context);
            ExtensionTypesResponseDto ExtensionTypes = ExtensionTypeManagementService.GetExtensionTypes(SecurityHelper.Context, AuditHelper.Context);
            ContractExtensionsResponseDto ContractExtensionData = ContractManagementService.GetContractExtensions(SecurityHelper.Context, AuditHelper.Context);
            BrownAndWhiteAllDetailsResponseDto BrownAndWhiteDetailsData = BrownAndWhiteDetailsManagementService.GetBrownAndWhiteAllDetails(SecurityHelper.Context, AuditHelper.Context);
            #endregion

            foreach (var item in PolicyList)
            {
                try
                {
                    DateTime d = DateTime.Parse(item.DateOfBirth);
                    d = DateTime.Parse(item.DealEndDate);
                    d = DateTime.Parse(item.DealStartDate);
                    d = DateTime.Parse(item.ManufacturerWarantyStartDate);
                    d = DateTime.Parse(item.ModelRiskStartDate);
                    d = DateTime.Parse(item.PolicyEndDate);
                    d = DateTime.Parse(item.PolicySoldDate);
                    d = DateTime.Parse(item.PolicyStartDate);
                    d = DateTime.Parse(item.PurchaseDate);
                    if (item.CommodityType == "Äutomobile")
                        d = DateTime.Parse(item.VehicleRegistrationDate);
                    d = DateTime.Parse(item.CustomerIdIssueDate);
                }
                catch
                {
                    return "Date fields should be valid";
                }
                try
                {
                    Decimal val = Convert.ToDecimal(item.BasicPremium);
                    val = Convert.ToDecimal(item.DealerPayment);
                    val = Convert.ToDecimal(item.DealerPrice);
                    val = Convert.ToDecimal(item.Discount);
                    if (item.CommodityType == "Äutomobile")
                    {
                        val = Convert.ToDecimal(item.FourByFourPremium);
                        val = Convert.ToDecimal(item.SportsPremium);
                    }
                    val = Convert.ToDecimal(item.GrossPremium);
                    val = Convert.ToDecimal(item.ItemPrice);
                    val = Convert.ToDecimal(item.MaximumPremium);
                    val = Convert.ToDecimal(item.MinimumPremium);
                    val = Convert.ToDecimal(item.OtherPremium);
                    val = Convert.ToDecimal(item.Premium);
                    val = Convert.ToDecimal(item.PremiumTotal);
                    val = Convert.ToDecimal(item.ClaimLimitation);
                    val = Convert.ToDecimal(item.LiabilityLimitation);
                    val = Convert.ToDecimal(item.AdditionalPremium);
                    val = Convert.ToDecimal(item.CustomerPayment);
                }
                catch
                {
                    return "Currency fields should be valid";

                }
                try
                {
                    int val = Convert.ToInt32(item.CategoryLength);
                    if (item.CommodityType == "Automobile")
                    {
                        val = Convert.ToInt32(item.ManufacturerWarantyKm);
                        val = Convert.ToInt32(item.ExtensionKm);
                        val = Convert.ToInt32(item.VehicleCylinderCount);
                        val = Convert.ToInt32(item.VehicleModelYear);
                    }
                    else
                    {
                        val = Convert.ToInt32(item.ElectronicModelYear);
                        val = Convert.ToInt32(item.ExtensionHours);
                    }
                    val = Convert.ToInt32(item.ExtensionMonth);
                    val = Convert.ToInt32(item.ManufacturerWarantyMonth);
                    val = Convert.ToInt32(item.HrsUsedAtPolicySale);
                }
                catch
                {
                    return "Please enter Integer for the following fields Electronic Model Year / Extension Hours / Extension Km / Extension Month / Manufacturer Waranty Km / Manufacturer Waranty Measure Type / Manufacturer Waranty Month / Vehicle Cylinder Count / Vehicle Model Year / Category Length / Hrs Used At Policy Sale";

                }
                if (!EmailValidator(item.CustomerEmail) || !EmailValidator(item.DealerBranchSalesEmail) || !EmailValidator(item.DealerBranchServiceEmail))
                {
                    return "Please enter valid emails";
                }
                if (cityData.Cities.FindAll(c => c.CityName == item.CustomerCity || c.CityName == item.DealerBranchCity).Count == 0)
                {
                    return "Please Enter a Valid City";
                }
                if (CountryData.Countries.FindAll(c => c.CountryName == item.CustomerCountry ||
                    c.CountryName == item.DealerCountry ||
                    c.CountryName == item.InsurerCountry ||
                    c.CountryName == item.ManufacturerWarantyCountry
                ).Count == 0)
                {
                    return "Please Enter a Valid Country";
                }
                if (item.CommodityType == "Automobile" && CountryData.Countries.FindAll(c => c.CountryName == item.VehiclelCountryOfOrigine).Count == 0)
                {
                    return "Please Enter a Valid Country";
                }
                if (item.CommodityType == "Electronic" && CountryData.Countries.FindAll(c => c.CountryName == item.ElectronicCountryOfOrigine).Count == 0)
                {
                    return "Please Enter a Valid Country";
                }
                if (CurrencyData.Currencies.FindAll(c => c.Code == item.DealerCurrency ||
                    c.Code == item.DealerPaymentCurrencyType ||
                    c.Code == item.PremiumCurrencyType).Count == 0)
                {
                    return "Please Enter a Valid Currency";
                }
                if (MaritalStatus.MarritalStatuses.FindAll(c => c.Name == item.CustomerMaritalStatus || c.Name == "").Count == 0)
                {
                    return "Please Enter a Valid marital status or enter blank";
                }
                if (Occupation.Occupations.FindAll(c => c.Name == item.CustomerOccupation || c.Name == "").Count == 0)
                {
                    return "Please Enter a Valid Occupation or enter blank";
                }
                if (Titles.Titles.FindAll(c => c.Name == item.CustomerTitle || c.Name == "").Count == 0)
                {
                    return "Please Enter a Valid Title or enter blank";
                }
                if (customertypeData.CustomerTypes.FindAll(c => c.CustomerTypeName == item.CustomerType).Count == 0)
                {
                    return "Please Enter a Valid Customer Type: Corporate / Individual";
                }
                if (usageTypeData.UsageTypes.FindAll(c => c.UsageTypeName == item.UsageType).Count == 0)
                {
                    return "Please Enter a Valid Customer Type: Private / Commercial";
                }
                if (nationalityData.Nationalities.FindAll(c => c.NationalityName == item.Nationality).Count == 0)
                {
                    return "Please Enter a Valid Nationality";
                }
                if (CommodityUsageTypeData.CommodityUsageTypes.FindAll(c => c.Name == item.CommodityUsageType).Count == 0)
                {
                    return "Please Enter a Valid Commodity Usage Types: Residence / Commercial";
                }
                if (item.CustomerGender.ToLower() != "f" && item.CustomerGender.ToLower() != "m")
                {
                    return "Please Enter F / M for Gender";
                }
                if (!(
                    (item.AutoApproval.ToLower() == "yes" || item.AutoApproval.ToLower() == "no") &&
                     (item.AutoRenewal.ToLower() == "yes" || item.AutoRenewal.ToLower() == "no") &&
                     (item.PromotionalDeal.ToLower() == "yes" || item.PromotionalDeal.ToLower() == "no") &&
                     (item.DiscountAvailable.ToLower() == "yes" || item.DiscountAvailable.ToLower() == "no") &&
                     (item.SpecialDeal.ToLower() == "yes" || item.SpecialDeal.ToLower() == "no") &&
                     (item.PartialPayment.ToLower() == "yes" || item.PartialPayment.ToLower() == "no") &&
                     (item.DealerPolicy.ToLower() == "yes" || item.DealerPolicy.ToLower() == "no")
                    ))
                {
                    return "Please Enter Yes / No for fields 'Auto Approval / Auto Renewal / Customer Payment / Promotional Deal / Discount Available / Special Deal / Partial Payment / Dealer Policy' ";
                }
                if (item.Status.ToLower() != "new" && item.Status.ToLower() != "used")
                {
                    return "Status should be New / Used";
                }
                if (item.CommodityType.ToLower() != "automobile" && item.CommodityType.ToLower() != "electronic")
                {
                    return "Commodity Type should be Automobile / Electronic";
                }
                if (users.Users.FindAll(c => c.FirstName + ' ' + c.LastName == item.SalesPerson).Count == 0)
                {
                    return "Please Enter a Valid sales person";
                }
                if (idTypeData.IdTypes.FindAll(c => c.IdTypeName == item.CustomerIdType).Count == 0)
                {
                    return "Please Enter a Valid Id Type";
                }

            }//Validation
            foreach (var item in PolicyList)
            {
                Guid CommodityTypeId = CommoditiesData.Commmodities.Find(c => c.CommodityTypeDescription == item.CommodityType).CommodityTypeId;
                List<Guid> CommodityTypes = new List<Guid>();
                CommodityTypes.Add(CommodityTypeId);

                Guid ItemStatus = ItemStatusData.ItemStatuss.Find(i => i.Status == item.Status).Id;

                #region Validate Existing data
                CommodityCategoriesRespondDto CommodityCategoryData = CommodityCategoryManagementService.GetCommodityCategories(CommodityTypeId, SecurityHelper.Context, AuditHelper.Context);
                #endregion

                #region Common Data Save

                var Category = new Guid();
                if (CommodityCategoryData.CommodityCategories.FindAll(c => c.CommodityCategoryDescription == item.CategoryName).Count == 0)
                {
                    Category = CommodityCategoryManagementService.AddCommodityCategory(new CommodityCategoryRequestDto()
                    {
                        CommodityCategoryCode = "",
                        CommodityCategoryDescription = item.CategoryName,
                        CommodityTypeId = CommodityTypeId,
                        Length = Convert.ToInt32(item.CategoryLength)
                    }, SecurityHelper.Context, AuditHelper.Context).CommodityCategoryId;

                }
                else
                {
                    Category = CommodityCategoryData.CommodityCategories.Find(c => c.CommodityCategoryDescription == item.CategoryName).CommodityCategoryId;
                }

                var Manufacturer = new Guid();
                if (ManufacturerData.Manufactures.FindAll(m => m.ManufacturerName == item.Manufacturer).Count == 0)
                {
                    Manufacturer = ManufacturerManagementService.AddManufacturer(new ManufacturerRequestDto()
                    {
                        ComodityTypes = CommodityTypes,
                        ManufacturerName = item.Manufacturer,
                        ManufacturerCode = item.ManufacturerCode,
                        IsWarrentyGiven = false,
                        IsActive = true
                    }, SecurityHelper.Context, AuditHelper.Context).Id;

                }
                else
                {
                    Manufacturer = ManufacturerData.Manufactures.Find(m => m.ManufacturerName == item.Manufacturer).Id;
                }

                List<Guid> Makes = new List<Guid>();
                var Make = new Guid();
                if (MakeData.Makes.FindAll(m => m.MakeName == item.MakeName).Count == 0)
                {
                    Make = MakeManagementService.AddMake(new MakeRequestDto()
                    {
                        CommodityTypeId = CommodityTypeId,
                        IsActive = true,
                        MakeCode = item.MakeCode,
                        MakeName = item.MakeName,
                        ManufacturerId = Manufacturer
                    }, SecurityHelper.Context, AuditHelper.Context).Id;
                    Makes.Add(Make);
                }
                else
                {
                    Make = MakeData.Makes.Find(m => m.MakeName == item.MakeName).Id;
                    Makes.Add(Make);
                }

                var Model = new Guid();
                if (ModelData.Modeles.FindAll(m => m.ModelName == item.ModelName).Count == 0)
                {
                    Model = ModelManagementService.AddModel(new ModelRequestDto()
                    {
                        AdditionalPremium = item.AdditionalPremium != "" ? true : false,
                        CategoryId = Category,
                        ContryOfOrigineId = CountryData.Countries.Find(c => c.CountryName == item.VehiclelCountryOfOrigine).Id,
                        IsActive = true,
                        MakeId = Make,
                        ModelCode = item.ModelCode,
                        ModelName = item.ModelName,
                        WarantyGiven = false,
                        NoOfDaysToRiskStart = Convert.ToInt32(item.ModelRiskStartDate)
                    }, SecurityHelper.Context, AuditHelper.Context).Id;
                }
                else
                {
                    Model = ModelData.Modeles.Find(m => m.ModelName == item.ModelName).Id;
                }

                List<Guid> Products = new List<Guid>();
                var Product = new Guid();
                if (productData.Products.FindAll(m => m.Productname == item.ProductName).Count == 0)
                {
                    Product = productManagementService.AddProduct(new ProductRequestDto()
                    {
                        CommodityTypeId = CommodityTypeId,
                        Isactive = true,
                        Productcode = item.ProductCode,
                        Productname = item.ProductName
                    }, SecurityHelper.Context, AuditHelper.Context).Id;
                    Products.Add(Product);
                }
                else
                {
                    Product = productData.Products.Find(m => m.Productname == item.ProductName).Id;
                    Products.Add(Product);
                }

                List<Guid> Countries = new List<Guid>();
                try
                {
                    Countries.Add(CountryData.Countries.Find(c => c.CountryCode == item.InsurerCountry).Id);
                }
                catch
                {
                }
                var Insurer = new Guid();
                if (InsurerData.Insurers.FindAll(m => m.InsurerFullName == item.InsurerName || m.InsurerShortName == item.InsurerName).Count == 0)
                {
                    Insurer = InsurerManagementService.AddInsurer(new InsurerRequestDto()
                    {
                        CommodityTypes = CommodityTypes,
                        InsurerCode = item.InsurerCode,
                        InsurerFullName = item.InsurerName,
                        Products = Products,
                        Countries = Countries
                    }, SecurityHelper.Context, AuditHelper.Context).Id;
                }
                else
                {
                    Insurer = InsurerData.Insurers.Find(m => m.InsurerFullName == item.InsurerName || m.InsurerShortName == item.InsurerName).Id;
                }

                var Dealer = new Guid();
                Guid DealerCountry = new Guid();
                try
                {
                    DealerCountry = CountryData.Countries.Find(c => c.CountryCode == item.DealerCountry).Id;
                }
                catch { }
                Guid DealerCurrency = new Guid();
                try
                {
                    DealerCurrency = CurrencyData.Currencies.Find(c => c.Code == item.DealerCurrency).Id;
                }
                catch { }
                if (DealerData.Dealers.FindAll(m => m.DealerName == item.DealerName).Count == 0)
                {
                    DealerManagementService.AddDealer(new DealerRequestDto()
                    {
                        CommodityTypeId = CommodityTypeId,
                        CountryId = DealerCountry,
                        CurrencyId = DealerCurrency,
                        DealerName = item.DealerName,
                        InsurerId = Insurer,
                        IsActive = true,
                        Makes = Makes,
                        Type = "Dealer"
                    }, SecurityHelper.Context, AuditHelper.Context);
                }
                else
                {
                    Dealer = DealerData.Dealers.Find(m => m.DealerName == item.DealerName).Id;
                }

                var Location = new Guid();
                if (DealerLocationData.DealerLocations.FindAll(m => m.Location == item.DealerBranch).Count == 0)
                {
                    Location = DealerLocationManagementService.AddDealerLocation(new DealerLocationRequestDto()
                    {
                        DealerId = Dealer,
                        HeadOfficeBranch = true,
                        Location = item.DealerBranch,
                        CityId = cityData.Cities.Find(c => c.CityName == item.DealerBranchCity).Id,
                        SalesContactPerson = item.DealerBranchSalesContractPerson,
                        SalesEmail = item.DealerBranchSalesEmail,
                        SalesFax = item.DealerBranchSalesFax,
                        SalesTelephone = item.DealerBranchSalesTelephoneNo,
                        ServiceContactPerson = item.DealerBranchServiceContractPerson,
                        ServiceEmail = item.DealerBranchServiceEmail,
                        ServiceFax = item.DealerBranchServiceFax,
                        ServiceTelephone = item.DealerBranchServiceTelephoneNo,
                    }, SecurityHelper.Context, AuditHelper.Context).Id;
                }
                else
                {
                    Location = DealerLocationData.DealerLocations.Find(m => m.Location == item.DealerBranch).Id;
                }

                var ManWarranty = new Guid();
                if (ManufacturerWarrantyData.ManufacturerWarranties.FindAll(m => m.WarrantyName == item.ManufacturerWaranty && m.ModelId == Model).Count == 0)
                {
                    ManWarranty = ManufacturerWarrantyManagementService.AddManufacturerWarranty(new ManufacturerWarrantyRequestDto()
                    {
                        ApplicableFrom = DateTime.Parse(item.ManufacturerWarantyStartDate),
                        MakeId = Make,
                        CountryId = CountryData.Countries.Find(c => c.CountryName == item.ManufacturerWarantyCountry).Id,
                        ModelId = Model,
                        WarrantyKm = Convert.ToInt32(item.ManufacturerWarantyKm),
                        WarrantyMonths = Convert.ToInt32(item.ManufacturerWarantyMonth),
                        WarrantyName = item.ManufacturerWaranty
                    }, SecurityHelper.Context, AuditHelper.Context).Id;
                }
                else
                {
                    ManWarranty = ManufacturerWarrantyData.ManufacturerWarranties.Find(m => m.WarrantyName == item.ManufacturerWaranty && m.ModelId == Model).Id;
                }

                var Customer = new Guid();
                if (customerData.Customers.FindAll(m => m.FirstName + " " + m.LastName == item.CustomerFirstName + " " + item.CustomerLastName).Count == 0)
                {
                    Customer = Guid.Parse(customerManagementService.AddCustomer(new CustomerRequestDto()
                    {
                        Id = "00000000-0000-0000-0000-000000000000",
                        Address1 = item.CustomerAddress,
                        BusinessAddress1 = item.BusinessAddress,
                        BusinessName = item.BusinessName,
                        BusinessTelNo = item.BusinessPhoneNo,
                        CityId = cityData.Cities.Find(c => c.CityName == item.CustomerCity).Id,
                        CountryId = CountryData.Countries.Find(c => c.CountryName == item.CustomerCountry).Id,
                        CustomerTypeId = customertypeData.CustomerTypes.Find(c => c.CustomerTypeName == item.CustomerType).Id,
                        DateOfBirth = DateTime.Parse(item.DateOfBirth),
                        DLIssueDate = DateTime.Parse(item.CustomerIdIssueDate),
                        Email = item.CustomerEmail,
                        FirstName = item.CustomerFirstName,
                        Gender = item.CustomerGender == "Female" && item.CustomerGender == "F" ? 'F' : 'M',
                        IDNo = item.CustomerIdNo,
                        IDTypeId = idTypeData.IdTypes.Find(i => i.IdTypeName == item.CustomerIdType).Id,
                        IsActive = true,
                        LastName = item.CustomerLastName,
                        MaritalStatusId = MaritalStatus.MarritalStatuses.Find(m => m.Name == item.CustomerMaritalStatus).Id,
                        MobileNo = item.CustomerMobileNo,
                        NationalityId = nationalityData.Nationalities.Find(n => n.NationalityName == item.Nationality).Id,
                        OccupationId = Occupation.Occupations.Find(o => o.Name == item.CustomerOccupation).Id,
                        OtherTelNo = "",
                        TitleId = Titles.Titles.Find(t => t.Name == item.CustomerTitle).Id,
                        UsageTypeId = usageTypeData.UsageTypes.Find(u => u.UsageTypeName == item.UsageType).Id
                    }, SecurityHelper.Context, AuditHelper.Context).Id);
                }
                else
                {
                    Customer = Guid.Parse(customerData.Customers.Find(m => m.FirstName + " " + m.LastName == item.CustomerFirstName + " " + item.CustomerLastName).Id);
                }

                var Reinsurer = new Guid();
                if (ReinsurerData.Reinsurers.FindAll(m => m.ReinsurerName == item.ReinsurerName).Count == 0)
                {
                    Reinsurer = ReinsurerManagementService.AddReinsurer(new ReinsurerRequestDto()
                    {
                        ReinsurerName = item.ReinsurerName,
                        ReinsurerCode = item.ReinsurerCode
                    }, SecurityHelper.Context, AuditHelper.Context).Id;
                }
                else
                {
                    Reinsurer = ReinsurerData.Reinsurers.Find(m => m.ReinsurerName == item.ReinsurerName).Id;
                }

                var WarrantyType = new Guid();
                if (WarrantyTypeData.WarrantyTypes.FindAll(m => m.WarrantyTypeDescription == item.CoverType).Count == 0)
                {
                    WarrantyType = WarrantyTypeManagementService.AddWarrantyType(new WarrantyTypeRequestDto()
                    {
                        WarrantyTypeDescription = item.CoverType,
                    }, SecurityHelper.Context, AuditHelper.Context).Id;
                }
                else
                {
                    WarrantyType = WarrantyTypeData.WarrantyTypes.Find(m => m.WarrantyTypeDescription == item.CoverType).Id;
                }

                var Contract = new Guid();
                if (ContractData.Contracts.FindAll(m => m.DealName == item.ContractDealName).Count == 0)
                {
                    Contract = ContractManagementService.AddContract(new ContractRequestDto()
                    {
                        ClaimLimitation = Convert.ToInt32(item.ClaimLimitation),
                        CommodityTypeId = CommodityTypeId,
                        CommodityUsageTypeId = CommodityUsageTypeData.CommodityUsageTypes.Find(c => c.Name == item.CommodityUsageType).Id,
                        CountryId = CountryData.Countries.Find(c => c.CountryName == item.CustomerCountry).Id,
                        DealerId = Dealer,
                        DealName = item.DealerName,
                        DealType = item.ContractDealType,
                        DiscountAvailable = item.DiscountAvailable == "Yes" ? true : false,
                        EndDate = DateTime.Parse(item.DealEndDate),
                        GrossPremium = Convert.ToDecimal(item.GrossPremium),
                        InsurerId = Insurer,
                        IsActive = true,
                        IsAutoRenewal = item.AutoRenewal == "Yes" ? true : false,
                        IsPromotional = item.PromotionalDeal == "Yes" ? true : false,
                        ItemStatusId = ItemStatus,
                        LiabilityLimitation = Convert.ToDecimal(item.LiabilityLimitation),
                        PremiumTotal = Convert.ToDecimal(item.PremiumTotal),
                        ProductId = Product,
                        ReinsurerId = Reinsurer,
                        StartDate = DateTime.Parse(item.DealEndDate),
                        WarrantyTypeId = WarrantyType
                    }, SecurityHelper.Context, AuditHelper.Context).Id;
                }
                else
                {
                    Contract = ContractData.Contracts.Find(m => m.DealName == item.ContractDealName).Id;
                }

                #endregion

                if (item.CommodityType == "Automobile")
                {
                    var CylinderCount = new Guid();
                    if (cylinderCountData.CylinderCounts.FindAll(m => m.Count == item.VehicleCylinderCount).Count == 0)
                    {
                        CylinderCount = cylinderCountManagementService.AddCylinderCount(new CylinderCountRequestDto()
                        {
                            Count = item.VehicleCylinderCount
                        }, SecurityHelper.Context, AuditHelper.Context).Id;
                    }
                    else
                    {
                        CylinderCount = cylinderCountData.CylinderCounts.Find(m => m.Count == item.VehicleCylinderCount).Id;
                    }

                    var BodyType = new Guid();
                    if (vehicleBodyTypeData.VehicleBodyTypes.FindAll(m => m.VehicleBodyTypeDescription == item.VehicleBodyType).Count == 0)
                    {
                        BodyType = vehicleBodyTypeManagementService.AddVehicleBodyType(new VehicleBodyTypeRequestDto()
                        {
                            VehicleBodyTypeDescription = item.VehicleBodyType
                        }, SecurityHelper.Context, AuditHelper.Context).Id;
                    }
                    else
                    {
                        BodyType = vehicleBodyTypeData.VehicleBodyTypes.Find(m => m.VehicleBodyTypeDescription == item.VehicleBodyType).Id;
                    }

                    var DriveType = new Guid();
                    if (driveTypeData.DriveTypes.FindAll(m => m.Type == item.VehicleDriveType).Count == 0)
                    {
                        DriveType = driveTypeManagementService.AddDriveType(new DriveTypeRequestDto()
                        {
                            DriveTypeDescription = item.VehicleDriveType
                        }, SecurityHelper.Context, AuditHelper.Context).Id;
                    }
                    else
                    {
                        DriveType = driveTypeData.DriveTypes.Find(m => m.Type == item.VehicleDriveType).Id;
                    }

                    var EngineCapacity = new Guid();
                    if (engineCapacityData.EngineCapacities.FindAll(m => m.EngineCapacityNumber.ToString() == item.VehicleEngineCapacity).Count == 0)
                    {
                        EngineCapacity = engineCapacityManagementService.AddEngineCapacity(new EngineCapacityRequestDto()
                        {
                            EngineCapacityNumber = Convert.ToDecimal(item.VehicleEngineCapacity),
                            MesureType = item.EngineCapacityMeasureType
                        }, SecurityHelper.Context, AuditHelper.Context).Id;
                    }
                    else
                    {
                        EngineCapacity = engineCapacityData.EngineCapacities.Find(m => m.EngineCapacityNumber.ToString() == item.VehicleEngineCapacity).Id;
                    }

                    var FuelType = new Guid();
                    if (fuelTypeData.FuelTypes.FindAll(m => m.FuelTypeDescription == item.VehicleFuelType).Count == 0)
                    {
                        FuelType = fuelTypeManagementService.AddFuelType(new FuelTypeRequestDto()
                        {
                            FuelTypeDescription = item.VehicleFuelType
                        }, SecurityHelper.Context, AuditHelper.Context).FuelTypeId;
                    }
                    else
                    {
                        FuelType = fuelTypeData.FuelTypes.Find(m => m.FuelTypeDescription == item.VehicleFuelType).FuelTypeId;
                    }

                    List<string> Tech = new List<string>();
                    Tech.Add(TransmissionTechData.TransmissionTechnologies.Find(t => t.Name == item.VehicleTransmissionTechnology).Name);
                    var Transmission = new Guid();
                    if (TransmissionTypeData.TransmissionTypes.FindAll(m => m.TransmissionTypeCode == item.VehicleTransmission).Count == 0)
                    {
                        Transmission = TransmissionTypeManagementService.AddTransmissionType(new TransmissionTypeRequestDto()
                        {
                            TransmissionTypeCode = item.VehicleTransmission,
                            Technology = Tech,
                        }, SecurityHelper.Context, AuditHelper.Context).Id;
                    }
                    else
                    {
                        Transmission = TransmissionTypeData.TransmissionTypes.Find(m => m.TransmissionTypeCode == item.VehicleTransmission).Id;
                    }

                    var Aspiration = new Guid();
                    if (VehicleAspirationTypeData.VehicleAspirationTypes.FindAll(m => m.AspirationTypeCode == item.VehicleAspiration).Count == 0)
                    {
                        Aspiration = VehicleAspirationTypeManagementService.AddVehicleAspirationType(new VehicleAspirationTypeRequestDto()
                        {
                            AspirationTypeCode = item.VehicleAspiration
                        }, SecurityHelper.Context, AuditHelper.Context).Id;
                    }
                    else
                    {
                        Aspiration = VehicleAspirationTypeData.VehicleAspirationTypes.Find(m => m.AspirationTypeCode == item.VehicleAspiration).Id;
                    }

                    List<Guid> Aspirations = new List<Guid>();
                    Aspirations.Add(Aspiration);
                    List<Guid> BodyTypes = new List<Guid>();
                    BodyTypes.Add(BodyType);
                    List<Guid> Countrys = new List<Guid>();
                    Countrys.Add(CountryData.Countries.Find(c => c.CountryName == item.VehiclelCountryOfOrigine).Id);
                    List<Guid> DriveTypes = new List<Guid>();
                    DriveTypes.Add(DriveType);
                    List<Guid> FuelTypes = new List<Guid>();
                    FuelTypes.Add(FuelType);
                    List<Guid> Transmissions = new List<Guid>();
                    Transmissions.Add(Transmission);

                    var Variant = new Guid();
                    if (VariantData.Variants.FindAll(m => m.VariantName == item.VehicleVariant).Count == 0)
                    {
                        Variant = VariantManagementService.AddVariant(new VariantRequestDto()
                        {
                            Aspirations = Aspirations,
                            BodyCode = "",
                            BodyTypes = BodyTypes,
                            CommodityTypeId = CommodityTypeId,
                            Countrys = Countrys,
                            CylinderCountId = CylinderCount,
                            DriveTypes = DriveTypes,
                            EngineCapacityId = EngineCapacity,
                            FromModelYear = 0,
                            FuelTypes = FuelTypes,
                            IsActive = true,
                            ModelId = Model,
                            ToModelYear = 0,
                            Transmissions = Transmissions,
                            VariantName = item.VehicleVariant
                        }, SecurityHelper.Context, AuditHelper.Context).Id;
                    }
                    else
                    {
                        Variant = VariantData.Variants.Find(m => m.VariantName == item.VehicleVariant).Id;
                    }

                    var Vehicle = new Guid();
                    if (VehicleDetailsData.VehicleAllDetails.FindAll(m => m.VINNo == item.VehicleVinNo).Count == 0)
                    {
                        Vehicle = VehicleDetailsManagementService.AddVehicleDetails(new VehicleDetailsRequestDto()
                        {
                            AspirationId = Aspiration,
                            BodyTypeId = BodyType,
                            CategoryId = Category,
                            CommodityUsageTypeId = CommodityUsageTypeData.CommodityUsageTypes.Find(c => c.Name == item.CommodityUsageType).Id,
                            CylinderCountId = CylinderCount,
                            DealerPrice = Convert.ToDecimal(item.DealerPrice),
                            DriveTypeId = DriveType,
                            EngineCapacityId = EngineCapacity,
                            FuelTypeId = FuelType,
                            ItemPurchasedDate = DateTime.Parse(item.PurchaseDate),
                            ItemStatusId = ItemStatus,
                            MakeId = Make,
                            ModelId = Model,
                            ModelYear = item.VehicleModelYear,
                            PlateNo = item.VehiclePlateNo,
                            TransmissionId = Transmission,
                            VehiclePrice = Convert.ToDecimal(item.ItemPrice),
                            VINNo = item.VehicleVinNo,
                            Variant = Variant
                        }, SecurityHelper.Context, AuditHelper.Context).Id;
                    }
                    else
                    {
                        Vehicle = VehicleDetailsData.VehicleAllDetails.Find(m => m.VINNo == item.VehicleVinNo).Id;
                    }

                    var ExtensionType = new Guid();
                    if (ExtensionTypes.ExtensionTypes.FindAll(m => m.ExtensionName == item.ExtensionName).Count == 0)
                    {
                        ExtensionType = ExtensionTypeManagementService.AddExtensionType(new ExtensionTypeRequestDto()
                        {
                            CommodityTypeId = CommodityTypeId,
                            ExtensionName = item.ExtensionName,
                            Hours = Convert.ToInt32(item.ExtensionHours),
                            Km = Convert.ToInt32(item.ExtensionKm),
                            Month = Convert.ToInt32(item.ExtensionMonth),
                            ProductId = Product
                        }, SecurityHelper.Context, AuditHelper.Context).Id;
                    }
                    else
                    {
                        ExtensionType = ExtensionTypes.ExtensionTypes.Find(m => m.ExtensionName == item.ExtensionName).Id;
                    }

                    List<Guid> CylinderCounts = new List<Guid>();
                    CylinderCounts.Add(CylinderCount);
                    List<Guid> EngineCapacities = new List<Guid>();
                    EngineCapacities.Add(EngineCapacity);
                    List<Guid> Models = new List<Guid>();
                    Models.Add(Model);

                    List<ContractExtensionsPremiumAddonRequestDto> addons = new List<ContractExtensionsPremiumAddonRequestDto>();
                    addons.Add(new ContractExtensionsPremiumAddonRequestDto()
                    {
                        PremiumAddonTypeId = PremiumAddonTypes.PremiumAddonTypes.Find(p => p.Description == "Basic Premium").Id,
                        Value = Convert.ToInt32(item.BasicPremium)
                    });
                    addons.Add(new ContractExtensionsPremiumAddonRequestDto()
                    {
                        PremiumAddonTypeId = PremiumAddonTypes.PremiumAddonTypes.Find(p => p.Description == "Other Premium").Id,
                        Value = Convert.ToInt32(item.OtherPremium)
                    });
                    addons.Add(new ContractExtensionsPremiumAddonRequestDto()
                    {
                        PremiumAddonTypeId = PremiumAddonTypes.PremiumAddonTypes.Find(p => p.Description == "4x4 premium").Id,
                        Value = Convert.ToInt32(item.FourByFourPremium)
                    });
                    addons.Add(new ContractExtensionsPremiumAddonRequestDto()
                    {
                        PremiumAddonTypeId = PremiumAddonTypes.PremiumAddonTypes.Find(p => p.Description == "Sport Premium").Id,
                        Value = Convert.ToInt32(item.SportsPremium)
                    });
                    addons.Add(new ContractExtensionsPremiumAddonRequestDto()
                    {
                        PremiumAddonTypeId = PremiumAddonTypes.PremiumAddonTypes.Find(p => p.Description == "Additional Premium").Id,
                        Value = Convert.ToInt32(item.AdditionalPremium)
                    });

                    var ContractExtension = new Guid();
                    Guid PremiumBasedOnsId = new Guid();
                    try
                    {
                        PremiumBasedOnsId = PremiumBasedOns.PremiumBasedOns.Find(p => p.Description == item.PremiumBasedOnDescription).Id;
                    }
                    catch { }
                    Guid CurrencyDataId = new Guid();
                    try
                    {
                        CurrencyDataId = CurrencyData.Currencies.Find(c => c.Code == item.PremiumCurrencyType).Id;
                    }
                    catch { }
                    Guid RegionId = new Guid();
                    try
                    {
                        RegionId = region.Regions.Find(r => r.RegionName == item.Region).Id;
                    }
                    catch { }
                    Guid RSAProviderId = new Guid();
                    try
                    {
                        RSAProviderId = RSAProvider.RSAProviders.Find(r => r.ProviderName == item.RSAProviderName).Id;
                    }
                    catch { }
                    if (ContractExtensionData.ContractExtensions.FindAll(m => m.ExtensionTypeId == ExtensionType && m.ContractId == Contract).Count == 0)
                    {
                        ContractExtension = ContractManagementService.AddContractExtensions(new ContractExtensionsRequestDto()
                        {
                            ContractId = Contract,
                            CylinderCounts = CylinderCounts,
                            EngineCapacities = EngineCapacities,
                            ExtensionTypeId = ExtensionType,
                            GrossPremium = Convert.ToDecimal(item.GrossPremium),
                            Makes = Makes,
                            ManufacturerWarranty = item.ManufacturerWaranty == "Yes" ? true : false,
                            MaxNett = Convert.ToDecimal(item.MaximumPremium),
                            MinNett = Convert.ToDecimal(item.MinimumPremium),
                            Modeles = Models,
                            PremiumAddones = addons,
                            PremiumBasedOnIdNett = PremiumBasedOnsId,
                            PremiumCurrencyId = CurrencyDataId,
                            PremiumTotal = Convert.ToDecimal(item.PremiumTotal),
                            WarrantyTypeId = WarrantyType,
                            RegionId = RegionId,
                            RSAProviderId = RSAProviderId//,
                            // Rate = Convert.ToDecimal(item.RateperAnum)
                        }, SecurityHelper.Context, AuditHelper.Context).Id;
                    }
                    else
                    {
                        ContractExtension = ContractExtensionData.ContractExtensions.Find(m => m.ExtensionTypeId == ExtensionType && m.ContractId == Contract).Id;
                    }

                    PolicyBundleRequestDto Bundle = PolicyManagementService.AddPolicyBundle(new PolicyBundleRequestDto()
                    {
                        Comment = item.Comment,
                        CommodityTypeId = CommodityTypeId,
                        ContractId = Contract,
                        CoverTypeId = WarrantyType,
                        CustomerId = Customer,
                        CustomerPayment = Convert.ToDecimal(item.CustomerPayment),
                        CustomerPaymentCurrencyTypeId = CurrencyData.Currencies.Find(c => c.Code == item.CustomerPaymentCurrencyType).Id,
                        DealerId = Dealer,
                        DealerLocationId = Location,
                        DealerPayment = Convert.ToDecimal(item.DealerPayment),
                        DealerPaymentCurrencyTypeId = CurrencyData.Currencies.Find(c => c.Code == item.DealerPaymentCurrencyType).Id,
                        DealerPolicy = item.DealerPolicy == "Yes" ? true : false,
                        Discount = Convert.ToDecimal(item.Discount),
                        ExtensionTypeId = ExtensionType,
                        HrsUsedAtPolicySale = item.HrsUsedAtPolicySale,
                        IsPartialPayment = item.PartialPayment == "Yes" ? true : false,
                        IsSpecialDeal = item.SpecialDeal == "Yes" ? true : false,
                        ItemId = Vehicle,
                        PaymentModeId = Guid.Parse(item.PaymentMode),
                        PolicyEndDate = DateTime.Parse(item.PolicyEndDate),
                        PolicyNo = item.PolicyNo,
                        PolicySoldDate = DateTime.Parse(item.PolicySoldDate),
                        PolicyStartDate = DateTime.Parse(item.PolicyStartDate),
                        Premium = Convert.ToDecimal(item.Premium),
                        PremiumCurrencyTypeId = CurrencyData.Currencies.Find(c => c.Code == item.PremiumCurrencyType).Id,
                        ProductId = Product,
                        RefNo = item.RefNo,
                        SalesPersonId = Guid.Parse(users.Users.Find(u => u.FirstName + " " + u.LastName == item.SalesPerson).Id),
                    }, SecurityHelper.Context, AuditHelper.Context);
                    PolicyRequestDto Policy = PolicyManagementService.AddPolicy(new PolicyRequestDto()
                    {
                        Comment = item.Comment,
                        CommodityTypeId = CommodityTypeId,
                        ContractId = Contract,
                        CoverTypeId = WarrantyType,
                        CustomerId = Customer,
                        CustomerPayment = Convert.ToDecimal(item.CustomerPayment),
                        CustomerPaymentCurrencyTypeId = CurrencyData.Currencies.Find(c => c.Code == item.CustomerPaymentCurrencyType).Id,
                        DealerId = Dealer,
                        DealerLocationId = Location,
                        DealerPayment = Convert.ToDecimal(item.DealerPayment),
                        DealerPaymentCurrencyTypeId = CurrencyData.Currencies.Find(c => c.Code == item.DealerPaymentCurrencyType).Id,
                        DealerPolicy = item.DealerPolicy == "Yes" ? true : false,
                        Discount = Convert.ToDecimal(item.Discount),
                        ExtensionTypeId = ExtensionType,
                        HrsUsedAtPolicySale = item.HrsUsedAtPolicySale,
                        IsPartialPayment = item.PartialPayment == "Yes" ? true : false,
                        IsSpecialDeal = item.SpecialDeal == "Yes" ? true : false,
                        ItemId = Vehicle,
                        PaymentModeId = Guid.Parse(item.PaymentMode),
                        PolicyEndDate = DateTime.Parse(item.PolicyEndDate),
                        PolicyNo = item.PolicyNo,
                        PolicySoldDate = DateTime.Parse(item.PolicySoldDate),
                        PolicyStartDate = DateTime.Parse(item.PolicyStartDate),
                        Premium = Convert.ToDecimal(item.Premium),
                        PremiumCurrencyTypeId = CurrencyData.Currencies.Find(c => c.Code == item.PremiumCurrencyType).Id,
                        ProductId = Product,
                        RefNo = item.RefNo,
                        SalesPersonId = Guid.Parse(users.Users.Find(u => u.FirstName + " " + u.LastName == item.SalesPerson).Id),
                        PolicyBundleId = Bundle.Id
                    }, SecurityHelper.Context, AuditHelper.Context);
                }
                else
                {
                    var ExtensionType = new Guid();
                    if (ExtensionTypes.ExtensionTypes.FindAll(m => m.ExtensionName == item.ExtensionName).Count == 0)
                    {
                        ExtensionType = ExtensionTypeManagementService.AddExtensionType(new ExtensionTypeRequestDto()
                        {
                            CommodityTypeId = CommodityTypeId,
                            ExtensionName = item.ExtensionName,
                            Hours = Convert.ToInt32(item.ExtensionHours),
                            Month = Convert.ToInt32(item.ExtensionMonth),
                            ProductId = Product
                        }, SecurityHelper.Context, AuditHelper.Context).Id;
                    }
                    else
                    {
                        ExtensionType = ExtensionTypes.ExtensionTypes.Find(m => m.ExtensionName == item.ExtensionName).Id;
                    }

                    List<Guid> Models = new List<Guid>();
                    Models.Add(Model);

                    List<ContractExtensionsPremiumAddonRequestDto> addons = new List<ContractExtensionsPremiumAddonRequestDto>();
                    addons.Add(new ContractExtensionsPremiumAddonRequestDto()
                    {
                        PremiumAddonTypeId = PremiumAddonTypes.PremiumAddonTypes.Find(p => p.Description == "Basic Premium").Id,
                        Value = Convert.ToInt32(item.BasicPremium)
                    });
                    addons.Add(new ContractExtensionsPremiumAddonRequestDto()
                    {
                        PremiumAddonTypeId = PremiumAddonTypes.PremiumAddonTypes.Find(p => p.Description == "Other Premium").Id,
                        Value = Convert.ToInt32(item.OtherPremium)
                    });
                    addons.Add(new ContractExtensionsPremiumAddonRequestDto()
                    {
                        PremiumAddonTypeId = PremiumAddonTypes.PremiumAddonTypes.Find(p => p.Description == "Additional Premium").Id,
                        Value = Convert.ToInt32(item.AdditionalPremium)
                    });

                    var ContractExtension = new Guid();
                    if (ContractExtensionData.ContractExtensions.FindAll(m => m.ExtensionTypeId == ExtensionType && m.ContractId == Contract).Count == 0)
                    {
                        ContractExtension = ContractManagementService.AddContractExtensions(new ContractExtensionsRequestDto()
                        {
                            ContractId = Contract,
                            ExtensionTypeId = ExtensionType,
                            GrossPremium = Convert.ToDecimal(item.GrossPremium),
                            Makes = Makes,
                            ManufacturerWarranty = item.ManufacturerWaranty == "Yes" ? true : false,
                            MaxNett = Convert.ToDecimal(item.MaximumPremium),
                            MinNett = Convert.ToDecimal(item.MinimumPremium),
                            Modeles = Models,
                            PremiumAddones = addons,
                            PremiumBasedOnIdNett = PremiumBasedOns.PremiumBasedOns.Find(p => p.Description == item.PremiumBasedOnDescription).Id,
                            PremiumCurrencyId = CurrencyData.Currencies.Find(c => c.Code == item.PremiumCurrencyType).Id,
                            PremiumTotal = Convert.ToDecimal(item.PremiumTotal),
                            WarrantyTypeId = WarrantyType,
                        }, SecurityHelper.Context, AuditHelper.Context).Id;
                    }
                    else
                    {
                        ContractExtension = ContractExtensionData.ContractExtensions.Find(m => m.ExtensionTypeId == ExtensionType && m.ContractId == Contract).Id;
                    }

                    var BrownAndWhiteDetails = new Guid();
                    if (BrownAndWhiteDetailsData.BrownAndWhiteAllDetails.FindAll(m => m.SerialNo == item.ElectronicSerialNo).Count == 0)
                    {
                        BrownAndWhiteDetails = BrownAndWhiteDetailsManagementService.AddBrownAndWhiteDetails(new BrownAndWhiteDetailsRequestDto()
                        {
                            CategoryId = Category,
                            CommodityUsageTypeId = CommodityUsageTypeData.CommodityUsageTypes.Find(c => c.Name == item.CommodityUsageType).Id,
                            DealerPrice = Convert.ToDecimal(item.DealerPrice),
                            ItemPurchasedDate = DateTime.Parse(item.PurchaseDate),
                            ItemStatusId = ItemStatus,
                            MakeId = Make,
                            ModelId = Model,
                            ModelYear = item.VehicleModelYear,
                            SerialNo = item.ElectronicSerialNo,
                            ItemPrice = Convert.ToDecimal(item.ItemPrice),
                            InvoiceNo = item.ElectronicInvoiceNo,
                            ModelCode = item.ModelCode,
                        }, SecurityHelper.Context, AuditHelper.Context).Id;
                    }
                    else
                    {
                        BrownAndWhiteDetails = BrownAndWhiteDetailsData.BrownAndWhiteAllDetails.Find(m => m.SerialNo == item.ElectronicSerialNo).Id;
                    }

                    PolicyBundleRequestDto Bundle = PolicyManagementService.AddPolicyBundle(new PolicyBundleRequestDto()
                    {
                        Comment = item.Comment,
                        CommodityTypeId = CommodityTypeId,
                        ContractId = Contract,
                        CoverTypeId = WarrantyType,
                        CustomerId = Customer,
                        CustomerPayment = Convert.ToDecimal(item.CustomerPayment),
                        CustomerPaymentCurrencyTypeId = CurrencyData.Currencies.Find(c => c.Code == item.CustomerPaymentCurrencyType).Id,
                        DealerId = Dealer,
                        DealerLocationId = Location,
                        DealerPayment = Convert.ToDecimal(item.DealerPayment),
                        DealerPaymentCurrencyTypeId = CurrencyData.Currencies.Find(c => c.Code == item.DealerPaymentCurrencyType).Id,
                        DealerPolicy = item.DealerPolicy == "Yes" ? true : false,
                        Discount = Convert.ToDecimal(item.Discount),
                        ExtensionTypeId = ExtensionType,
                        HrsUsedAtPolicySale = item.HrsUsedAtPolicySale,
                        IsPartialPayment = item.PartialPayment == "Yes" ? true : false,
                        IsSpecialDeal = item.SpecialDeal == "Yes" ? true : false,
                        ItemId = BrownAndWhiteDetails,
                        PaymentModeId = Guid.Parse(item.PaymentMode),
                        PolicyEndDate = DateTime.Parse(item.PolicyEndDate),
                        PolicyNo = item.PolicyNo,
                        PolicySoldDate = DateTime.Parse(item.PolicySoldDate),
                        PolicyStartDate = DateTime.Parse(item.PolicyStartDate),
                        Premium = Convert.ToDecimal(item.Premium),
                        PremiumCurrencyTypeId = CurrencyData.Currencies.Find(c => c.Code == item.PremiumCurrencyType).Id,
                        ProductId = Product,
                        RefNo = item.RefNo,
                        SalesPersonId = Guid.Parse(users.Users.Find(u => u.FirstName + " " + u.LastName == item.SalesPerson).Id),
                    }, SecurityHelper.Context, AuditHelper.Context);
                    PolicyRequestDto Policy = PolicyManagementService.AddPolicy(new PolicyRequestDto()
                    {
                        Comment = item.Comment,
                        CommodityTypeId = CommodityTypeId,
                        ContractId = Contract,
                        CoverTypeId = WarrantyType,
                        CustomerId = Customer,
                        CustomerPayment = Convert.ToDecimal(item.CustomerPayment),
                        CustomerPaymentCurrencyTypeId = CurrencyData.Currencies.Find(c => c.Code == item.CustomerPaymentCurrencyType).Id,
                        DealerId = Dealer,
                        DealerLocationId = Location,
                        DealerPayment = Convert.ToDecimal(item.DealerPayment),
                        DealerPaymentCurrencyTypeId = CurrencyData.Currencies.Find(c => c.Code == item.DealerPaymentCurrencyType).Id,
                        DealerPolicy = item.DealerPolicy == "Yes" ? true : false,
                        Discount = Convert.ToDecimal(item.Discount),
                        ExtensionTypeId = ExtensionType,
                        HrsUsedAtPolicySale = item.HrsUsedAtPolicySale,
                        IsPartialPayment = item.PartialPayment == "Yes" ? true : false,
                        IsSpecialDeal = item.SpecialDeal == "Yes" ? true : false,
                        ItemId = BrownAndWhiteDetails,
                        PaymentModeId = Guid.Parse(item.PaymentMode),
                        PolicyEndDate = DateTime.Parse(item.PolicyEndDate),
                        PolicyNo = item.PolicyNo,
                        PolicySoldDate = DateTime.Parse(item.PolicySoldDate),
                        PolicyStartDate = DateTime.Parse(item.PolicyStartDate),
                        Premium = Convert.ToDecimal(item.Premium),
                        PremiumCurrencyTypeId = CurrencyData.Currencies.Find(c => c.Code == item.PremiumCurrencyType).Id,
                        ProductId = Product,
                        RefNo = item.RefNo,
                        SalesPersonId = Guid.Parse(users.Users.Find(u => u.FirstName + " " + u.LastName == item.SalesPerson).Id),
                        PolicyBundleId = Bundle.Id
                    }, SecurityHelper.Context, AuditHelper.Context);
                }
            }

            */

#endregion
