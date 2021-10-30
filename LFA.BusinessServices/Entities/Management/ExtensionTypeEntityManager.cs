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
    public class ExtensionTypeEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<ExtensionTypeResponseDto> GetExtensionTypes()
        {

            ISession session = EntitySessionManager.GetSession();
            return session.Query<ExtensionType>().Select(ExtensionType => new ExtensionTypeResponseDto {
                Id = ExtensionType.Id,
                Km = ExtensionType.Km,
                Month = ExtensionType.Month,
                Hours = ExtensionType.Hours,
                ExtensionName = ExtensionType.ExtensionName,
                ProductId = ExtensionType.ProductId,
                CommodityTypeId = ExtensionType.CommodityTypeId,
                EntryDateTime = ExtensionType.EntryDateTime,
                EntryUser = ExtensionType.EntryUser,
                //need to write other fields
            }).ToList();
        }

        public ExtensionTypeResponseDto GetExtensionTypeById(Guid ExtensionTypeId)
        {

            try
            {
                ISession session = EntitySessionManager.GetSession();
                ExtensionTypeResponseDto pDto = new ExtensionTypeResponseDto();

                var query =
                    from ExtensionType in session.Query<ExtensionType>()
                    where ExtensionType.Id == ExtensionTypeId
                    select new { ExtensionType = ExtensionType };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().ExtensionType.Id;
                    pDto.CommodityTypeId = result.First().ExtensionType.CommodityTypeId;
                    pDto.ProductId = result.First().ExtensionType.ProductId;
                    pDto.ExtensionName = result.First().ExtensionType.ExtensionName;
                    pDto.Hours = result.First().ExtensionType.Hours;
                    pDto.Km = result.First().ExtensionType.Km;
                    pDto.Month = result.First().ExtensionType.Month;
                    pDto.EntryDateTime = result.First().ExtensionType.EntryDateTime;
                    pDto.EntryUser = result.First().ExtensionType.EntryUser;

                    pDto.IsExtensionTypeExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsExtensionTypeExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }


        internal bool AddExtensionType(ExtensionTypeRequestDto ExtensionType)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                ExtensionType pr = new Entities.ExtensionType();

                pr.Id = new Guid();
                pr.ExtensionName = ExtensionType.ExtensionName;
                pr.Km = ExtensionType.Km;
                pr.Month = ExtensionType.Month;
                pr.Hours = ExtensionType.Hours;
                pr.CommodityTypeId = ExtensionType.CommodityTypeId;
                pr.ProductId = ExtensionType.ProductId;
                pr.EntryDateTime = DateTime.Today.ToUniversalTime();
                pr.EntryUser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);
                    transaction.Commit();
                    ExtensionType.Id = pr.Id;
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }

        internal bool UpdateExtensionType(ExtensionTypeRequestDto ExtensionType)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                ExtensionType pr = new Entities.ExtensionType();

                pr.Id = ExtensionType.Id;
                pr.ExtensionName = ExtensionType.ExtensionName;
                pr.Km = ExtensionType.Km;
                pr.Month = ExtensionType.Month;
                pr.Hours = ExtensionType.Hours;
                pr.CommodityTypeId = ExtensionType.CommodityTypeId;
                pr.ProductId = ExtensionType.ProductId;
                pr.EntryDateTime = ExtensionType.EntryDateTime;
                pr.EntryUser = ExtensionType.EntryUser;

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
    }
}
