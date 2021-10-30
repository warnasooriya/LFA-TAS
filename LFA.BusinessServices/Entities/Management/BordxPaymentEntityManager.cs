using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Common;
using TAS.Services.Common.Enums;
using TAS.Services.Common.Transformer;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.Entities.Management
{
    public class BordxPaymentEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<ClaimBordxPayment> GetClaimBordxPayments()
        {
            List<ClaimBordxPayment> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<ClaimBordxPayment> ClaimBordxPaymentData = session.Query<ClaimBordxPayment>();
            entities = ClaimBordxPaymentData.ToList();
            return entities;
        }

        public BordxPaymentsResponseDto GetClaimBordxPaymentsById(Guid ClaimBordxId)
        {
            ISession session = EntitySessionManager.GetSession();
            BordxPaymentsResponseDto res = new BordxPaymentsResponseDto();
            try
            {
                var query =
                from ClaimBordxPayment in session.Query<ClaimBordxPayment>()
                where ClaimBordxPayment.ClaimBordxID == ClaimBordxId
                select new { ClaimBordxPayment = ClaimBordxPayment };

                var result = query.ToList();

                
                res.BordxPayments = new List<BordxPaymentResponseDto>();
                foreach (var Payment in result)
                {
                    BordxPaymentResponseDto pr = new BordxPaymentResponseDto();

                    pr.Id = Payment.ClaimBordxPayment.Id;
                    pr.BordereauID = Payment.ClaimBordxPayment.ClaimBordxID;
                    // pr.BordxNumber = GetBordxPaymentById(Payment.ClaimBordxPayment.BordereauID).BordxNumber;
                    pr.ReceiptDate = Payment.ClaimBordxPayment.ReceiptDate;
                    pr.BordxAmount = Payment.ClaimBordxPayment.BordxAmount;
                    pr.RefNo = Payment.ClaimBordxPayment.RefNo;

                    res.BordxPayments.Add(pr);
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            
            return res;

        }

        public BordxPaymentResponseDto GetBordxPaymentById(Guid Id)
        {
            ISession session = EntitySessionManager.GetSession();
            BordxPaymentResponseDto pDto = new BordxPaymentResponseDto();

            var query =
                from ClaimBordxPayment in session.Query<ClaimBordxPayment>()
                where ClaimBordxPayment.Id == Id
                select new { ClaimBordxPayment = ClaimBordxPayment };

            var result = query.ToList();

            if (result != null && result.Count > 0)
            {

                pDto.Id = result.First().ClaimBordxPayment.Id;
                pDto.BordereauID = result.First().ClaimBordxPayment.ClaimBordxID;
                pDto.ReceiptDate = result.First().ClaimBordxPayment.ReceiptDate;
                pDto.BordxAmount = result.First().ClaimBordxPayment.BordxAmount;
                pDto.RefNo = result.First().ClaimBordxPayment.RefNo;

                return pDto;
            }
            else
            {
                
                return null;
            }
            
           
        }

        internal bool AddBordxPayment(BordxPaymentRequestDto BordxPayment)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                ClaimBordxPayment pr = new Entities.ClaimBordxPayment();
                ClaimBordx cb_ = session.Query<ClaimBordx>().FirstOrDefault(a => a.Id == BordxPayment.ClaimBordxID);

                pr.Id = new Guid();
                pr.ClaimBordxID = BordxPayment.ClaimBordxID;
                pr.ReceiptDate = DateTime.Now;
                pr.BordxAmount = BordxPayment.BordxAmount;
                pr.BalanceAmount = BordxPayment.BalanceAmount;
                pr.PaidAmount = BordxPayment.PaidAmount;
                pr.RefNo = BordxPayment.RefNo;   

                //update Claim bordx Ispaid
                if (BordxPayment.BordxAmount <= BordxPayment.PaidAmount)
                cb_.IsPaid = true;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);
                    session.SaveOrUpdate(cb_);
                    BordxPayment.Id = pr.Id;
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

        internal bool UpdateBordxPayment(BordxPaymentRequestDto BordxPayment)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                ClaimBordxPayment pr = new Entities.ClaimBordxPayment();

                pr.Id = BordxPayment.Id;
                pr.ClaimBordxID = BordxPayment.ClaimBordxID;
                pr.ReceiptDate = BordxPayment.ReceiptDate;
                pr.BordxAmount = BordxPayment.BordxAmount;
                pr.RefNo = BordxPayment.RefNo;
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
