using TAS.Services.BusinessServices;

namespace TAS.Services
{
    public static class ServiceFactory
    {

        public static IUserManagementService GetUserManagementService()
        {
            return new UserManagementService();
        }

        public static IVehicleHorsePowerManagementService GetVehicleHorsePowerManagementService()
        {
            return new VehicleHorsePowerManagementService();
        }

        public static IVehicleKiloWattManagementService GetVehicleKiloWattManagementService()
        {
            return new VehicleKiloWattManagementService();
        }

        public static ICommodityUsageTypeManagementService GetCommodityUsageTypeManagementService()
        {
            return new CommodityUsageTypeManagementService();
        }

        public static IReportManagementService GetReportManagementService()
        {
            return new ReportManagementService();
        }

        public static IMenuManagementService GetMenuManagementService()
        {
            return new MenuManagementService();
        }

        public static IEligibilityManagementService GetEligibilityManagementService()
        {
            return new EligibilityManagementService();
        }

        public static IBordxManagementService GetBordxManagementService()
        {
            return new BordxManagementService();
        }

        public static IClaimBordxManagementService GetClaimBordxManagementService()
        {
            return new ClaimBordxManagementService();
        }

        public static IReinsurerReceiptManagementService GetReinsurerReceiptManagementService()
        {
            return new ReinsurerReceiptManagementService();
        }

        public static IRSAProviderManagementService GetRSAProviderManagementService()
        {
            return new RSAProviderManagementService();
        }

        public static IRegionManagementService GetRegionManagementService()
        {
            return new RegionManagementService();
        }

        public static INRPCommissionTypesManagementService GetNRPCommissionTypesManagementService()
        {
            return new NRPCommissionTypesManagementService();
        }

        public static ITaxManagementService GetTaxManagementService()
        {
            return new TaxManagementService();
        }

        //public static IFaultManagementService GetFaultManagementService()
        //{
        //    return new FaultManagementService();
        //}

        public static ISystemUserManagementService GetSystemUserManagementService()
        {
            return new SystemUserManagementService();
        }

        public static ICurrencyManagementService GetCurrencyManagementService()
        {
            return new CurrencyManagementService();
        }

        public static IWarrantyTypeManagementService GetWarrantyTypeManagementService()
        {
            return new WarrantyTypeManagementService();
        }

        public static IExtensionTypeManagementService GetExtensionTypeManagementService()
        {
            return new ExtensionTypeManagementService();
        }

        public static IPremiumBasedOnManagementService GetPremiumBasedOnManagementService()
        {
            return new PremiumBasedOnManagementService();
        }

        public static IPremiumAddonTypeManagementService GetPremiumAddonTypeManagementService()
        {
            return new PremiumAddonTypeManagementService();
        }

        public static IContractManagementService GetContractManagementService()
        {
            return new ContractManagementService();
        }
        public static IPolicyManagementService GetPolicyManagementService()
        {
            return new PolicyManagementService();
        }

        public static ICountryManagementService GetCountryManagementService()
        {
            return new CountryManagementService();
        }

        public static ICommodityManagementService GetCommodityManagementService()
        {
            return new CommodityManagementService();
        }

        public static INationalityManagementService GetNationalityManagementService()
        {
            return new NationalityManagementService();
        }

        public static ICityManagementService GetCityManagementService()
        {
            return new CityManagementService();
        }

        public static ICustomerManagementService GetCustomerManagementService()
        {
            return new CustomerManagementService();
        }
        public static IManufacturerWarrantyManagementService GetManufacturerWarrantyManagementService()
        {
            return new ManufacturerWarrantyManagementService();
        }
        //public static IUserDetailManagementService GetUserDetailManagementService()
        //{
        //    return new UserDetailManagementService();
        //}

        public static ICustomerTypeManagementService GetCustomerTypeManagementService()
        {
            return new CustomerTypeManagementService();
        }

        public static IIdTypeManagementService GetIdTypeManagementService()
        {
            return new IdTypeManagementService();
        }

        public static IUsageTypeManagementService GetUsageTypeManagementService()
        {
            return new UsageTypeManagementService();
        }

        public static IMakeManagementService GetMakeManagementService()
        {
            return new MakeManagementService();
        }

        public static IModelManagementService GetModelManagementService()
        {
            return new ModelManagementService();
        }

        public static IManufacturerManagementService GetManufacturerManagementService()
        {
            return new ManufatureManagementService();
        }

        public static ITPAManagementService GetTPAManagementService()
        {
            return new TPAManagementService();
        }

        public static IImageManagementService GetImageManagementService()
        {
            return new ImageManagementService();
        }

        public static ITPABranchManagementService GetTPABranchManagementService()
        {
            return new TPABranchManagementService();
        }

