using TAS.Services.Entities.Persistence;
using NHibernate;
using TAS.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;

using NHibernate.Criterion;
using TAS.Services.Common;
using System.Security.Cryptography;
using NLog;
using System.Reflection;

namespace TAS.Services.Entities.Management
{
    public class MakeEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public List<MakeResponseDto> GetMakes()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<Make>().Select(Make => new MakeResponseDto {
                Id = Make.Id,
                CommodityTypeId = Make.CommodityTypeId,
                MakeCode = Make.MakeCode,
                MakeName = Make.MakeName,
                ManufacturerId = Make.ManufacturerId,
                WarantyGiven = Make.WarantyGiven,
                IsActive = Make.IsActive,
                IsMakeExists =false,
                EntryDateTime = Make.EntryDateTime,
                EntryUser = Make.EntryUser,
                //need to write other fields
            }).ToList();
        }


        public MakeResponseDto GetMakeById(Guid MakeId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                MakeResponseDto pDto = new MakeResponseDto();

                var query =
                    from Make in session.Query<Make>()
                    where Make.Id == MakeId
                    select new { Make = Make };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().Make.Id;
                    pDto.CommodityTypeId = result.First().Make.CommodityTypeId;
                    pDto.MakeCode = result.First().Make.MakeCode;
                    pDto.MakeName = result.First().Make.MakeName;
                    pDto.ManufacturerId = result.First().Make.ManufacturerId;
                    pDto.WarantyGiven = result.First().Make.WarantyGiven;
                    pDto.IsActive = result.First().Make.IsActive;
                    pDto.EntryDateTime = result.First().Make.EntryDateTime;
                    pDto.EntryUser = result.First().Make.EntryUser;

                    pDto.IsMakeExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsMakeExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }


        internal MakeRequestDto AddMake(MakeRequestDto Make)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                Make.IsMakeExists = false;
                Make.MakeInsertion = false;
                if (session.Query<Make>().Where(a => a.MakeName == Make.MakeName).Count() == 0)
                {
                    Make pr = new Entities.Make();
                    pr.Id = new Guid();
                    pr.CommodityTypeId = Make.CommodityTypeId;
                    pr.MakeCode = Make.MakeCode;
                    pr.MakeName = Make.MakeName;
                    pr.ManufacturerId = Make.ManufacturerId;
                    pr.IsActive = Make.IsActive;
                    pr.WarantyGiven = Make.WarantyGiven;
                    pr.EntryDateTime = DateTime.Today.ToUniversalTime();
                    pr.EntryUser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(pr);
                        transaction.Commit();
                        Make.message = "Make Inserted Successfully !";
                        Make.MakeInsertion = true;
                    }
                }
                else {
                    Make.IsMakeExists = true;
                    Make.MakeInsertion = false;
                    Make.message = "Make Already Exist !";
                }

                return Make;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return Make;
            }
        }

        internal bool UpdateMake(MakeRequestDto Make)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Make pr = new Entities.Make();

                pr.Id = Make.Id;
                pr.CommodityTypeId = Make.CommodityTypeId;
                pr.MakeCode = Make.MakeCode;
                pr.MakeName = Make.MakeName;
                pr.ManufacturerId = Make.ManufacturerId;
                pr.WarantyGiven = Make.WarantyGiven;
                pr.IsActive = Make.IsActive;
                pr.EntryDateTime = Make.EntryDateTime;
                pr.EntryUser = Make.EntryUser;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(pr);

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

        internal static MakesResponseDto GetMakesByCommodityCategoryId(Guid CommodityCategoryId)
        {
            MakesResponseDto _Response = new MakesResponseDto();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                IQueryable<Model> relavantModelList = session.Query<Model>()
                    .Where(a => a.CategoryId == CommodityCategoryId);
                IQueryable<Make> relavantMakesList = session.Query<Make>()
                    .Where(a => relavantModelList.Any(b => b.MakeId == a.Id)).Distinct();

                _Response.Makes = new List<MakeResponseDto>();
                foreach (Make make in relavantMakesList)
                {
                    MakeResponseDto currentMake = new MakeResponseDto()
                    {
                        CommodityTypeId = make.CommodityTypeId,
                        EntryDateTime = make.EntryDateTime,
                        EntryUser = make.EntryUser,
                        Id = make.Id,
                        IsActive = make.IsActive,
                        IsMakeExists = make.IsMakeExists,
                        MakeCode = make.MakeCode,
                        MakeName = make.MakeName,
                        ManufacturerId = make.ManufacturerId,
                        WarantyGiven = make.WarantyGiven
                    };
                    _Response.Makes.Add(currentMake);
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                _Response =  null;
            }
            return _Response;
        }
    }
}
