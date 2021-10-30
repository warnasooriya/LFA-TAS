using NHibernate;
using System;
using System.Collections.Generic;
using TAS.DataTransfer.Requests;
using TAS.Services.Common.Notification;
using TAS.Services.Entities;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using TAS.Services.Reports.PolicyBooklet;
using TAS.Services.Reports.PolicyStatement;

namespace TAS.Services.Reports
{
    public class ReportsForSend
    {
        private static readonly Lazy<ReportsForSend> lazy = new Lazy<ReportsForSend>(() => new ReportsForSend());
        public void SendPolicyStatementAndBooklate(PolicyBundleRequestDto Policy, String DBName)
        {
            ProductEntityManager pe = new ProductEntityManager();
            List<byte[]> ReportAttachmentList = new List<byte[]>();
            ReportAttachmentList.Add(new PolicyStatementReport().Generate(DBName, pe.GetProductById(Policy.ProductId).Productcode, Policy.Id));
            ReportAttachmentList.Add(new PolicyBookletReport().Generate(DBName,
                pe.GetProductById(Policy.ProductId).Productcode,
                new CommonEntityManager().GetDealerById(Policy.DealerId).DealerCode,
                new CommonEntityManager().GetCommodityCategoryNameByPolicyId(Policy.Id),
                new CommonEntityManager().GetMakeCodeByPolicyId(Policy.Id),
                new CommonEntityManager().GetItemStatusByPolicyId(Policy.Id)));
            CustomerEntityManager ce = new CustomerEntityManager();
            List<String> EmailList = new List<string>();
            EmailList.Add(ce.GetCustomerById(Policy.CustomerId).Email);
            new GetMyEmail().PolicyStatementAndBooklate(ReportAttachmentList, EmailList, ce.GetCustomerById(Policy.CustomerId).FirstName);
        }

        public void SendPolicyStatementAndBooklateTire(PolicyBundleRequestDto policy, string DBName, object res, IList<Policy> policyDetails)
        {
            ProductEntityManager pe = new ProductEntityManager();

            ISession session = EntitySessionManager.GetSession();

                List<byte[]> ReportAttachmentList = new List<byte[]>();

                //object res = policyEntityManager.DownloadPolicyStatementforTYER(policyList.Id, DBName, dbConnectionString);
                //ReportAttachmentList.Add(new PolicyStatementReport().Generate(DBName, pe.GetProductById(policyList.ProductId).Productcode, policyList.Id));
                ReportAttachmentList.Add(new PolicyBookletReport().Generate(DBName,
                    pe.GetProductById(policy.ProductId).Productcode,
                    new CommonEntityManager().GetDealerById(policy.DealerId).DealerCode,
                    new CommonEntityManager().GetCommodityCategoryNameByPolicyId(policy.Id),
                    new CommonEntityManager().GetMakeCodeByPolicyId(policy.Id),
                    new CommonEntityManager().GetItemStatusByPolicyId(policy.Id)));
                CustomerEntityManager ce = new CustomerEntityManager();
                List<String> EmailList = new List<string>();
                EmailList.Add(ce.GetCustomerById(policy.CustomerId).Email);
                new GetMyEmail().PolicyStatementAndBooklateTire(ReportAttachmentList,EmailList,policy.jwt, res, ce.GetCustomerById(policy.CustomerId).FirstName, policyDetails);



        }
    }
}
