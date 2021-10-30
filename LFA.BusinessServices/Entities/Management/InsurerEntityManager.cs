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
    public class InsurerEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #region Insurer
        public List<InsurerResponseDto> GetInsurers()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<Insurer>().Select(Insurer => new InsurerResponseDto {
                Id = Insurer.Id,
                BookletCode = Insurer.BookletCode,
                InsurerCode = Insurer.InsurerCode,
                InsurerFullName = Insurer.InsurerFullName,
                InsurerShortName = Insurer.InsurerShortName,
                Comments = Insurer.Comments,
                IsActive = Insurer.IsActive,
                EntryDateTime = Insurer.EntryDateTime,
                EntryUser = Insurer.EntryUser
            }).ToList();
        }

        public InsurerResponseDto GetInsurerById(Guid InsurerId)
        {

            try
            {
                ISession session = EntitySessionManager.GetSession();

                InsurerResponseDto pDto = new InsurerResponseDto();

                var query1 =
                    from Insurer in session.Query<Insurer>()
                    where Insurer.Id == InsurerId
                    select new { Insurer = Insurer };
                var result1 = query1.ToList();

                if (result1 != null && result1.Count > 0)
                {
                    List<InsurerCountries> countries = null;
                    IQueryable<InsurerCountries> data = session.Query<InsurerCountries>();
                    countries = data.Where(x => x.InsurerId == InsurerId).ToList();

                    List<InsurerProducts> products = null;
                    IQueryable<InsurerProducts> productsData = session.Query<InsurerProducts>();
                    products = productsData.Where(x => x.InsurerId == InsurerId).ToList();

                    List<InsurerCommodityTypes> commodityType = null;
                    IQueryable<InsurerCommodityTypes> commodityTypeData = session.Query<InsurerCommodityTypes>();
                    commodityType = commodityTypeData.Where(x => x.InsurerId == InsurerId).ToList();

                    pDto.Id = result1.First().Insurer.Id;
                    pDto.BookletCode = result1.First().Insurer.BookletCode;
                    pDto.InsurerCode = result1.First().Insurer.InsurerCode;
                    pDto.InsurerFullName = result1.First().Insurer.InsurerFullName;
                    pDto.InsurerShortName = result1.First().Insurer.InsurerShortName;
                    pDto.Comments = result1.First().Insurer.Comments;
                    pDto.Countries = countries.Select(c => c.CountryId).ToList();
                    pDto.IsActive = result1.First().Insurer.IsActive;
                    pDto.EntryDateTime = result1.First().Insurer.EntryDateTime;
                    pDto.EntryUser = result1.First().Insurer.EntryUser;
                    pDto.Countries = countries.Select(c => c.CountryId).ToList();
                    pDto.Products = products.Select(c => c.ProductId).ToList();
                    pDto.CommodityTypes = commodityType.Select(c => c.CommodityTypeId).ToList();

                    pDto.IsInsurerExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsInsurerExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        internal bool AddInsurer(InsurerRequestDto Insurer)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Insurer pr = new Entities.Insurer();
                pr.Id = new Guid();
                pr.BookletCode = Insurer.BookletCode;
                pr.InsurerCode = Insurer.InsurerCode;
                pr.InsurerFullName = Insurer.InsurerFullName;
                pr.InsurerShortName = Insurer.InsurerShortName;
                pr.Comments = Insurer.Comments;
                pr.IsActive = Insurer.IsActive;
                pr.EntryDateTime = DateTime.Today.ToUniversalTime();
                pr.EntryUser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);
                    foreach (var item in Insurer.CommodityTypes)
                    {
                        InsurerCommodityTypes cc = new Entities.InsurerCommodityTypes();
                        cc.Id = new Guid();
                        cc.CommodityTypeId = item;
                        cc.InsurerId = pr.Id;
                        session.SaveOrUpdate(cc);
                    }
                    foreach (var item in Insurer.Countries)
                    {
                        InsurerCountries cc = new Entities.InsurerCountries();
                        cc.Id = new Guid();
                        cc.CountryId = item;
                        cc.InsurerId = pr.Id;
                        session.SaveOrUpdate(cc);
                    }
                    foreach (var item in Insurer.Products)
                    {
                        InsurerProducts cc = new Entities.InsurerProducts();
                        cc.Id = new Guid();
                        cc.ProductId = item;
                        cc.InsurerId = pr.Id;
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

        internal bool UpdateInsurer(InsurerRequestDto Insurer)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Insurer pr = new Entities.Insurer();
                pr.Id = Insurer.Id;
                pr.BookletCode = Insurer.BookletCode;
                pr.InsurerCode = Insurer.InsurerCode;
                pr.InsurerFullName = Insurer.InsurerFullName;
                pr.InsurerShortName = Insurer.InsurerShortName;
                pr.Comments = Insurer.Comments;
                pr.IsActive = Insurer.IsActive;
                pr.EntryDateTime = Insurer.EntryDateTime;
                pr.EntryUser =Insurer.EntryUser;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(pr);

                    var existingCountries = session.Query<InsurerCountries>().Where(a => a.InsurerId == Insurer.Id).ToList();
                    foreach (var country in existingCountries)
                    {
                        session.Delete(country);
                    }
                    foreach (var item in Insurer.Countries)
                    {
                        InsurerCountries cc = new Entities.InsurerCountries();
                        cc.Id = new Guid();
                        cc.CountryId = item;
                        cc.InsurerId = pr.Id;
                        session.SaveOrUpdate(cc);
                    }

                    var existingCommodityTypes = session.Query<InsurerCommodityTypes>().Where(a => a.InsurerId == Insurer.Id).ToList();
                    foreach (var country in existingCommodityTypes)
                    {
                        session.Delete(country);
                    }
                    foreach (var item in Insurer.CommodityTypes)
                    {
                        InsurerCommodityTypes cc = new Entities.InsurerCommodityTypes();
                        cc.Id = new Guid();
                        cc.CommodityTypeId = item;
                        cc.InsurerId = pr.Id;
                        session.SaveOrUpdate(cc);
                    }

                    var existingProducts = session.Query<InsurerProducts>().Where(a => a.InsurerId == Insurer.Id).ToList();
                    foreach (var country in existingProducts)
                    {
                        session.Delete(country);
                    }
                    foreach (var item in Insurer.Products)
                    {
                        InsurerProducts cc = new Entities.InsurerProducts();
                        cc.Id = new Guid();
                        cc.ProductId = item;
                        cc.InsurerId = pr.Id;
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
        #endregion

        #region Insurer Consortium
        public List<InsurerConsortium> GetInsurerConsortiums()
        {
            List<InsurerConsortium> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<InsurerConsortium> InsurerConsortiumData = session.Query<InsurerConsortium>();
            entities = InsurerConsortiumData.ToList();
            return entities;
        }

        public InsurerConsortiumResponseDto GetInsurerConsortiumById(Guid InsurerConsortiumId)
        {

            try
            {
                ISession session = EntitySessionManager.GetSession();

                InsurerConsortiumResponseDto pDto = new InsurerConsortiumResponseDto();

                var query =
                    from InsurerConsortium in session.Query<InsurerConsortium>()
                    where InsurerConsortium.Id == InsurerConsortiumId
                    select new { InsurerConsortium = InsurerConsortium };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().InsurerConsortium.Id;
                    pDto.ParentInsurerId = result.First().InsurerConsortium.ParentInsurerId;
                    pDto.InsurerId = result.First().InsurerConsortium.InsurerId;
                    pDto.NRPPercentage = result.First().InsurerConsortium.NRPPercentage;
                    pDto.ProfitSharePercentage = result.First().InsurerConsortium.ProfitSharePercentage;
                    pDto.RiskSharePercentage = result.First().InsurerConsortium.RiskSharePercentage;

                    pDto.IsInsurerConsortiumExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsInsurerConsortiumExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        internal bool AddInsurerConsortium(InsurerConsortiumRequestDto InsurerConsortium)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();

                InsurerConsortium pr = new Entities.InsurerConsortium();

                pr.Id = new Guid();
                pr.ParentInsurerId = InsurerConsortium.ParentInsurerId;
                pr.InsurerId = InsurerConsortium.InsurerId;
                pr.NRPPercentage = InsurerConsortium.NRPPercentage;
                pr.ProfitSharePercentage = InsurerConsortium.ProfitSharePercentage;
                pr.RiskSharePercentage = InsurerConsortium.RiskSharePercentage;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);
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

        internal bool UpdateInsurerConsortium(InsurerConsortiumRequestDto InsurerConsortium)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                InsurerConsortium pr = new Entities.InsurerConsortium();

                pr.Id = InsurerConsortium.Id;
                pr.ParentInsurerId = InsurerConsortium.ParentInsurerId;
                pr.InsurerId = InsurerConsortium.InsurerId;
                pr.NRPPercentage = InsurerConsortium.NRPPercentage;
                pr.ProfitSharePercentage = InsurerConsortium.ProfitSharePercentage;
                pr.RiskSharePercentage = InsurerConsortium.RiskSharePercentage;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    if (pr.RiskSharePercentage == 0)
                    {
                        var InsurerConsortiumDeleted = from InsurerConsortiumd in session.Query<InsurerConsortium>()
                                                         where InsurerConsortiumd.Id == pr.Id
                                                         select InsurerConsortiumd;
                        var InsurerConsortiumList = InsurerConsortiumDeleted.ToList();
                        foreach (var item in InsurerConsortiumList)
                        {
                            session.Delete(item);
                        }
                    }
                    else
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
        #endregion
    }
}
