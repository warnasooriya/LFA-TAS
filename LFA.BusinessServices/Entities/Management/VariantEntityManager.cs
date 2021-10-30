using NHibernate;
using NHibernate.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.Entities.Management
{
    public class VariantEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<VariantResponseDto> GetVariants()
        {

            List<VariantResponseDto> result = new List<VariantResponseDto>();

            try
            {
                ISession session = EntitySessionManager.GetSession();
                List<VariantAspirations> VariantAspirations = session.Query<VariantAspirations>().ToList();
                List<VariantBodyTypes> VariantBodyTypes = session.Query<VariantBodyTypes>().ToList();
                List<VariantCountrys> VariantCountrys = session.Query<VariantCountrys>().ToList();
                List<VariantDriveTypes> VariantDriveTypes = session.Query<VariantDriveTypes>().ToList();
                List<VariantFuelTypes> VariantFuelTypes = session.Query<VariantFuelTypes>().ToList();
                List<VariantTransmissions> VariantTransmissions = session.Query<VariantTransmissions>().ToList();

                IQueryable<Variant> VariantData = session.Query<Variant>();
                foreach (var item in VariantData)
                {
                    List<Guid> VariantAspirationsList = VariantAspirations.Where(x => x.VariantId == item.Id).Select(a => a.AspirationId).ToList();
                    List<Guid> VariantBodyTypesList = VariantBodyTypes.Where(x => x.VariantId == item.Id).Select(a=>a.BodyTypeId).ToList();
                    List<Guid> VariantCountrysList = VariantCountrys.Where(x => x.VariantId == item.Id).Select(a=>a.CountryId).ToList();
                    List<Guid> VariantDriveTypesList = VariantDriveTypes.Where(x => x.VariantId == item.Id).Select(a => a.DriveTypeId).ToList();
                    List<Guid> VariantFuelTypesList = VariantFuelTypes.Where(x => x.VariantId == item.Id).Select(a=>a.FuelTypeId).ToList();
                    List<Guid> VariantTransmissionsList = VariantTransmissions.Where(x => x.VariantId == item.Id).Select(a=>a.TransmissionId).ToList();

                    VariantResponseDto VariantInfo = new VariantResponseDto()
                    {
                        Id = item.Id,
                        CommodityTypeId = item.CommodityTypeId,
                        VariantName = item.VariantName,
                        FromModelYear = item.FromModelYear,
                        ToModelYear = item.ToModelYear,
                        EngineCapacityId = item.EngineCapacityId,
                        CylinderCountId = item.CylinderCountId,
                        BodyCode = item.BodyCode,
                        IsActive = item.IsActive,
                        ModelId = item.ModelId,
                        DriveTypes = VariantDriveTypesList,
                        BodyTypes = VariantBodyTypesList,
                        FuelTypes = VariantFuelTypesList,
                        Transmissions = VariantTransmissionsList,
                        Aspirations = VariantAspirationsList,
                        Countrys = VariantCountrysList,
                        EntryDateTime = item.EntryDateTime,
                        EntryUser = item.EntryUser
                    };
                    result.Add(VariantInfo);
                }


            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return result;
        }

        internal VariantsResponseDto GetVariantsByModelId(Guid modelId)
        {
            throw new NotImplementedException();
        }

        public VariantResponseDto GetVariantById(Guid VariantId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                VariantResponseDto pDto = new VariantResponseDto();

                var query =
                    from Variant in session.Query<Variant>()
                    where Variant.Id == VariantId
                    select new { Variant = Variant };

                var result = query.ToList();

                List<VariantAspirations> VariantAspirations = null;
                List<VariantBodyTypes> VariantBodyTypes = null;
                List<VariantCountrys> VariantCountrys = null;
                List<VariantDriveTypes> VariantDriveTypes = null;
                List<VariantFuelTypes> VariantFuelTypes = null;
                List<VariantTransmissions> VariantTransmissions = null;
                List<VariantPremiumAddon> VariantPremiumAddon = null;

                IQueryable<VariantAspirations> VariantAspirationsData = session.Query<VariantAspirations>();
                IQueryable<VariantBodyTypes> VariantBodyTypesData = session.Query<VariantBodyTypes>();
                IQueryable<VariantCountrys> VariantCountrysData = session.Query<VariantCountrys>();
                IQueryable<VariantDriveTypes> VariantDriveTypesData = session.Query<VariantDriveTypes>();
                IQueryable<VariantFuelTypes> VariantFuelTypesData = session.Query<VariantFuelTypes>();
                IQueryable<VariantTransmissions> VariantTransmissionsData = session.Query<VariantTransmissions>();
                IQueryable<VariantPremiumAddon> VariantPremiumAddonData = session.Query<VariantPremiumAddon>();


                VariantAspirations = VariantAspirationsData.Where(x => x.VariantId == VariantId).ToList();
                VariantBodyTypes = VariantBodyTypesData.Where(x => x.VariantId == VariantId).ToList();
                VariantCountrys = VariantCountrysData.Where(x => x.VariantId == VariantId).ToList();
                VariantDriveTypes = VariantDriveTypesData.Where(x => x.VariantId == VariantId).ToList();
                VariantFuelTypes = VariantFuelTypesData.Where(x => x.VariantId == VariantId).ToList();
                VariantTransmissions = VariantTransmissionsData.Where(x => x.VariantId == VariantId).ToList();
                VariantPremiumAddon = VariantPremiumAddonData.Where(x => x.VariantId == VariantId).ToList();
                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().Variant.Id;
                    pDto.CommodityTypeId = result.First().Variant.CommodityTypeId;
                    pDto.VariantName = result.First().Variant.VariantName;
                    pDto.FromModelYear = result.First().Variant.FromModelYear;
                    pDto.ToModelYear = result.First().Variant.ToModelYear;
                    pDto.EngineCapacityId = result.First().Variant.EngineCapacityId;
                    pDto.CylinderCountId = result.First().Variant.CylinderCountId;
                    pDto.BodyCode = result.First().Variant.BodyCode;
                    pDto.IsActive = result.First().Variant.IsActive;
                    pDto.ModelId = result.First().Variant.ModelId;
                    pDto.DriveTypes = VariantDriveTypes.Select(c => c.DriveTypeId).ToList();
                    pDto.BodyTypes = VariantBodyTypes.Select(c => c.BodyTypeId).ToList();
                    pDto.PremiumAddonType = VariantPremiumAddon.Select(c => c.PremiumAddonTypeId).ToList();
                    pDto.FuelTypes = VariantFuelTypes.Select(c => c.FuelTypeId).ToList();
                    pDto.Transmissions = VariantTransmissions.Select(c => c.TransmissionId).ToList();
                    pDto.Aspirations = VariantAspirations.Select(c => c.AspirationId).ToList();
                    pDto.Countrys = VariantCountrys.Select(c => c.CountryId).ToList();
                    pDto.FromModelYear = result.First().Variant.FromModelYear;
                    pDto.ToModelYear = result.First().Variant.ToModelYear;
                    pDto.EntryDateTime = result.First().Variant.EntryDateTime;
                    pDto.EntryUser = result.First().Variant.EntryUser;
                    pDto.GrossWeight = result.First().Variant.GrossWeight;
                    //pDto.IsForuByFour = result.First().Variant.IsForuByFour;
                    //pDto.IsSports = result.First().Variant.IsSports;
                    pDto.IsVariantExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsVariantExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        internal bool AddVariant(VariantRequestDto Variant)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                Variant pr = new Entities.Variant();

                pr.Id = new Guid();
                pr.CommodityTypeId = Variant.CommodityTypeId;
                pr.VariantName = Variant.VariantName;
                pr.FromModelYear = Variant.FromModelYear;
                pr.ToModelYear = Variant.ToModelYear;
                pr.EngineCapacityId = Variant.EngineCapacityId;
                pr.CylinderCountId = Variant.CylinderCountId;
                pr.BodyCode = Variant.BodyCode;
                pr.IsActive = Variant.IsActive;
                pr.ModelId = Variant.ModelId;
                pr.FromModelYear = Variant.FromModelYear;
                pr.ToModelYear = Variant.ToModelYear;
                pr.GrossWeight = Variant.GrossWeight;
                //pr.IsForuByFour = Variant.IsForuByFour;
                //pr.IsSports = Variant.IsSports;
                pr.EntryDateTime = DateTime.Today.ToUniversalTime();
                pr.EntryUser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);
                    foreach (var item in Variant.Countrys)
                    {
                        VariantCountrys cc = new Entities.VariantCountrys();
                        cc.Id = new Guid();
                        cc.CountryId = item;
                        cc.VariantId = pr.Id;
                        session.SaveOrUpdate(cc);
                    }
                    foreach (var item in Variant.Aspirations)
                    {
                        VariantAspirations cc = new Entities.VariantAspirations();
                        cc.Id = new Guid();
                        cc.AspirationId = item;
                        cc.VariantId = pr.Id;
                        session.SaveOrUpdate(cc);
                    }
                    foreach (var item in Variant.BodyTypes)
                    {
                        VariantBodyTypes cc = new Entities.VariantBodyTypes();
                        cc.Id = new Guid();
                        cc.BodyTypeId = item;
                        cc.VariantId = pr.Id;
                        session.SaveOrUpdate(cc);
                    }
                    foreach (var item in Variant.DriveTypes)
                    {
                        VariantDriveTypes cc = new Entities.VariantDriveTypes();
                        cc.Id = new Guid();
                        cc.DriveTypeId = item;
                        cc.VariantId = pr.Id;
                        session.SaveOrUpdate(cc);
                    }
                    foreach (var item in Variant.FuelTypes)
                    {
                        VariantFuelTypes cc = new Entities.VariantFuelTypes();
                        cc.Id = new Guid();
                        cc.FuelTypeId = item;
                        cc.VariantId = pr.Id;
                        session.SaveOrUpdate(cc);
                    }
                    foreach (var item in Variant.Transmissions)
                    {
                        VariantTransmissions cc = new Entities.VariantTransmissions();
                        cc.Id = new Guid();
                        cc.TransmissionId = item;
                        cc.VariantId = pr.Id;
                        session.SaveOrUpdate(cc);
                    }
                    foreach (var item in Variant.PremiumAddonType)
                    {
                        VariantPremiumAddon cc = new Entities.VariantPremiumAddon();
                        cc.Id = new Guid();
                        cc.VariantId = pr.Id;
                        cc.PremiumAddonTypeId = item;
                        session.SaveOrUpdate(cc);
                    }
                    transaction.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }

        internal bool UpdateVariant(VariantRequestDto Variant)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Variant pr = new Entities.Variant();
                pr.Id = Variant.Id;
                pr.CommodityTypeId = Variant.CommodityTypeId;
                pr.VariantName = Variant.VariantName;
                pr.FromModelYear = Variant.FromModelYear;
                pr.ToModelYear = Variant.ToModelYear;
                pr.EngineCapacityId = Variant.EngineCapacityId;
                pr.CylinderCountId = Variant.CylinderCountId;
                pr.BodyCode = Variant.BodyCode;
                pr.IsActive = Variant.IsActive;
                pr.ModelId = Variant.ModelId;
                pr.FromModelYear = Variant.FromModelYear;
                pr.ToModelYear = Variant.ToModelYear;
                pr.EntryDateTime = Variant.EntryDateTime;
                pr.EntryUser = Variant.EntryUser;
                pr.GrossWeight = Variant.GrossWeight;
                //pr.IsSports = Variant.IsSports;
                //pr.IsForuByFour = Variant.IsForuByFour;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(pr);

                    #region  Add New
                    foreach (var item in Variant.Countrys)
                    {
                        var query = from VariantCountry in session.Query<VariantCountrys>()
                                    where VariantCountry.CountryId == item && VariantCountry.VariantId == pr.Id
                                    select new { Id = VariantCountry.CountryId };
                        if (query.ToList().Count == 0)
                        {
                            VariantCountrys cc = new Entities.VariantCountrys();
                            cc.Id = new Guid();
                            cc.CountryId = item;
                            cc.VariantId = Variant.Id;
                            session.SaveOrUpdate(cc);
                        }
                    }
                    foreach (var item in Variant.Aspirations)
                    {
                        var query = from VariantAspiration in session.Query<VariantAspirations>()
                                    where VariantAspiration.AspirationId == item && VariantAspiration.VariantId == pr.Id
                                    select new { Id = VariantAspiration.AspirationId };
                        if (query.ToList().Count == 0)
                        {
                            VariantAspirations cc = new Entities.VariantAspirations();
                            cc.Id = new Guid();
                            cc.AspirationId = item;
                            cc.VariantId = Variant.Id;
                            session.SaveOrUpdate(cc);
                        }
                    }
                    foreach (var item in Variant.FuelTypes)
                    {
                        var query = from VariantFuelType in session.Query<VariantFuelTypes>()
                                    where VariantFuelType.FuelTypeId == item && VariantFuelType.VariantId == pr.Id
                                    select new { Id = VariantFuelType.FuelTypeId };
                        if (query.ToList().Count == 0)
                        {
                            VariantFuelTypes cc = new Entities.VariantFuelTypes();
                            cc.Id = new Guid();
                            cc.FuelTypeId = item;
                            cc.VariantId = Variant.Id;
                            session.SaveOrUpdate(cc);
                        }
                    }
                    foreach (var item in Variant.BodyTypes)
                    {
                        var query = from VariantBodyType in session.Query<VariantBodyTypes>()
                                    where VariantBodyType.BodyTypeId == item && VariantBodyType.VariantId == pr.Id
                                    select new { Id = VariantBodyType.BodyTypeId };
                        if (query.ToList().Count == 0)
                        {
                            VariantBodyTypes cc = new Entities.VariantBodyTypes();
                            cc.Id = new Guid();
                            cc.BodyTypeId = item;
                            cc.VariantId = Variant.Id;
                            session.SaveOrUpdate(cc);
                        }
                    }
                    foreach (var item in Variant.DriveTypes)
                    {
                        var query = from VariantDriveType in session.Query<VariantDriveTypes>()
                                    where VariantDriveType.DriveTypeId == item && VariantDriveType.VariantId == pr.Id
                                    select new { Id = VariantDriveType.DriveTypeId };
                        if (query.ToList().Count == 0)
                        {
                            VariantDriveTypes cc = new Entities.VariantDriveTypes();
                            cc.Id = new Guid();
                            cc.DriveTypeId = item;
                            cc.VariantId = Variant.Id;
                            session.SaveOrUpdate(cc);
                        }
                    }
                    foreach (var item in Variant.Transmissions)
                    {
                        var query = from VariantTransmission in session.Query<VariantTransmissions>()
                                    where VariantTransmission.TransmissionId == item && VariantTransmission.VariantId == pr.Id
                                    select new { Id = VariantTransmission.TransmissionId };
                        if (query.ToList().Count == 0)
                        {
                            VariantTransmissions cc = new Entities.VariantTransmissions();
                            cc.Id = new Guid();
                            cc.TransmissionId = item;
                            cc.VariantId = Variant.Id;
                            session.SaveOrUpdate(cc);
                        }
                    }
                    foreach (var item in Variant.PremiumAddonType)
                    {
                        var query = from VariantPremiumAddons in session.Query<VariantPremiumAddon>()
                                    where VariantPremiumAddons.PremiumAddonTypeId == item && VariantPremiumAddons.VariantId == pr.Id
                                    select new { Id = VariantPremiumAddons.PremiumAddonTypeId };
                        if (query.ToList().Count == 0)
                        {
                            VariantPremiumAddon cc = new Entities.VariantPremiumAddon();
                            cc.Id = new Guid();
                            cc.PremiumAddonTypeId = item;
                            cc.VariantId = Variant.Id;
                            session.SaveOrUpdate(cc);
                        }
                    }
                    #endregion

                    #region Delete Removed
                    var VariantCountrysqueryDeleted = from VariantCountry in session.Query<VariantCountrys>()
                                                      where VariantCountry.VariantId == Variant.Id
                                                      select VariantCountry;
                    var VariantCountryslist = VariantCountrysqueryDeleted.ToList();
                    foreach (var item in VariantCountryslist)
                    {
                        if (Variant.Countrys.Contains(item.CountryId) == false)
                        {
                            session.Delete(item);
                        }
                    }
                    var VariantAspirationsqueryDeleted = from VariantAspiration in session.Query<VariantAspirations>()
                                                         where VariantAspiration.VariantId == Variant.Id
                                                         select VariantAspiration;
                    var VariantAspirationslist = VariantAspirationsqueryDeleted.ToList();
                    foreach (var item in VariantAspirationslist)
                    {
                        if (Variant.Aspirations.Contains(item.AspirationId) == false)
                        {
                            session.Delete(item);
                        }
                    }
                    var VariantBodyTypesqueryDeleted = from VariantBodyType in session.Query<VariantBodyTypes>()
                                                       where VariantBodyType.VariantId == Variant.Id
                                                       select VariantBodyType;
                    var VariantBodyTypeslist = VariantBodyTypesqueryDeleted.ToList();
                    foreach (var item in VariantBodyTypeslist)
                    {
                        if (Variant.BodyTypes.Contains(item.BodyTypeId) == false)
                        {
                            session.Delete(item);
                        }
                    }
                    var VariantDriveTypesqueryDeleted = from VariantDriveType in session.Query<VariantDriveTypes>()
                                                        where VariantDriveType.VariantId == Variant.Id
                                                        select VariantDriveType;
                    var VariantDriveTypeslist = VariantDriveTypesqueryDeleted.ToList();
                    foreach (var item in VariantDriveTypeslist)
                    {
                        if (Variant.DriveTypes.Contains(item.DriveTypeId) == false)
                        {
                            session.Delete(item);
                        }
                    }
                    var VariantFuelTypesqueryDeleted = from VariantFuelType in session.Query<VariantFuelTypes>()
                                                       where VariantFuelType.VariantId == Variant.Id
                                                       select VariantFuelType;
                    var VariantFuelTypeslist = VariantFuelTypesqueryDeleted.ToList();
                    foreach (var item in VariantFuelTypeslist)
                    {
                        if (Variant.FuelTypes.Contains(item.FuelTypeId) == false)
                        {
                            session.Delete(item);
                        }
                    }
                    var VariantTransmissionsqueryDeleted = from VariantTransmission in session.Query<VariantTransmissions>()
                                                           where VariantTransmission.VariantId == Variant.Id
                                                           select VariantTransmission;
                    var VariantTransmissionslist = VariantTransmissionsqueryDeleted.ToList();
                    foreach (var item in VariantTransmissionslist)
                    {
                        if (Variant.Transmissions.Contains(item.TransmissionId) == false)
                        {
                            session.Delete(item);
                        }
                    }
                    var VariantPremiumAddonqueryDeleted = from VariantPremiumAddons in session.Query<VariantPremiumAddon>()
                                                          where VariantPremiumAddons.VariantId == Variant.Id
                                                          select VariantPremiumAddons;
                    var VariantPremiumAddonslist = VariantPremiumAddonqueryDeleted.ToList();
                    foreach (var item in VariantPremiumAddonslist)
                    {
                        if (Variant.PremiumAddonType.Contains(item.PremiumAddonTypeId) == false)
                        {
                            session.Delete(item);
                        }
                    }
                    #endregion

                    transaction.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }

        public static VariantsResponseDto GetAllVarientsByModelIds(List<Guid> ModelIdList)
        {
            VariantsResponseDto Response = new VariantsResponseDto();
            try
            {
                ISession session = EntitySessionManager.GetSession();

                Response.Variants = session.Query<Variant>()
                    .Where(a => a.IsActive && ModelIdList.Contains(a.ModelId)).Select( b=> new VariantResponseDto { Id = b.Id , VariantName= b.VariantName }).ToList();

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }
    }
}
