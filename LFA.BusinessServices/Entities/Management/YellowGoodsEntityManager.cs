using NHibernate;
using NLog;
using System;
using System.Reflection;
using TAS.DataTransfer.Requests;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.Entities.Management
{
    public class YellowGoodsEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        internal bool AddYellowGoodDetails(YellowGoodRequestDto YellowGoodDetails)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                YellowGoodDetails pr = new Entities.YellowGoodDetails();

                pr.Id = new Guid();
                //  pr.Id = YellowGoodDetails.Id;
                pr.AddnSerialNo = YellowGoodDetails.AddnSerialNo;
                pr.CategoryId = YellowGoodDetails.CategoryId;
                pr.DealerPrice = YellowGoodDetails.DealerPrice;
                pr.InvoiceNo = YellowGoodDetails.InvoiceNo;
                pr.MakeId = YellowGoodDetails.MakeId;
                pr.ModelCode = YellowGoodDetails.ModelCode;
                pr.ModelId = YellowGoodDetails.ModelId;
                pr.ModelYear = YellowGoodDetails.ModelYear;
                pr.ItemPrice = YellowGoodDetails.ItemPrice;
                pr.ItemPurchasedDate = YellowGoodDetails.ItemPurchasedDate;
                pr.ItemStatusId = YellowGoodDetails.ItemStatusId;
                pr.CommodityUsageTypeId = YellowGoodDetails.CommodityUsageTypeId;
                pr.SerialNo = YellowGoodDetails.SerialNo;
                pr.EntryDateTime = YellowGoodDetails.EntryDateTime;
                pr.EntryUser = YellowGoodDetails.EntryUser;
                pr.EntryDateTime = DateTime.Today.ToUniversalTime();
                pr.EntryUser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);
                    YellowGoodDetails.Id = pr.Id;
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

        internal bool UpdateYellowGoodDetails(YellowGoodRequestDto YellowGoodDetails)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                YellowGoodDetails pr = new Entities.YellowGoodDetails();
                session.Load(pr, YellowGoodDetails.Id);
                pr.Id = YellowGoodDetails.Id;
                pr.AddnSerialNo = YellowGoodDetails.AddnSerialNo;
                pr.CategoryId = YellowGoodDetails.CategoryId;
                pr.DealerPrice = YellowGoodDetails.DealerPrice;
                pr.InvoiceNo = YellowGoodDetails.InvoiceNo;
                pr.MakeId = YellowGoodDetails.MakeId;
                pr.ModelCode = YellowGoodDetails.ModelCode;
                pr.ModelId = YellowGoodDetails.ModelId;
                pr.CommodityUsageTypeId = YellowGoodDetails.CommodityUsageTypeId;
                pr.ModelYear = YellowGoodDetails.ModelYear;
                pr.ItemPrice = YellowGoodDetails.ItemPrice;
                pr.ItemPurchasedDate = YellowGoodDetails.ItemPurchasedDate;
                pr.ItemStatusId = YellowGoodDetails.ItemStatusId;
                pr.SerialNo = YellowGoodDetails.SerialNo;
                pr.EntryDateTime = YellowGoodDetails.EntryDateTime;
                pr.EntryUser = YellowGoodDetails.EntryUser;

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
