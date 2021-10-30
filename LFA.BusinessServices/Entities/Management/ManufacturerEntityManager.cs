using TAS.Services.Entities.Persistence;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System.Reflection;
using NLog;

namespace TAS.Services.Entities.Management
{
    public class ManufacturerEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        internal List<ManufacturerResponseDto> GetAllManufacturers()
        {
            List<ManufacturerComodityTypes> ManufacturerComodityTypes = null;
            List<ManufacturerResponseDto> ManufacturerWithCommodityTypes = new List<ManufacturerResponseDto>();

            try
            {
                ISession session = EntitySessionManager.GetSession();

                IQueryable<Manufacturer> ManufacturerData = session.Query<Manufacturer>();
                foreach (var item in ManufacturerData)
                {
                    IQueryable<ManufacturerComodityTypes> ManufacturerComodityTypeData = session.Query<ManufacturerComodityTypes>();
                    ManufacturerComodityTypes = ManufacturerComodityTypeData.Where(x => x.ManufacturerId == item.Id).ToList();

                    ManufacturerResponseDto ManufacturerWithCommodityType = new ManufacturerResponseDto()
                    {
                        Id = item.Id,
                        ManufacturerName = item.ManufacturerName,
                        ManufacturerCode = item.ManufacturerCode,
                        IsWarrentyGiven = item.IsWarrentyGiven,
                        ComodityTypes = ManufacturerComodityTypes.Select(c => c.CommodityTypeId).ToList(),
                        IsActive = item.IsActive,
                        EntryDateTime = item.EntryDateTime,
                        EntryUser = item.EntryUser
                    };
                    ManufacturerWithCommodityTypes.Add(ManufacturerWithCommodityType);
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return ManufacturerWithCommodityTypes;
        }

        public ManufacturerResponseDto GetManufacturerById(Guid ManufacturerId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                ManufacturerResponseDto pDto = new ManufacturerResponseDto();

                var query =
                    from Manufacturer in session.Query<Manufacturer>()
                    where Manufacturer.Id == ManufacturerId
                    select new { Manufacturer = Manufacturer };

                var result = query.ToList();

                List<ManufacturerComodityTypes> ManufacturerComodityType = null;
                IQueryable<ManufacturerComodityTypes> data = session.Query<ManufacturerComodityTypes>();
                ManufacturerComodityType = data.Where(x => x.ManufacturerId == ManufacturerId).ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().Manufacturer.Id;
                    pDto.ComodityTypes = ManufacturerComodityType.Select(c => c.CommodityTypeId).ToList();
                    pDto.ManufacturerCode = result.First().Manufacturer.ManufacturerCode;
                    pDto.ManufacturerName = result.First().Manufacturer.ManufacturerName;
                    pDto.ManufacturerClassId = result.First().Manufacturer.ManufacturerClassId;
                    pDto.IsWarrentyGiven = result.First().Manufacturer.IsWarrentyGiven;
                    pDto.IsActive = result.First().Manufacturer.IsActive;
                    pDto.EntryDateTime = result.First().Manufacturer.EntryDateTime;
                    pDto.EntryUser = result.First().Manufacturer.EntryUser;

                    pDto.IsManufacturerExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsManufacturerExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        internal bool AddManufacturer(ManufacturerRequestDto Manufacturer)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Manufacturer pr = new Entities.Manufacturer();

                pr.Id = new Guid();
                pr.ManufacturerCode = Manufacturer.ManufacturerCode;
                pr.ManufacturerName = Manufacturer.ManufacturerName;
                pr.ManufacturerClassId = Manufacturer.ManufacturerClassId;
                pr.IsWarrentyGiven = Manufacturer.IsWarrentyGiven;
                pr.IsActive = Manufacturer.IsActive;
                pr.EntryDateTime = DateTime.Today.ToUniversalTime();
                pr.EntryUser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);
                    foreach (var item in Manufacturer.ComodityTypes)
                    {
                        ManufacturerComodityTypes cc = new Entities.ManufacturerComodityTypes();
                        cc.Id = new Guid();
                        cc.CommodityTypeId = item;
                        cc.ManufacturerId = pr.Id;
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

        internal bool UpdateManufacturer(ManufacturerRequestDto Manufacturer)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Manufacturer pr = new Entities.Manufacturer();

                pr.Id = Manufacturer.Id;
                pr.ManufacturerCode = Manufacturer.ManufacturerCode;
                pr.ManufacturerName = Manufacturer.ManufacturerName;
                pr.IsWarrentyGiven = Manufacturer.IsWarrentyGiven;
                pr.IsActive = Manufacturer.IsActive;
                pr.EntryDateTime = Manufacturer.EntryDateTime;
                pr.ManufacturerClassId = Manufacturer.ManufacturerClassId;
                pr.EntryUser = Manufacturer.EntryUser;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(pr);
                    //Add New Countries
                    foreach (var item in Manufacturer.ComodityTypes)
                    {
                        var query = from ManufacturerComodityType in session.Query<ManufacturerComodityTypes>()
                                    where ManufacturerComodityType.CommodityTypeId == item && ManufacturerComodityType.ManufacturerId == Manufacturer.Id
                                    select new { Id = ManufacturerComodityType.CommodityTypeId };
                        if (query.ToList().Count == 0)
                        {
                            ManufacturerComodityTypes cc = new Entities.ManufacturerComodityTypes();
                            cc.Id = new Guid();
                            cc.CommodityTypeId = item;
                            cc.ManufacturerId = Manufacturer.Id;
                            session.SaveOrUpdate(cc);
                        }
                    }
                    //Delete Removed Countries
                    var queryDeleted = from ManufacturerComodityType in session.Query<ManufacturerComodityTypes>()
                                       where ManufacturerComodityType.ManufacturerId == Manufacturer.Id
                                       select ManufacturerComodityType;
                    var list = queryDeleted.ToList();
                    foreach (var item in list)
                    {
                        if (Manufacturer.ComodityTypes.Contains(item.CommodityTypeId) == false)
                        {
                            session.Delete(item);
                        }
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
    }
}
