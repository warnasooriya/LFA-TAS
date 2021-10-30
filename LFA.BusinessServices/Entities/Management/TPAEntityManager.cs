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
    public class TPAEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        internal static List<TPAResponseDto> GetAllTPAs()
        {
            List<TPA> entities = null;
            ISession session = EntitySessionManager.GetSession();
            return session.Query<TPA>().Select(tpa => new TPAResponseDto {
                Id = tpa.Id,
                tpaCode = tpa.TpaCode,
                Address = tpa.Address,
                Banner = tpa.Banner,
                Banner2 = tpa.Banner2,
                Banner3 = tpa.Banner3,
                Banner4 = tpa.Banner4,
                Banner5 = tpa.Banner5,
                DiscountDescription = tpa.DiscountDescription,
                Logo = tpa.Logo,
                Name = tpa.Name,
                TelNumber = tpa.TelNumber,
                OriginalTPAName = tpa.OriginalTPAName
            }).ToList();
        }

        internal static List<TPAResponseDto> GetTPADetailById(Guid tpaId)
        {
            ISession session = EntitySessionManager.GetSession();
            return  session.Query<TPA>().Where(a=>a.Id==tpaId)
                .Select(tpa => new TPAResponseDto
            {
                Id = tpa.Id,
                tpaCode = tpa.TpaCode,
                Address = tpa.Address,
                Banner = tpa.Banner,
                Banner2 = tpa.Banner2,
                Banner3 = tpa.Banner3,
                Banner4 = tpa.Banner4,
                Banner5 = tpa.Banner5,
                DiscountDescription = tpa.DiscountDescription,
                Logo = tpa.Logo,
                Name = tpa.Name,
                TelNumber = tpa.TelNumber,
                OriginalTPAName = tpa.OriginalTPAName
            }).ToList();
        }

        internal static bool SaveTPA(TPARequestDto TPA)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                TPA tpa = new Entities.TPA();
                tpa.Address = TPA.Address;
                tpa.Banner = TPA.Banner;
                tpa.Banner2 = TPA.Banner2;
                tpa.Banner3 = TPA.Banner3;
                tpa.Banner4 = TPA.Banner4;
                tpa.Banner5 = TPA.Banner5;
                tpa.DiscountDescription = TPA.DiscountDescription;
                tpa.Id = Guid.NewGuid();
                tpa.Logo = TPA.Logo;
                tpa.Name = TPA.Name;
                tpa.TelNumber = TPA.TelNumber;
                tpa.TpaCode = TPA.TpaCode;


                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(tpa);
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

        internal static bool UpdateTPA(TPARequestDto TPA)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                TPA tpa = session.Query<TPA>().Where(a => a.Id == TPA.Id).FirstOrDefault();
                // TPA tpa = new Entities.TPA();
                // tpa.Id = TPA.Id;
                tpa.Address = TPA.Address;
                tpa.Banner = TPA.Banner;
                tpa.Banner2 = TPA.Banner2;
                tpa.Banner3 = TPA.Banner3;
                tpa.Banner4 = TPA.Banner4;
                tpa.Banner5 = TPA.Banner5;
                tpa.DiscountDescription = TPA.DiscountDescription;
                tpa.Logo = TPA.Logo;
                tpa.Name = TPA.Name;
                tpa.TelNumber = TPA.TelNumber;
                tpa.TpaCode = TPA.TpaCode;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(tpa);
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

        internal static List<TPAConfig> GetTPAipConfigDataById(Guid tpaId)
        {

            List<TPAConfig> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<TPAConfig> TPAData = session.Query<TPAConfig>();
            entities = TPAData.ToList();
            return entities;
        }

        internal static List<TPAAllowedIP> GetAllTPAallowedIPstpaId(Guid tpaId)
        {
            List<TPAAllowedIP> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<TPAAllowedIP> TPAData = session.Query<TPAAllowedIP>();
            entities = TPAData.ToList();
            return entities;
        }
    }
}
