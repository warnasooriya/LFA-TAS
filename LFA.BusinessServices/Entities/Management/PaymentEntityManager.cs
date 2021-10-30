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
    public class PaymentEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<PaymetModeResponseDto> GetPaymentModes()
        {
            List<PaymentMode> entities = null;
            ISession session = EntitySessionManager.GetSession();
            return session.Query<PaymentMode>().Select(s => new PaymetModeResponseDto
            {
                Code = s.Code,
                Id = s.Id,
                PaymentMode = s.PaymentModeName
            }).ToList();
        }

        public PaymentTypesResponseDto GetAllPaymentTypesByPaymentModeId(Guid PaymentModeId)
        {
            List<PaymentOptions> entities = null;
            PaymentTypesResponseDto PaymentTypesResponseDto = new PaymentTypesResponseDto();

            try
            {
                PaymentTypesResponseDto.PaymetTypes = new List<PaymetTypeResponseDto>();
                ISession session = EntitySessionManager.GetSession();
                IQueryable<PaymentOptions> PaymentOptions = session.Query<PaymentOptions>().Where(a => a.PaymentModeId == PaymentModeId);
                entities = PaymentOptions.ToList();
                foreach (PaymentOptions item in entities)
                {
                    var paymentType = new PaymetTypeResponseDto()
                    {
                        Id = item.Id,
                        PaymentCharge = item.PaymentCharge,
                        PaymentType = item.PaymentOption
                    };
                    PaymentTypesResponseDto.PaymetTypes.Add(paymentType);

                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }


            return PaymentTypesResponseDto;
        }
    }
}
