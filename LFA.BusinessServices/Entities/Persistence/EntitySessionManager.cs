
using NHibernate;
using NHibernate.Cfg;
using TAS.Core.Storage;
using TAS.Services.Common;


namespace TAS.Services.Entities.Persistence
{
    internal static class EntitySessionManager
    {
        private static ISessionFactory sessionFactory;
        //private static System.Data.IDbConnection conn;
        private static int retryCnt = 0;

        private static void buildSessionFactory()
        {
            retryCnt += 1;
            try
            {
                Configuration configuration = new Configuration();
                string configPath = ConfigurationData.DataMappingFilePath;
                string mappingPath = configPath + @"Mappings\";

                configuration.Configure(configPath + @"hibernate.cfg.xml");
                configuration.AddFile(mappingPath + @"SystemUser.hbm.xml");
                configuration.AddFile(mappingPath + @"CurrencyEmail.hbm.xml");
                configuration.AddFile(mappingPath + @"User.hbm.xml");
                configuration.AddFile(mappingPath + @"UserType.hbm.xml");
                configuration.AddFile(mappingPath + @"UserRole.hbm.xml");
                configuration.AddFile(mappingPath + @"CommodityCategory.hbm.xml");
                configuration.AddFile(mappingPath + @"CommodityType.hbm.xml");
                configuration.AddFile(mappingPath + @"Manufacturer.hbm.xml");
                configuration.AddFile(mappingPath + @"Country.hbm.xml");
                configuration.AddFile(mappingPath + @"Nationality.hbm.xml");
                configuration.AddFile(mappingPath + @"CustomerType.hbm.xml");
                configuration.AddFile(mappingPath + @"UsageType.hbm.xml");
                configuration.AddFile(mappingPath + @"CommodityUsageType.hbm.xml");
                configuration.AddFile(mappingPath + @"IdType.hbm.xml");
                configuration.AddFile(mappingPath + @"City.hbm.xml");
                configuration.AddFile(mappingPath + @"TPA.hbm.xml");
                configuration.AddFile(mappingPath + @"Customer.hbm.xml");
                configuration.AddFile(mappingPath + @"Image.hbm.xml");
                configuration.AddFile(mappingPath + @"TPABranch.hbm.xml");
                configuration.AddFile(mappingPath + @"Product.hbm.xml");
                configuration.AddFile(mappingPath + @"ProductType.hbm.xml");
                configuration.AddFile(mappingPath + @"BundledProduct.hbm.xml");
                configuration.AddFile(mappingPath + @"CylinderCount.hbm.xml");
                configuration.AddFile(mappingPath + @"DriveType.hbm.xml");
                configuration.AddFile(mappingPath + @"EngineCapacity.hbm.xml");
                configuration.AddFile(mappingPath + @"FuelType.hbm.xml");
                configuration.AddFile(mappingPath + @"VehicleBodyType.hbm.xml");
                configuration.AddFile(mappingPath + @"VehicleColor.hbm.xml");
                configuration.AddFile(mappingPath + @"Dealer.hbm.xml");
                configuration.AddFile(mappingPath + @"CountryMakes.hbm.xml");
                configuration.AddFile(mappingPath + @"CountryModeles.hbm.xml");
                configuration.AddFile(mappingPath + @"DealerMakes.hbm.xml");
                configuration.AddFile(mappingPath + @"DealerLocation.hbm.xml");
                configuration.AddFile(mappingPath + @"Model.hbm.xml");
                configuration.AddFile(mappingPath + @"Make.hbm.xml");
                configuration.AddFile(mappingPath + @"ItemStatus.hbm.xml");
                configuration.AddFile(mappingPath + @"ProductCategory.hbm.xml");
                configuration.AddFile(mappingPath + @"BrownAndWhiteDetails.hbm.xml");
                configuration.AddFile(mappingPath + @"VehicleDetails.hbm.xml");
                configuration.AddFile(mappingPath + @"Insurer.hbm.xml");
                configuration.AddFile(mappingPath + @"InsurerCountries.hbm.xml");
                configuration.AddFile(mappingPath + @"InsurerCommodityTypes.hbm.xml");
                configuration.AddFile(mappingPath + @"InsurerProducts.hbm.xml");
                configuration.AddFile(mappingPath + @"Reinsurer.hbm.xml");
                configuration.AddFile(mappingPath + @"ReinsurerConsortium.hbm.xml");
                configuration.AddFile(mappingPath + @"InsurerConsortium.hbm.xml");
                configuration.AddFile(mappingPath + @"ReinsurerContract.hbm.xml");
                configuration.AddFile(mappingPath + @"TransmissionType.hbm.xml");
                configuration.AddFile(mappingPath + @"VehicleAspirationType.hbm.xml");
                configuration.AddFile(mappingPath + @"ManufacturerComodityTypes.hbm.xml");
                configuration.AddFile(mappingPath + @"VariantBodyTypes.hbm.xml");
                configuration.AddFile(mappingPath + @"VariantCountrys.hbm.xml");
                configuration.AddFile(mappingPath + @"VariantFuelTypes.hbm.xml");
                configuration.AddFile(mappingPath + @"VariantAspirations.hbm.xml");
                configuration.AddFile(mappingPath + @"VariantTransmissions.hbm.xml");
                configuration.AddFile(mappingPath + @"VariantDriveTypes.hbm.xml");
                configuration.AddFile(mappingPath + @"Variant.hbm.xml");
                configuration.AddFile(mappingPath + @"Policy.hbm.xml");
                configuration.AddFile(mappingPath + @"PolicyBundle.hbm.xml");
                configuration.AddFile(mappingPath + @"PolicyBundleTransaction.hbm.xml");
                configuration.AddFile(mappingPath + @"PolicyBundleHistory.hbm.xml");
                configuration.AddFile(mappingPath + @"PolicyHistory.hbm.xml");
                configuration.AddFile(mappingPath + @"PolicyTransaction.hbm.xml");
                configuration.AddFile(mappingPath + @"PolicyTransactionType.hbm.xml");
                configuration.AddFile(mappingPath + @"VehiclePolicy.hbm.xml");
                configuration.AddFile(mappingPath + @"BAndWPolicy.hbm.xml");
                configuration.AddFile(mappingPath + @"ManufacturerWarranty.hbm.xml");
                configuration.AddFile(mappingPath + @"WarrantyType.hbm.xml");
                configuration.AddFile(mappingPath + @"ContractExtensionCylinderCount.hbm.xml");
                configuration.AddFile(mappingPath + @"ContractExtensionEngineCapacity.hbm.xml");
                configuration.AddFile(mappingPath + @"ContractExtensionManufacturerWarranty.hbm.xml");
                configuration.AddFile(mappingPath + @"ContractExtensionMake.hbm.xml");
                configuration.AddFile(mappingPath + @"ContractExtensionModel.hbm.xml");
                configuration.AddFile(mappingPath + @"PremiumBasedOn.hbm.xml");
                configuration.AddFile(mappingPath + @"PremiumAddonType.hbm.xml");
                configuration.AddFile(mappingPath + @"Contract.hbm.xml");
                configuration.AddFile(mappingPath + @"ContractExtensions.hbm.xml");
                configuration.AddFile(mappingPath + @"ContractExtensionsPremiumAddon.hbm.xml");
                configuration.AddFile(mappingPath + @"ClaimCriteria.hbm.xml");
                configuration.AddFile(mappingPath + @"ExtensionType.hbm.xml");
                configuration.AddFile(mappingPath + @"Currency.hbm.xml");
                configuration.AddFile(mappingPath + @"Menu.hbm.xml");
                configuration.AddFile(mappingPath + @"CurrencyConversionPeriods.hbm.xml");
                configuration.AddFile(mappingPath + @"CurrencyConversions.hbm.xml");
                configuration.AddFile(mappingPath + @"UserLogin.hbm.xml");
                configuration.AddFile(mappingPath + @"NRPCommissionContractMapping.hbm.xml");
                configuration.AddFile(mappingPath + @"NRPCommissionTypes.hbm.xml");
                configuration.AddFile(mappingPath + @"TaxTypes.hbm.xml");
                configuration.AddFile(mappingPath + @"CountryTaxes.hbm.xml");
                configuration.AddFile(mappingPath + @"RoleMenuMapping.hbm.xml");
                configuration.AddFile(mappingPath + @"PriviledgeLevel.hbm.xml");
                configuration.AddFile(mappingPath + @"ContractTaxes.hbm.xml");
                configuration.AddFile(mappingPath + @"RSAProvider.hbm.xml");
                configuration.AddFile(mappingPath + @"RSAAnualPremium.hbm.xml");
                configuration.AddFile(mappingPath + @"Region.hbm.xml");
                configuration.AddFile(mappingPath + @"PolicyContractProduct.hbm.xml");
                configuration.AddFile(mappingPath + @"StagOnlinePurchase.hbm.xml");
                configuration.AddFile(mappingPath + @"MaritalStatus.hbm.xml");
                configuration.AddFile(mappingPath + @"Title.hbm.xml");
                configuration.AddFile(mappingPath + @"Occupation.hbm.xml");
                configuration.AddFile(mappingPath + @"Bordx.hbm.xml");
                configuration.AddFile(mappingPath + @"BordxDetails.hbm.xml");
                configuration.AddFile(mappingPath + @"Eligibility.hbm.xml");
                configuration.AddFile(mappingPath + @"BordxReportColumns.hbm.xml");
                configuration.AddFile(mappingPath + @"BordxReportColumnsMap.hbm.xml");
                configuration.AddFile(mappingPath + @"VehicleHorsePower.hbm.xml");
                configuration.AddFile(mappingPath + @"VehicleKiloWatt.hbm.xml");
                configuration.AddFile(mappingPath + @"DealerStaff.hbm.xml");
                configuration.AddFile(mappingPath + @"TransmissionTechnology.hbm.xml");
                configuration.AddFile(mappingPath + @"TransmissionTechnologyMap.hbm.xml");
                configuration.AddFile(mappingPath + @"ForgotPasswordRequest.hbm.xml");
                configuration.AddFile(mappingPath + @"UserRoleMapping.hbm.xml");
                configuration.AddFile(mappingPath + @"ReinsurerUser.hbm.xml");
                configuration.AddFile(mappingPath + @"DealType.hbm.xml");
                configuration.AddFile(mappingPath + @"UserBranch.hbm.xml");
                configuration.AddFile(mappingPath + @"PaymentMode.hbm.xml");
                configuration.AddFile(mappingPath + @"OtherItemDetails.hbm.xml");
                configuration.AddFile(mappingPath + @"OtherItemPolicy.hbm.xml");
                configuration.AddFile(mappingPath + @"YellowGoodDetails.hbm.xml");
                configuration.AddFile(mappingPath + @"PaymentOption.hbm.xml");
                configuration.AddFile(mappingPath + @"YellowGoodPolicy.hbm.xml");
                configuration.AddFile(mappingPath + @"PolicyAttachment.hbm.xml");
                configuration.AddFile(mappingPath + @"UserAttachment.hbm.xml");
                configuration.AddFile(mappingPath + @"AttachmentSection.hbm.xml");
                configuration.AddFile(mappingPath + @"AttachmentType.hbm.xml");
                configuration.AddFile(mappingPath + @"PolicyAttachmentTransaction.hbm.xml");
                configuration.AddFile(mappingPath + @"BrownAndWhiteDetailsHistory.hbm.xml");
                configuration.AddFile(mappingPath + @"CustomerHistory.hbm.xml");
                configuration.AddFile(mappingPath + @"OtherItemDetailsHistory.hbm.xml");
                configuration.AddFile(mappingPath + @"VehicleDetailsHistory.hbm.xml");
                configuration.AddFile(mappingPath + @"YellowGoodDetailsHistory.hbm.xml");

                configuration.AddFile(mappingPath + @"UnitOfWork.hbm.xml");
                configuration.AddFile(mappingPath + @"PolicyRenewal.hbm.xml");
                configuration.AddFile(mappingPath + @"BordxReportColumnHeaders.hbm.xml");
                configuration.AddFile(mappingPath + @"TimeZone.hbm.xml");
                configuration.AddFile(mappingPath + @"PolicyTax.hbm.xml");
                configuration.AddFile(mappingPath + @"ContractExtensionVariant.hbm.xml");
                configuration.AddFile(mappingPath + @"DealerInvoiceReportColumns.hbm.xml");

                configuration.AddFile(mappingPath + @"TempBulkHeader.hbm.xml");
                configuration.AddFile(mappingPath + @"TempBulkUpload.hbm.xml");

                configuration.AddFile(mappingPath + @"Part.hbm.xml");
                configuration.AddFile(mappingPath + @"PartArea.hbm.xml");
                configuration.AddFile(mappingPath + @"PartModel.hbm.xml");
                configuration.AddFile(mappingPath + @"PartPrice.hbm.xml");
                configuration.AddFile(mappingPath + @"PartSuggestion.hbm.xml");
                configuration.AddFile(mappingPath + @"VehicleServiceHistory.hbm.xml");

                configuration.AddFile(mappingPath + @"ClaimItemType.hbm.xml");
                configuration.AddFile(mappingPath + @"ClaimStatusCode.hbm.xml");
                configuration.AddFile(mappingPath + @"ClaimSubmission.hbm.xml");
                configuration.AddFile(mappingPath + @"ClaimSubmissionAttachment.hbm.xml");
                configuration.AddFile(mappingPath + @"ClaimSubmissionItem.hbm.xml");

                configuration.AddFile(mappingPath + @"PartRejectionType.hbm.xml");

                configuration.AddFile(mappingPath + @"ClaimInvoiceEntry.hbm.xml");
                configuration.AddFile(mappingPath + @"ClaimInvoiceEntryClaim.hbm.xml");
                configuration.AddFile(mappingPath + @"Claim.hbm.xml");
                configuration.AddFile(mappingPath + @"ClaimGroupClaim.hbm.xml");
                configuration.AddFile(mappingPath + @"ClaimBatchGroup.hbm.xml");
                configuration.AddFile(mappingPath + @"ClaimBatch.hbm.xml");
                configuration.AddFile(mappingPath + @"ClaimBordxYearly.hbm.xml");

                configuration.AddFile(mappingPath + @"ClaimChequePayment.hbm.xml");
                configuration.AddFile(mappingPath + @"ClaimChequePaymentDetail.hbm.xml");

                configuration.AddFile(mappingPath + @"ReinsureBordxReportColumns.hbm.xml");

                configuration.AddFile(mappingPath + @"FaultCategory.hbm.xml");
                configuration.AddFile(mappingPath + @"FaultArea.hbm.xml");
                configuration.AddFile(mappingPath + @"Fault.hbm.xml");
                configuration.AddFile(mappingPath + @"FaultCauseOfFailure.hbm.xml");
                configuration.AddFile(mappingPath + @"ClaimBordx.hbm.xml");
                configuration.AddFile(mappingPath + @"ClaimBordxPayment.hbm.xml");
                configuration.AddFile(mappingPath + @"ClaimItem.hbm.xml");
                configuration.AddFile(mappingPath + @"ClaimAttachment.hbm.xml");
                configuration.AddFile(mappingPath + @"ClaimProcessingStat.hbm.xml");
                configuration.AddFile(mappingPath + @"ClaimEndorsementItem.hbm.xml");
                configuration.AddFile(mappingPath + @"ClaimEndorsement.hbm.xml");
                configuration.AddFile(mappingPath + @"ClaimEndorsementAttachment.hbm.xml");
                configuration.AddFile(mappingPath + @"ClaimBordxYearlySum.hbm.xml");
                configuration.AddFile(mappingPath + @"DealerBranchStaff.hbm.xml");

                configuration.AddFile(mappingPath + @"ClaimBordxProcess.hbm.xml");
                configuration.AddFile(mappingPath + @"ClaimBordxDetail.hbm.xml");
                configuration.AddFile(mappingPath + @"ClaimBordxValueDetail.hbm.xml");
                configuration.AddFile(mappingPath + @"DealerDiscount.hbm.xml");
                configuration.AddFile(mappingPath + @"DealerDiscountScheme.hbm.xml");

                configuration.AddFile(mappingPath + @"DashboardChartAccess.hbm.xml");
                configuration.AddFile(mappingPath + @"DashboardChart.hbm.xml");

                configuration.AddFile(mappingPath + @"TPAAllowedIP.hbm.xml");
                configuration.AddFile(mappingPath + @"TPAConfig.hbm.xml");
                configuration.AddFile(mappingPath + @"ManufacturerWarrantyDetails.hbm.xml");
                configuration.AddFile(mappingPath + @"DealerLabourCharge.hbm.xml");
                configuration.AddFile(mappingPath + @"VariantPremiumAddon.hbm.xml");
                configuration.AddFile(mappingPath + @"VehicleWeight.hbm.xml");
                configuration.AddFile(mappingPath + @"PolicyNumberFormat.hbm.xml");
                configuration.AddFile(mappingPath + @"InsuaranceLimitation.hbm.xml");
                configuration.AddFile(mappingPath + @"ContractInsuaranceLimitation.hbm.xml");
                configuration.AddFile(mappingPath + @"ContractExtensionGVW.hbm.xml");
                configuration.AddFile(mappingPath + @"ContractExtensionPremium.hbm.xml");
                configuration.AddFile(mappingPath + @"StoredProcedures.hbm.xml");
                configuration.AddFile(mappingPath + @"Broker.hbm.xml");
                configuration.AddFile(mappingPath + @"ClaimNotes.hbm.xml");
                configuration.AddFile(mappingPath + @"ClaimComment.hbm.xml");
                configuration.AddFile(mappingPath + @"ClaimRejectionType.hbm.xml");
                configuration.AddFile(mappingPath + @"CustomerLogin.hbm.xml");
                configuration.AddFile(mappingPath + @"CustomerComplaint.hbm.xml");
                configuration.AddFile(mappingPath + @"DealerComment.hbm.xml");
                configuration.AddFile(mappingPath + @"TireUTDValuation.hbm.xml");
                configuration.AddFile(mappingPath + @"ClaimItemTireDetails.hbm.xml");

                configuration.AddFile(mappingPath + @"InvoiceCode.hbm.xml");
                configuration.AddFile(mappingPath + @"InvoiceCodeDetails.hbm.xml");
                configuration.AddFile(mappingPath + @"InvoiceCodeTireDetails.hbm.xml");
                configuration.AddFile(mappingPath + @"AdditionalPolicyData.hbm.xml");
                configuration.AddFile(mappingPath + @"AdditionalPolicyFields.hbm.xml");
                configuration.AddFile(mappingPath + @"TireSizeVariantMap.hbm.xml");
                configuration.AddFile(mappingPath + @"AdditionalPolicyMakeData.hbm.xml");
                configuration.AddFile(mappingPath + @"AdditionalPolicyModelData.hbm.xml");
                configuration.AddFile(mappingPath + @"AvailableTireSizes.hbm.xml");
                configuration.AddFile(mappingPath + @"AvailableTireSizesPattern.hbm.xml");
                configuration.AddFile(mappingPath + @"ClaimTax.hbm.xml");

                configuration.AddFile(mappingPath + @"CustomerEnterdInvoiceDetails.hbm.xml");
                configuration.AddFile(mappingPath + @"CustomerEnterdInvoiceTireDetails.hbm.xml");
                configuration.AddFile(mappingPath + @"ReportDataQuery.hbm.xml");

                configuration.AddFile(mappingPath + @"Report.hbm.xml");
                configuration.AddFile(mappingPath + @"ReportParameter.hbm.xml");

                configuration.AddFile(mappingPath + @"BordxReportTemplate.hbm.xml");
                configuration.AddFile(mappingPath + @"BordxReportTemplateDetails.hbm.xml");
                configuration.AddFile(mappingPath + @"OtherItemServiceHistory.hbm.xml");
                configuration.AddFile(mappingPath + @"SystemLanguage.hbm.xml");
                configuration.AddFile(mappingPath + @"ArticleMapping.hbm.xml");
                configuration.AddFile(mappingPath + @"PolicyAdditionalDetails.hbm.xml");
                //configuration.AddFile(mappingPath + @"DealerInvoiceCodeReportColumnHeader.hbm.xml");

                sessionFactory = configuration.BuildSessionFactory();
            }
            catch (System.Exception e)
            {
                if (retryCnt <= 3)
                {
                    buildSessionFactory();
                }
            }

        }



        public static void OpenSession()
        {
            if (sessionFactory == null)
            {
                buildSessionFactory();
            }
            ISession session = sessionFactory.OpenSession();
            EntitySessionManager.SetSession(session);
        }

        public static void OpenSession(string connectionString)
        {
            System.Data.IDbConnection conn = new System.Data.SqlClient.SqlConnection(connectionString);

            if (sessionFactory == null)
            {
                buildSessionFactory();
            }

            conn.Open();
            ISession session = sessionFactory.OpenSession(conn);
            EntitySessionManager.SetSession(session);


        }

        public static ISession GetSession()
        {
            return StorageManager.GetData("session") as ISession;
        }

        internal static void SetSession(ISession session)
        {
            StorageManager.SetData("session", session);
        }

        public static void CloseSession()
        {
            ISession session = EntitySessionManager.GetSession();
            if (session != null)
            {
                if (session.IsOpen)
                {
                    session.Close();
                    session.Dispose();
                    session = null;
                }
            }

            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            // TODO: check if session is getting disposed correctly
        }
    }
}