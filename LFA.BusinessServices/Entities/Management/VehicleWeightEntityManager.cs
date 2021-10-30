using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using NLog;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Common;
using TAS.Services.Common.Enums;
using TAS.Services.Common.Transformer;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.Entities.Management
{
    public class VehicleWeightEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public List<VehicleWeightResponseDto> GetVehicleWeights()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<VehicleWeight>().ToList().Select(VehicleWeight => new VehicleWeightResponseDto
            {
                Id = VehicleWeight.Id,
                VehicleWeightDescription = VehicleWeight.VehicleWeightDescription,
                WeightFrom = VehicleWeight.WeightFrom,
                WeightTo = VehicleWeight.WeightTo,
                EntryUser = VehicleWeight.EntryUser
            }).ToList();
        }
        private static bool IsGuid(string candidate)
        {
            if (candidate == "00000000-0000-0000-0000-000000000000")
                return false;
            Regex isGuid = new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled);

            if (candidate != null)
            {
                if (isGuid.IsMatch(candidate))
                {
                    return true;
                }
            }

            return false;
        }
        internal static object SubmitVehicleWeight(VehicleWeightRequestDto vehicleWeightData)
        {
            object response = null;

            try
            {
                //validate Vehicle Weight
                if(vehicleWeightData == null)
                    return "Request data invalid.";
                if (vehicleWeightData.WeightFrom > vehicleWeightData.WeightTo)
                {
                    if (vehicleWeightData.WeightTo == 0)
                    {

                    }
                    else
                    {
                        return vehicleWeightData.WeightFrom + " cannot be grater than " + vehicleWeightData.WeightTo;
                    }

                }


                bool isUpdate = IsGuid(vehicleWeightData.Id.ToString());

                ISession session = EntitySessionManager.GetSession();
                VehicleWeight pr = new Entities.VehicleWeight();


                Expression<Func<VehicleWeight, bool>> VehicleWeightfilter = PredicateBuilder.True<VehicleWeight>();
                //VehicleWeightfilter = VehicleWeightfilter.And(a => a.Id == vehicleWeightData.Id);

                List<VehicleWeight> vehicleWeight = session.Query<VehicleWeight>().Where(VehicleWeightfilter).ToList();





                if (vehicleWeight.Count > 0)
                {
                    if (!isUpdate)
                    {
                        //date range validations
                        if (vehicleWeight.Select(vehicleWeightrange => vehicleWeightData.WeightFrom <= vehicleWeightrange.WeightTo &&
                                                                     vehicleWeightrange.WeightFrom <= vehicleWeightData.WeightTo)
                                                                     .Any(overlap => overlap))
                        {
                            return "The Values overlapped with existing record.";
                        }
                    }
                    else
                    {
                        if (vehicleWeight.Where(a => a.Id != vehicleWeightData.Id)
                            .Select(vehicleWeightrange => vehicleWeightData.WeightFrom <= vehicleWeightrange.WeightTo &&
                                    vehicleWeightrange.WeightFrom <= vehicleWeightData.WeightTo).Any(overlap => overlap))
                        {
                            return "The Values overlapped with existing record.";
                        }
                    }
                }


                if (!isUpdate)
                {

                    pr.VehicleWeightDescription = vehicleWeightData.VehicleWeightDescription;
                    pr.VehicleWeightCode = vehicleWeightData.VehicleWeightCode;
                    pr.WeightFrom = vehicleWeightData.WeightFrom;
                    pr.WeightTo = vehicleWeightData.WeightTo;
                    pr.EntryDateTime = DateTime.Today.ToUniversalTime();
                    pr.EntryUser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        pr.Id = Guid.NewGuid();
                        session.Evict(pr);
                        session.Save(pr);
                        transaction.Commit();
                    }
                    response = "ok";
                }
                else
                {
                    pr.Id = vehicleWeightData.Id;
                    pr.VehicleWeightDescription = vehicleWeightData.VehicleWeightDescription;
                    pr.VehicleWeightCode = vehicleWeightData.VehicleWeightCode;
                    pr.WeightFrom = vehicleWeightData.WeightFrom;
                    pr.WeightTo = vehicleWeightData.WeightTo;
                    pr.EntryDateTime = DateTime.Today.ToUniversalTime();
                    pr.EntryUser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Clear();
                        session.SaveOrUpdate(pr);
                        transaction.Commit();
                    }
                    response = "ok";
                }



            }
            catch (Exception ex)
            {
                response = null;
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return response;
        }

        internal VehicleWeightResponseDto GetVehicleWeightById(Guid vehicleWeightId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                VehicleWeightResponseDto pDto = new VehicleWeightResponseDto();

                var query =
                    from vehicleWeight in session.Query<VehicleWeight>()
                    where vehicleWeight.Id == vehicleWeightId
                    select new { vehicleWeight = vehicleWeight };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {

                    pDto.Id = result.First().vehicleWeight.Id;
                    pDto.VehicleWeightDescription = result.First().vehicleWeight.VehicleWeightDescription;
                    pDto.VehicleWeightCode = result.First().vehicleWeight.VehicleWeightCode;
                    pDto.WeightFrom = result.First().vehicleWeight.WeightFrom;
                    pDto.WeightTo = result.First().vehicleWeight.WeightTo;
                    pDto.EntryUser = result.First().vehicleWeight.EntryUser;

                    pDto.IsVehicleWeightExists = true;

                    return pDto;
                }
                else
                {
                    pDto.IsVehicleWeightExists = false;

                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }
    }
}