        public static IProductManagementService GetProductManagementService()
        {
            return new ProductManagementService();
        }

        public static IProductTypeManagementService GetProductTypeManagementService()
        {
            return new ProductTypeManagementService();
        }

        public static ICylinderCountManagementService GetCylinderCountManagementService()
        {
            return new CylinderCountManagementService();
        }

        public static IDriveTypeManagementService GetDriveTypeManagementService()
        {
            return new DriveTypeManagementService();
        }

        public static IItemStatusManagementService GetItemStatusManagementService()
        {
            return new ItemStatusManagementService();
        }

        public static ITransmissionTypeManagementService GetTransmissionTypeManagementService()
        {
            return new TransmissionTypeManagementService();
        }

        public static IVehicleAspirationTypeManagementService GetVehicleAspirationTypeManagementService()
        {
            return new VehicleAspirationTypeManagementService();
        }

        public static IReinsurerManagementService GetReinsurerManagementService()
        {
            return new ReinsurerManagementService();
        }

        public static IReinsurerContractManagementService GetReinsurerContractManagementService()
        {
            return new ReinsurerContractManagementService();
        }
        public static IInsurerManagementService GetInsurerManagementService()
        {
            return new InsurerManagementService();
        }

        public static IBrownAndWhiteDetailsManagementService GetBrownAndWhiteDetailsManagementService()
        {
            return new BrownAndWhiteDetailsManagementService();
        }

        public static IVehicleDetailsManagementService GetVehicleDetailsManagementService()
        {
            return new VehicleDetailsManagementService();
        }

        public static ICommodityCategoryManagementService GetCommodityCategoryManagementService()
        {
            return new CommodityCategoryManagementService();
        }

        public static IEngineCapacityManagementService GetEngineCapacityManagementService()
        {
            return new EngineCapacityManagementService();
        }

        public static IFuelTypeManagementService GetFuelTypeManagementService()
        {
            return new FuelTypeManagementService();
        }

        public static IVehicleWeightManagementService GetVehicleWeightManagementService()
        {
            return new VehicleWeightManagementService();
        }

        public static IVehicleBodyTypeManagementService GetVehicleBodyTypeManagementService()
        {
            return new VehicleBodyTypeManagementService();
        }

        public static IVehicleColorManagementService GetVehicleColorManagementService()
        {
            return new VehicleColorManagementService();
        }

        public static IDealerManagementService GetDealerManagementService()
        {
            return new DealerManagementService();
        }
        public static IDealerLocationManagementService GetDealerLocationManagementService()
        {
            return new DealerLocationManagementService();
        }
        public static IVariantManagementService GetVariantManagementService()
        {
            return new VariantManagementService();
        }

        public static ITASTPAManagementService GetTASTPAManagementService()
        {
            return new TASTPAManagementService();
        }

        public static IDealTypeManagementService GetDealTypeManagementService()
        {
            return new DealTypeManagementService();
        }

        public static IPaymentManagementService GetPaymentManagementService()
        {
            return new PaymentManagementService();
        }

        public static IOtherItemManagementService GetOtherItemManagementService()
        {
            return new OtherItemManagementService();
        }

        public static IUploadManagementService GetUploadManagementService()
        {
            return new UploadManagementService();
        }

        public static IYellowGoodManagementService GetYellowGoodManagementService()
        {
            return new YellowGoodManagementService();
        }

        public static ITPABranchManagementService GetTpaManagementService()
        {
            return new TPABranchManagementService();
        }

        public static ITempBulkUploadManagementService GetTempBulkUploadManagementService()
        {
            return new TempBulkUploadManagementService();
        }


        public static IClaimManagementService GetClaimManagementService()
        {
            return new ClaimManagementService();
        }


        public static IClaimInvoiceManagementService GetClaimInvoiceManagementService()
        {
            return new ClaimInvoiceManagementService();
        }

        public static IClaimBatchingManagementService GetClaimBatchingManagementService()
        {
            return new ClaimBatchingManagementService();
        }



        public static IFaultManagementService GetFaultManagementService()
        {
            return new FaultManagementService();
        }

        public static IClaimBordxProcessManagementService GetClaimBordxProcessManagementService()
        {
            return new ClaimBordxProcessManagementService();
        }

        public static IIncurredErningManagementService GetIncurredErningManagementService()
        {
            return new IncurredErningManagementService();
        }

        public static IBrokerManagementService GetBrokerManagementService()
        {
            return new BrokerManagementService();
        }

        public static IBuyingWizardManagementService GetBuyingWizardManagementService()
        {
            return new BuyingWizardManagementService();
        }

        public static ISystemLanguageManagementService GetSystemLanguageManagementService()
        {
            return new SystemLanguageManagementService();
        }
    }
}
