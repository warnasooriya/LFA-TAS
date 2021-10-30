using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using TAS.Services.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class PolicyBundleTransactionUpdationUnitOfWork //: UnitOfWork
    {
        //public PolicyBundleHistoryRequestDto Policy;
        //public PolicyBundleHistoryResponseDto ExPr;
       
        //public PolicyBundleHistoryUpdationUnitOfWork(PolicyBundleHistoryRequestDto Policy)
        //{
            
        //    this.Policy = Policy;
        //}
        //public override bool PreExecute()
        //{
        //    try
        //    {
        //        EntitySessionManager.OpenSession();
        //        PolicyEntityManager PolicyEntityManager = new PolicyEntityManager();
        //        var ce = PolicyEntityManager.GetPolicyBundleHistoryById(Policy.Id);
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
        //public override void Execute()
        //{
        //    try
        //    {
        //        EntitySessionManager.OpenSession();
        //        PolicyEntityManager PolicyEntityManager = new PolicyEntityManager();
        //        //var ce = PolicyEntityManager.GetPolicyById(Policy.Id);
        //        //if (ce.IsPolicyExists == true)
        //        //{
        //            bool result = PolicyEntityManager.UpdatePolicyBundleHistory(Policy);
        //            this.Policy.PolicyBundleInsertion = result;
        //        //}
        //        //else
        //        //{
        //        //    this.Policy.PolicyInsertion = false;
        //        //}

        //    }
        //    finally
        //    {
        //        EntitySessionManager.CloseSession();
        //    }
        //}
    }
}
