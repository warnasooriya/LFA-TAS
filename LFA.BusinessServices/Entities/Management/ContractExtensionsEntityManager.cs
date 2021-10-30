using NHibernate;
using NHibernate.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.Entities.Management
{
    public class ContractExtensionsEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<ContractExtensions> GetContractExtensions()
        {
            List<ContractExtensions> result = new List<ContractExtensions>();
            ISession session = EntitySessionManager.GetSession();
            IQueryable<ContractExtensions> ContractExtensionsData = session.Query<ContractExtensions>();
            result = ContractExtensionsData.ToList();

            #region Not Used
            //List<ContractExtensionMake> ContractExtensionsMakes = null;
            //List<ContractExtensionModel> ContractExtensionsModeles = null;
            //List<ContractExtensionManufacturerWarranty> ContractExtensionManufacturerWarranties = null;
            //List<ContractExtensionEngineCapacity> ContractExtensionsEngineCapacities = null;
            //List<ContractExtensionCylinderCount> ContractExtensionsCylinderCounts = null;
            //List<ContractExtensionsPremiumAddon> ContractExtensionsPremiumAddons = null;
            //List<Contract> Contract = null;

            //List<ContractExtensionsInfo> result = new List<ContractExtensionsInfo>();
            //foreach (var item in ContractExtensionsData)
            //{
            //    IQueryable<ContractExtensionMake> ContractExtensionsMakesData = session.Query<ContractExtensionMake>();
            //    IQueryable<ContractExtensionModel> ContractExtensionsModelsData = session.Query<ContractExtensionModel>();
            //    IQueryable<ContractExtensionManufacturerWarranty> ContractExtensionManufacturerWarrantiesData = session.Query<ContractExtensionManufacturerWarranty>();
            //    IQueryable<ContractExtensionEngineCapacity> ContractExtensionsEngineCapacitysData = session.Query<ContractExtensionEngineCapacity>();
            //    IQueryable<ContractExtensionCylinderCount> ContractExtensionsCylinderCountsData = session.Query<ContractExtensionCylinderCount>();
            //    IQueryable<ContractExtensionsPremiumAddon> ContractExtensionsPremiumAddonsData = session.Query<ContractExtensionsPremiumAddon>();
            //    IQueryable<Contract> ContractsData = session.Query<Contract>();

            //    ContractExtensionsMakes = ContractExtensionsMakesData.Where(x => x.ContractExtensionId == item.Id).ToList();
            //    ContractExtensionsModeles = ContractExtensionsModelsData.Where(x => x.ContractExtensionId == item.Id).ToList();
            //    ContractExtensionManufacturerWarranties = ContractExtensionManufacturerWarrantiesData.Where(x => x.ContractExtensionId == item.Id).ToList();
            //    ContractExtensionsEngineCapacities= ContractExtensionsEngineCapacitysData.Where(x => x.ContractExtensionId == item.Id).ToList();
            //    ContractExtensionsCylinderCounts = ContractExtensionsCylinderCountsData.Where(x => x.ContractExtensionId == item.Id).ToList();
            //    ContractExtensionsPremiumAddons = ContractExtensionsPremiumAddonsData.Where(x => x.ContractExtensionId == item.Id).ToList();
            //    Contract = ContractsData.Where(x => x.Id == item.ContractId).ToList();

            //    ContractExtensionsInfo ContractExtensionsInfo = new ContractExtensionsInfo()
            //    {
            //        Id = item.Id,
            //        AttributeSpecification = item.AttributeSpecification,
            //        ContractId = item.ContractId,
            //        IsCustAvailable = item.IsCustAvailable,
            //        ManufacturerWarranty = item.ManufacturerWarranty,
            //       WarrantyTypeId = item.WarrantyTypeId,
            //        ExtensionTypeId = item.ExtensionTypeId,
            //        Max = item.Max,
            //        Min=item.Min,
            //        PremiumBasedOnId = item.PremiumBasedOnId,
            //        PremiumTotal = item.PremiumTotal,
            //        PremiumCurrencyId = item.PremiumCurrencyId ,                    
            //        Makes = ContractExtensionsMakes.Select(c => c.MakeId).ToList(),
            //        Modeles = ContractExtensionsModeles.Select(c => c.ModelId).ToList(),
            //        EngineCapacities = ContractExtensionsEngineCapacities.Select(c => c.EngineCapacityId).ToList(),
            //        CylinderCounts = ContractExtensionsCylinderCounts.Select(c => c.CylinderCountId).ToList(),
            //        PremiumAddones = ContractExtensionsPremiumAddons.ToList(), 
            //        EntryDateTime = item.EntryDateTime,
            //        EntryUser = item.EntryUser,
            //    };
            //    result.Add(ContractExtensionsInfo);
            //}
            #endregion
            return result;
        }

        public ContractExtensionResponseDto GetContractExtensionsById(Guid ContractExtensionsId)
        {

            try
            {
                if (ContractExtensionsId == Guid.Empty)
                    return null;

                ISession session = EntitySessionManager.GetSession();

                ContractExtensionResponseDto pr = new ContractExtensionResponseDto();
                ContractResponseDto ContractData = new ContractResponseDto();

                var query =
                    from ContractExtensions in session.Query<ContractExtensions>()
                    where ContractExtensions.Id == ContractExtensionsId
                    select new { ContractExtensions = ContractExtensions };

                var result = query.ToList();

                var queryContact =
                   from Contract in session.Query<Contract>()
                   //where Contract.Id == result.First().ContractExtensions.ContractId
                   select new { Contract = Contract };


                Guid contractExtId = result.FirstOrDefault().ContractExtensions.Id;

                List<ContractExtensionMake> ContractExtensionsMakes = null;
                List<ContractExtensionModel> ContractExtensionsModeles = null;
                List<ContractExtensionVariant> ContractExtensionsVariants = null;
                List<ContractExtensionManufacturerWarranty> ContractExtensionManufacturerWarranties = null;
                List<ContractExtensionEngineCapacity> ContractExtensionsEngineCapacities = null;
                List<ContractExtensionCylinderCount> ContractExtensionsCylinderCounts = null;
                List<ContractExtensionsPremiumAddon> ContractExtensionsPremiumAddons = null;

                IEnumerable<ContractExtensionMake> ContractExtensionsMakesData = session.Query<ContractExtensionMake>();
                IEnumerable<ContractExtensionModel> ContractExtensionsModelsData = session.Query<ContractExtensionModel>();
                IEnumerable<ContractExtensionVariant> ContractExtensionVariantData = session.Query<ContractExtensionVariant>();
                IEnumerable<ContractExtensionManufacturerWarranty> ContractExtensionManufacturerWarrantiesData = session.Query<ContractExtensionManufacturerWarranty>();
                IEnumerable<ContractExtensionEngineCapacity> ContractExtensionsEngineCapacitysData = session.Query<ContractExtensionEngineCapacity>();
                IEnumerable<ContractExtensionCylinderCount> ContractExtensionsCylinderCountsData = session.Query<ContractExtensionCylinderCount>();
                IEnumerable<ContractExtensionsPremiumAddon> ContractExtensionsPremiumAddonsData = session.Query<ContractExtensionsPremiumAddon>();

                ContractExtensionsMakes = ContractExtensionsMakesData.Where(x => x.ContractExtensionId == contractExtId).ToList();
                ContractExtensionsModeles = ContractExtensionsModelsData.Where(x => x.ContractExtensionId == contractExtId).ToList();
                ContractExtensionsVariants = ContractExtensionVariantData.Where(a => a.ContractExtensionId == contractExtId).ToList();
                ContractExtensionManufacturerWarranties = ContractExtensionManufacturerWarrantiesData.Where(x => x.ContractExtensionId == contractExtId).ToList();
                ContractExtensionsEngineCapacities = ContractExtensionsEngineCapacitysData.Where(x => x.ContractExtensionId == contractExtId).ToList();
                ContractExtensionsCylinderCounts = ContractExtensionsCylinderCountsData.Where(x => x.ContractExtensionId == contractExtId).ToList();
                ContractExtensionsPremiumAddons = ContractExtensionsPremiumAddonsData.Where(x => x.ContractExtensionId == contractExtId).ToList();

                List<ContractExtensionsPremiumAddonResponseDto> addons = new List<ContractExtensionsPremiumAddonResponseDto>();
                foreach (var item in ContractExtensionsPremiumAddons)
                {
                    addons.Add(new ContractExtensionsPremiumAddonResponseDto()
                    {
                        Id = item.Id,
                        ContractExtensionId = item.ContractExtensionId,
                        PremiumAddonTypeId = item.PremiumAddonTypeId,
                        Value = item.Value
                    });
                }

                if (result != null && result.Count > 0)
                {
                    pr.Id = result.First().ContractExtensions.Id;
                  //  pr.AttributeSpecification = result.First().ContractExtensions.AttributeSpecification;
                  //  pr.ContractId = result.First().ContractExtensions.ContractId;
                   // pr.IsCustAvailableGross = result.First().ContractExtensions.IsCustAvailableGross;
                   // pr.IsCustAvailableNett = result.First().ContractExtensions.IsCustAvailableNett;

                  //  pr.ManufacturerWarrantyGross = result.First().ContractExtensions.ManufacturerWarrantyGross;
                  //  pr.ManufacturerWarrantyNett = result.First().ContractExtensions.ManufacturerWarrantyNett;

                    //pr.WarrantyTypeId = result.First().ContractExtensions.WarrantyTypeId;
                    //pr.ExtensionTypeId = result.First().ContractExtensions.ExtensionTypeId;
                    //pr.MaxGross = result.First().ContractExtensions.MaxGross;
                    //pr.MaxNett = result.First().ContractExtensions.MaxNett;

                    //pr.MinGross = result.First().ContractExtensions.MinGross;
                    //pr.MinNett = result.First().ContractExtensions.MinNett;

                    //pr.PremiumBasedOnIdGross = result.First().ContractExtensions.PremiumBasedOnIdGross;
                    //pr.PremiumBasedOnIdNett = result.First().ContractExtensions.PremiumBasedOnIdNett;

                    //pr.PremiumTotal = result.First().ContractExtensions.PremiumTotal;
                    //pr.GrossPremium = result.First().ContractExtensions.GrossPremium;
                    pr.RSAProviderId = result.First().ContractExtensions.RSAProviderId;
                    pr.RegionId = result.First().ContractExtensions.RegionId;
                    pr.Rate = result.First().ContractExtensions.Rate;
                    //pr.PremiumCurrencyId = result.First().ContractExtensions.PremiumCurrencyId;
                    pr.Makes = ContractExtensionsMakes.Select(c => c.MakeId).ToList();
                    if (ContractExtensionsVariants != null)
                        pr.Variants = ContractExtensionsVariants.Select(a => a.VariantId).ToList();
                    pr.Modeles = ContractExtensionsModeles.Select(c => c.ModelId).ToList();
                    pr.EngineCapacities = ContractExtensionsEngineCapacities.Select(c => c.EngineCapacityId).ToList();
                    pr.CylinderCounts = ContractExtensionsCylinderCounts.Select(c => c.CylinderCountId).ToList();
                    pr.PremiumAddones = addons;
                    pr.EntryDateTime = result.First().ContractExtensions.EntryDateTime;
                    pr.EntryUser = result.First().ContractExtensions.EntryUser;
                    pr.isAllCyllinderCount = result.First().ContractExtensions.IsAllCyllinderCountsSelected;
                    pr.isAllEngineCapacities = result.First().ContractExtensions.IsAllEngineCapacitiesSelected;
                    pr.isAllMakes = result.First().ContractExtensions.IsAllMakesSelected;
                    pr.isAllModels = result.First().ContractExtensions.IsAllModelsSelected;
                    pr.isAllVariants = result.First().ContractExtensions.IsAllVariantsSelected;
                    pr.IsContractExtensionsExists = true;
                    return pr;
                }
                else
                {
                    pr.IsContractExtensionsExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

            
        }

        //internal bool AddContractExtensions(ContractExtensionsRequestDto ContractExtensions)
        //{
        //    try
        //    {
        //        ISession session = EntitySessionManager.GetSession();
        //        ContractExtensions pr = new Entities.ContractExtensions();

        //        pr.Id = new Guid();
        //        pr.AttributeSpecification = ContractExtensions.AttributeSpecification;
        //        pr.ContractId = ContractExtensions.ContractId;
        //        pr.IsCustAvailable = ContractExtensions.IsCustAvailable;
        //        pr.ManufacturerWarranty = ContractExtensions.ManufacturerWarranty;
        //        pr.WarrantyTypeId = ContractExtensions.WarrantyTypeId;
        //        pr.ExtensionTypeId = ContractExtensions.ExtensionTypeId;
        //        pr.Max = ContractExtensions.Max;
        //        pr.Min = ContractExtensions.Min;
        //        pr.PremiumBasedOnId = ContractExtensions.PremiumBasedOnId;
        //        pr.PremiumTotal = ContractExtensions.PremiumTotal;
        //        pr.PremiumCurrencyId = ContractExtensions.PremiumCurrencyId;
        //        pr.GrossPremium = ContractExtensions.GrossPremium;
        //        pr.RSAProviderId = ContractExtensions.RSAProviderId;
        //        pr.RegionId = ContractExtensions.RegionId;
        //        pr.Rate = ContractExtensions.Rate;
        //        pr.EntryDateTime = DateTime.Today.ToUniversalTime();
        //        pr.EntryUser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");

        //        using (ITransaction transaction = session.BeginTransaction())
        //        {
        //            session.SaveOrUpdate(pr);
        //            ContractExtensions.Id = pr.Id;
        //            foreach (var item in ContractExtensions.EngineCapacities)
        //            {
        //                ContractExtensionEngineCapacity cc = new Entities.ContractExtensionEngineCapacity();
        //                cc.Id = new Guid();
        //                cc.EngineCapacityId = item;
        //                cc.ContractExtensionId = pr.Id;
        //                session.SaveOrUpdate(cc);
        //            }
        //            foreach (var item in ContractExtensions.Makes)
        //            {
        //                ContractExtensionMake cc = new Entities.ContractExtensionMake();
        //                cc.Id = new Guid();
        //                cc.MakeId = item;
        //                cc.ContractExtensionId = pr.Id;
        //                session.SaveOrUpdate(cc);
        //            }
        //            foreach (var item in ContractExtensions.Modeles)
        //            {
        //                ContractExtensionModel cc = new Entities.ContractExtensionModel();
        //                cc.Id = new Guid();
        //                cc.ModelId = item;
        //                cc.ContractExtensionId = pr.Id;
        //                session.SaveOrUpdate(cc);
        //            }             
        //            foreach (var item in ContractExtensions.CylinderCounts)
        //            {
        //                ContractExtensionCylinderCount cc = new Entities.ContractExtensionCylinderCount();
        //                cc.Id = new Guid();
        //                cc.CylinderCountId = item;
        //                cc.ContractExtensionId = pr.Id;
        //                session.SaveOrUpdate(cc);
        //            }
        //            if (ContractExtensions.PremiumAddones != null)
        //            {
        //                foreach (var item in ContractExtensions.PremiumAddones)
        //                {
        //                    ContractExtensionsPremiumAddon cc = new Entities.ContractExtensionsPremiumAddon();
        //                    cc.Id = new Guid();
        //                    cc.PremiumAddonTypeId = item.PremiumAddonTypeId;
        //                    cc.ContractExtensionId = pr.Id;
        //                    cc.Value = item.Value;
        //                    session.SaveOrUpdate(cc);

        //                }
        //            }
        //            transaction.Commit();
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {

        //        return false;
        //    }
        //}

        //internal bool UpdateContractExtensions(ContractExtensionsRequestDto ContractExtensions)
        //{
        //    try
        //    {
        //        ISession session = EntitySessionManager.GetSession();
        //        ContractExtensions pr = new Entities.ContractExtensions();
        //        pr.Id = ContractExtensions.Id;
        //        pr.AttributeSpecification = ContractExtensions.AttributeSpecification;
        //        pr.ContractId = ContractExtensions.ContractId;
        //        pr.IsCustAvailable = ContractExtensions.IsCustAvailable;
        //        pr.ManufacturerWarranty = ContractExtensions.ManufacturerWarranty;
        //        pr.WarrantyTypeId = ContractExtensions.WarrantyTypeId;
        //        pr.ExtensionTypeId = ContractExtensions.ExtensionTypeId;
        //        pr.Max = ContractExtensions.Max;
        //        pr.Min = ContractExtensions.Min;
        //        pr.PremiumBasedOnId = ContractExtensions.PremiumBasedOnId;
        //        pr.PremiumTotal = ContractExtensions.PremiumTotal;
        //        pr.GrossPremium = ContractExtensions.GrossPremium;
        //        pr.PremiumCurrencyId = ContractExtensions.PremiumCurrencyId;
        //        pr.EntryDateTime = ContractExtensions.EntryDateTime;
        //        pr.EntryUser = ContractExtensions.EntryUser;
        //        pr.RegionId = ContractExtensions.RegionId;
        //        pr.RSAProviderId = ContractExtensions.RSAProviderId;
        //        pr.Rate = ContractExtensions.Rate;

        //        using (ITransaction transaction = session.BeginTransaction())
        //        {
        //            session.Update(pr);

        //            #region  Add New
        //            foreach (var item in ContractExtensions.EngineCapacities)
        //            {
        //                var query = from ContractExtensionsEngineCapacity in session.Query<ContractExtensionEngineCapacity>()
        //                            where ContractExtensionsEngineCapacity.EngineCapacityId == item && ContractExtensionsEngineCapacity.ContractExtensionId == ContractExtensions.Id
        //                            select new { Id = ContractExtensionsEngineCapacity.EngineCapacityId };
        //                if (query.ToList().Count == 0)
        //                {
        //                    ContractExtensionEngineCapacity cc = new Entities.ContractExtensionEngineCapacity();
        //                    cc.Id = new Guid();
        //                    cc.EngineCapacityId = item;
        //                    cc.ContractExtensionId = ContractExtensions.Id;
        //                    session.SaveOrUpdate(cc);
        //                }
        //            }
        //            foreach (var item in ContractExtensions.Makes)
        //            {
        //                var query = from ContractExtensionsMake in session.Query<ContractExtensionMake>()
        //                            where ContractExtensionsMake.MakeId == item && ContractExtensionsMake.ContractExtensionId == ContractExtensions.Id
        //                            select new { Id = ContractExtensionsMake.MakeId };
        //                if (query.ToList().Count == 0)
        //                {
        //                    ContractExtensionMake cc = new Entities.ContractExtensionMake();
        //                    cc.Id = new Guid();
        //                    cc.MakeId = item;
        //                    cc.ContractExtensionId = ContractExtensions.Id;
        //                    session.SaveOrUpdate(cc);
        //                }
        //            }                 
        //            foreach (var item in ContractExtensions.Modeles)
        //            {
        //                var query = from ContractExtensionsModel in session.Query<ContractExtensionModel>()
        //                            where ContractExtensionsModel.ModelId == item && ContractExtensionsModel.ContractExtensionId == ContractExtensions.Id
        //                            select new { Id = ContractExtensionsModel.ModelId };
        //                if (query.ToList().Count == 0)
        //                {
        //                    ContractExtensionModel cc = new Entities.ContractExtensionModel();
        //                    cc.Id = new Guid();
        //                    cc.ModelId = item;
        //                    cc.ContractExtensionId = ContractExtensions.Id;
        //                    session.SaveOrUpdate(cc);
        //                }
        //            }                  
        //            foreach (var item in ContractExtensions.CylinderCounts)
        //            {
        //                var query = from ContractExtensionsCylinderCount in session.Query<ContractExtensionCylinderCount>()
        //                            where ContractExtensionsCylinderCount.CylinderCountId == item && ContractExtensionsCylinderCount.ContractExtensionId == ContractExtensions.Id
        //                            select new { Id = ContractExtensionsCylinderCount.CylinderCountId };
        //                if (query.ToList().Count == 0)
        //                {
        //                    ContractExtensionCylinderCount cc = new Entities.ContractExtensionCylinderCount();
        //                    cc.Id = new Guid();
        //                    cc.CylinderCountId = item;
        //                    cc.ContractExtensionId = ContractExtensions.Id;
        //                    session.SaveOrUpdate(cc);
        //                }
        //            }
        //            foreach (var item in ContractExtensions.PremiumAddones)
        //            {
        //                var query = from ContractExtensionsPremiumAddon in session.Query<ContractExtensionsPremiumAddon>()
        //                            where ContractExtensionsPremiumAddon.PremiumAddonTypeId == item.PremiumAddonTypeId && ContractExtensionsPremiumAddon.ContractExtensionId == ContractExtensions.Id
        //                            select new { Id = ContractExtensionsPremiumAddon.PremiumAddonTypeId };
        //                if (query.ToList().Count == 0)
        //                {
        //                    ContractExtensionsPremiumAddon cc = new Entities.ContractExtensionsPremiumAddon();
        //                    cc.Id = new Guid();
        //                    cc.PremiumAddonTypeId = item.PremiumAddonTypeId;
        //                    cc.ContractExtensionId = ContractExtensions.Id;
        //                    cc.Value = item.Value;
        //                    session.SaveOrUpdate(cc);
        //                }
        //                else
        //                {
        //                    ContractExtensionsPremiumAddon cc = new Entities.ContractExtensionsPremiumAddon();
        //                    cc.Id = item.Id;
        //                    cc.PremiumAddonTypeId = item.PremiumAddonTypeId;
        //                    cc.ContractExtensionId = item.ContractExtensionId;
        //                    cc.Value = item.Value;
        //                    session.Update(cc);
        //                }
        //            }   
        //            #endregion

        //            #region Delete Removed 
        //            var ContractExtensionsEngineCapacitiesqueryDeleted = from ContractExtensionEngineCapacity in session.Query<ContractExtensionEngineCapacity>()
        //                                                                 where ContractExtensionEngineCapacity.ContractExtensionId == ContractExtensions.Id
        //                                                                 select ContractExtensionEngineCapacity;
        //            var ContractExtensionsEngineCapacitieslist = ContractExtensionsEngineCapacitiesqueryDeleted.ToList();
        //            foreach (var item in ContractExtensionsEngineCapacitieslist)
        //            {
        //                if (ContractExtensions.EngineCapacities.Contains(item.EngineCapacityId) == false)
        //                {
        //                    session.Delete(item);
        //                }
        //            }
        //            var ContractExtensionsMakesqueryDeleted = from ContractExtensionMake in session.Query<ContractExtensionMake>()
        //                                                      where ContractExtensionMake.ContractExtensionId == ContractExtensions.Id
        //                                                      select ContractExtensionMake;
        //            var ContractExtensionsMakeslist = ContractExtensionsMakesqueryDeleted.ToList();
        //            foreach (var item in ContractExtensionsMakeslist)
        //            {
        //                if (ContractExtensions.Makes.Contains(item.MakeId) == false)
        //                {
        //                    session.Delete(item);
        //                }
        //            }
        //            var ContractExtensionsModelesqueryDeleted = from ContractExtensionModel in session.Query<ContractExtensionModel>()
        //                                                        where ContractExtensionModel.ContractExtensionId == ContractExtensions.Id
        //                                                        select ContractExtensionModel;
        //            var ContractExtensionsModeleslist = ContractExtensionsModelesqueryDeleted.ToList();
        //            foreach (var item in ContractExtensionsModeleslist)
        //            {
        //                if (ContractExtensions.Modeles.Contains(item.ModelId) == false)
        //                {
        //                    session.Delete(item);
        //                }
        //            }

        //            var ContractExtensionsCylinderCountqueryDeleted = from ContractExtensionCylinderCount in session.Query<ContractExtensionCylinderCount>()
        //                                                              where ContractExtensionCylinderCount.ContractExtensionId == ContractExtensions.Id
        //                                                              select ContractExtensionCylinderCount;
        //            var ContractExtensionsCylinderCountlist = ContractExtensionsCylinderCountqueryDeleted.ToList();
        //            foreach (var item in ContractExtensionsCylinderCountlist)
        //            {
        //                if (ContractExtensions.CylinderCounts.Contains(item.CylinderCountId) == false)
        //                {
        //                    session.Delete(item);
        //                }
        //            }
        //            //var ContractExtensionsPremiumAddonqueryDeleted = from ContractExtensionsPremiumAddon in session.Query<ContractExtensionsPremiumAddon>()
        //            //                                                 where ContractExtensionsPremiumAddon.ContractExtensionId == ContractExtensions.Id
        //            //                                                 select ContractExtensionsPremiumAddon;
        //            //var ContractExtensionsPremiumAddonlist = ContractExtensionsPremiumAddonqueryDeleted.ToList();
        //            //foreach (var item in ContractExtensionsPremiumAddonlist)
        //            //{
        //            //    if (ContractExtensions.PremiumAddones.FindAll(p=> p.Id ==item.PremiumAddonTypeId).Count==0)
        //            //    {
        //            //        session.Delete(item);
        //            //    }
        //            //}    

        //            #endregion

        //            transaction.Commit();
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
    }
}
