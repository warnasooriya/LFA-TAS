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
    public class ProductTypeEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<ProductTypeResponseDto> GetProductTypes()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<ProductType>().Select(ProductType => new ProductTypeResponseDto {
                Id = ProductType.Id,
                Type = ProductType.Type,
                EntryDateTime = ProductType.EntryDateTime,
                EntryUser = ProductType.EntryUser,
                Code = ProductType.Code,
            }).ToList();
        }

        public ProductTypeResponseDto GetProductTypeById(Guid ProductTypeId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                ProductTypeResponseDto pDto = new ProductTypeResponseDto();

                var query =
                    from ProductType in session.Query<ProductType>()
                    where ProductType.Id == ProductTypeId
                    select new { ProductType = ProductType };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().ProductType.Id;
                    pDto.Type = result.First().ProductType.Type;
                    pDto.EntryDateTime = result.First().ProductType.EntryDateTime;
                    pDto.EntryUser = result.First().ProductType.EntryUser;

                    pDto.IsProductTypeExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsProductTypeExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }


        internal bool AddProductType(ProductTypeRequestDto ProductType)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                ProductType pr = new Entities.ProductType();

                pr.Id = new Guid();
                pr.Type = ProductType.Type;
                pr.EntryDateTime = DateTime.Today.ToUniversalTime();
                pr.EntryUser = Guid.Parse(" ");



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

        internal bool UpdateProductType(ProductTypeRequestDto ProductType)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                ProductType pr = new Entities.ProductType();

                pr.Id = ProductType.Id;
                pr.Type = ProductType.Type;
                pr.EntryDateTime = ProductType.EntryDateTime;
                pr.EntryUser = ProductType.EntryUser;

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
