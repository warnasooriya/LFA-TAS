using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using TAS.Services.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Common;
using TAS.Services.Reports.PolicyStatement;
using TAS.Services.Common.Notification;
using TAS.Services.Reports.PolicyBooklet;
using TAS.Services.Reports;
using NHibernate;
using NHibernate.Linq;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class PolicyBundleUpdationUnitOfWork : UnitOfWork
    {
        public PolicyBundleRequestDto Policy;
        public PolicyBundleResponseDto ExPr;

        public PolicyBundleUpdationUnitOfWork(PolicyBundleRequestDto Policy)
        {

            this.Policy = Policy;
        }
        public override bool PreExecute()
        {
            try
            {
                Common.JWTHelper JWTHelper = new Common.JWTHelper(SecurityContext);
                Dictionary<string, object> str = JWTHelper.DecodeAuthenticationToken();
                string dbName = str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
                if (dbName != "")
                {
                    TASEntitySessionManager.OpenSession();
                    string tasTpaConnString = TASTPAEntityManager.GetTPAConnStringBydbName(dbName);
                    TASEntitySessionManager.CloseSession();
                    if (tasTpaConnString != "")
                    {
                        dbConnectionString = tasTpaConnString;
                        EntitySessionManager.OpenSession(dbConnectionString);
                        if (JWTHelper.checkTokenValidity(Convert.ToInt32(ConfigurationData.tasTokenValidTime.ToString())))
                        {
                            PolicyEntityManager PolicyEntityManager = new PolicyEntityManager();
                            var ce = PolicyEntityManager.GetPolicyBundleById(Policy.Id);
                            //getting error when new policy adding
                            if (ce==null || ce.IsPolicyBundleExists == true)
                            {
                               // Policy.EntryDateTime = ce.EntryDateTime;
                               // Policy.EntryUser = ce.EntryUser;
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        EntitySessionManager.CloseSession();

                    }
                }
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
            return false;
        }

        //public override bool PreExecute()
        //{
        //    try
        //    {
        //        EntitySessionManager.OpenSession();
        //        PolicyEntityManager PolicyEntityManager = new PolicyEntityManager();
        //        var ce = PolicyEntityManager.GetPolicyBundleById(Policy.Id);
        //        if (ce.IsPolicyBundleExists == true)
        //        {
        //            Policy.EntryDateTime = ce.EntryDateTime;
        //            Policy.EntryUser = ce.EntryUser;
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    finally
        //    {
        //        EntitySessionManager.CloseSession();

        //    }

        //}

        public override void Execute()
        {
            try
            {
                //temp section can remove  //**  lines once preexecute jwt and db name working fine
                if (dbConnectionString != null)   //**
                {     //**
                    EntitySessionManager.OpenSession(dbConnectionString);
                }     //**
                else     //**
                {     //**
                    EntitySessionManager.OpenSession();     //**
                }     //**
                PolicyEntityManager PolicyEntityManager = new PolicyEntityManager();
                //var ce = PolicyEntityManager.GetPolicyById(Policy.Id);
                //if (ce.IsPolicyExists == true)
                //{
                    bool result = PolicyEntityManager.UpdatePolicyBundle(Policy);
                    this.Policy.PolicyBundleInsertion = result;
                    if (result && Policy.IsApproved)
                    {
                        try
                        {
                            Common.JWTHelper JWTHelper = new Common.JWTHelper(SecurityContext);
                            Dictionary<string, object> str = JWTHelper.DecodeAuthenticationToken();
                            string dbName = str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
                        ISession session = EntitySessionManager.GetSession();

                        Guid productId = session.Query<Policy>().Where(a => a.PolicyBundleId == Policy.Id).FirstOrDefault().ProductId;
                        string productCode = session.Query<Product>().Where(a => a.Id == productId).FirstOrDefault().Productcode;
                        if (productCode.ToLower().Trim() == "tyre")
                        {
                            IList<Policy> policies = session.QueryOver<Policy>().Where(a => a.PolicyBundleId == Policy.Id).List();
                            object res = null;
                            //List<object> policyNumberList = new List<object>();
                            foreach (var policyList in policies)
                            {
                                 res = PolicyEntityManager.DownloadPolicyStatementforTYER(policyList.PolicyBundleId, dbName, dbConnectionString);

                                //policyNumberList.Add(policyList.PolicyNo);
                            }
                           // string policyNumberCommaSeparated = string.Join(",", policyNumberList);

                            new ReportsForSend().SendPolicyStatementAndBooklateTire(Policy, dbName, res, policies);

                        }
                        else
                        {
                            new ReportsForSend().SendPolicyStatementAndBooklate(Policy, dbName);
                        }

                        }
                        catch (Exception ex)
                        {
                        }
                    }
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
