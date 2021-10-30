using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using TAS.Web.Common;
using TAS.Services;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities;
using TAS.DataTransfer.Requests;

using Newtonsoft.Json;
using NLog;
using System.Reflection;

namespace TAS.Web.Controllers
{
    public class ManufacturerWarrantyManagementController : ApiController
    {
        //private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [HttpPost]
        public string AddManufacturerWarranty(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                ManufacturerWarrantyRequestDto ManufacturerWarranty = data.ToObject<ManufacturerWarrantyRequestDto>();
                IManufacturerWarrantyManagementService ManufacturerWarrantyManagementService = ServiceFactory.GetManufacturerWarrantyManagementService();
                ManufacturerWarrantyRequestDto result = ManufacturerWarrantyManagementService.AddManufacturerWarranty(ManufacturerWarranty, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Manufacturer Warranty Details Added");
                if (result.ManufacturerWarrantyInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add  Manufacturer Warranty Details failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Manufacturer Warranty Details failed!";
            }
            
        }

        [HttpPost]
        public string UpdateManufacturerWarranty(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                ManufacturerWarrantyRequestDto ManufacturerWarranty = data.ToObject<ManufacturerWarrantyRequestDto>();
                IManufacturerWarrantyManagementService ManufacturerWarrantyManagementService = ServiceFactory.GetManufacturerWarrantyManagementService();
                ManufacturerWarrantyRequestDto result = ManufacturerWarrantyManagementService.UpdateManufacturerWarranty(ManufacturerWarranty, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Manufacturer Warranty Details Added");
                if (result.ManufacturerWarrantyInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add Manufacturer Warranty Details failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Manufacturer Warranty Details failed!";
            }
            
        }

        [HttpPost]
        public object GetManufacturerWarrantyById(JObject data)
        {
            try
            {

                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IManufacturerWarrantyManagementService ManufacturerWarrantyManagementService = ServiceFactory.GetManufacturerWarrantyManagementService();

                ManufacturerWarrantyResponseDto ManufacturerWarranty = ManufacturerWarrantyManagementService.GetManufacturerWarrantyById(Guid.Parse(data["Id"].ToString()),
                    SecurityHelper.Context,
                    AuditHelper.Context);
                return ManufacturerWarranty;              

                #region oldcord

                //SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                //IManufacturerWarrantyManagementService ManufacturerWarrantyManagementService = ServiceFactory.GetManufacturerWarrantyManagementService();

                //ManufacturerWarrantyResponseDto ManufacturerWarranty = ManufacturerWarrantyManagementService.GetManufacturerWarrantyById(Guid.Parse(data["Id"].ToString()),
                //    SecurityHelper.Context,
                //    AuditHelper.Context);
                //ManufacturerWarrantyInfo ret = new ManufacturerWarrantyInfo()
                //{
                //    ApplicableFrom = ManufacturerWarranty.ApplicableFrom,
                //   // CountryId = ManufacturerWarranty.CountryId,
                //    EntryDateTime = ManufacturerWarranty.EntryDateTime,
                //    EntryUser = ManufacturerWarranty.EntryUser,
                //    Id = ManufacturerWarranty.Id,
                //    MakeId = ManufacturerWarranty.MakeId,
                //    //ModelId = ManufacturerWarranty.ModelId,
                //    WarrantyKm = ManufacturerWarranty.WarrantyKm,
                //    WarrantyMonths = ManufacturerWarranty.WarrantyMonths,
                //    WarrantyName = ManufacturerWarranty.WarrantyName
                //};
                ////ret.Expired = ManufacturerWarrantyManagementService.GetManufacturerWarranties(
                ////SecurityHelper.Context,
                ////AuditHelper.Context).ManufacturerWarranties.FindAll(m => m.MakeId == ManufacturerWarranty.MakeId
                ////    && m.ModelId == ManufacturerWarranty.ModelId && m.CountryId == ManufacturerWarranty.CountryId && m.Id != ManufacturerWarranty.Id);
                //return ret;

               
                #endregion


            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
           
        }
        
        [HttpPost]
        public object GetManufacturerWarranties()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IManufacturerWarrantyManagementService ManufacturerWarrantyManagementService = ServiceFactory.GetManufacturerWarrantyManagementService();

                ManufacturerWarrantiesResponseDto ManufacturerWarrantyData = ManufacturerWarrantyManagementService.GetManufacturerWarranties(
                SecurityHelper.Context,
                AuditHelper.Context);
                return ManufacturerWarrantyData.ManufacturerWarranties.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

            
        }
      
        [HttpPost]
        public object GetManufacturerWarrantiesByCountryId(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IManufacturerWarrantyManagementService ManufacturerWarrantyManagementService = ServiceFactory.GetManufacturerWarrantyManagementService();

                //List<ManufacturerWarrantyResponseDto> ManufacturerWarrantyData = ManufacturerWarrantyManagementService.GetManufacturerWarranties(SecurityHelper.Context, AuditHelper.Context).
                //ManufacturerWarranties.FindAll(m => m.CountryId == Guid.Parse(data["Id"].ToString()));

                List<ManufacturerWarrantyResponseDto> ret = new List<ManufacturerWarrantyResponseDto>();
                //foreach (var item in ManufacturerWarrantyData)
                //{
                //    //List<ManufacturerWarrantyResponseDto> temp = ManufacturerWarrantyData.FindAll(m => m.CountryId == item.CountryId
                //    //    && m.MakeId == item.MakeId && m.ModelId == item.ModelId);
                //    //if (temp.Count == 1 && ret.FindAll(z => z.Id == item.Id).Count == 0)
                //    //    ret.Add(item);

                //    //ManufacturerWarrantyResponseDto mw = temp.OrderBy(x => x.ApplicableFrom).LastOrDefault();
                //    //if (ret.FindAll(m => m.Id == mw.Id).Count == 0)
                //    //{
                //    //    ret.Add(mw);
                //    //}
                //}
                return ret.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
                       

        }

        [HttpPost]
        public object GetManufacturerWarrantiesByCountryIdAndMakeId(JObject data)
        {

            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IManufacturerWarrantyManagementService ManufacturerWarrantyManagementService = ServiceFactory.GetManufacturerWarrantyManagementService();

                List<ManufacturerWarrantyResponseDto> ManufacturerWarrantyData = ManufacturerWarrantyManagementService.GetManufacturerWarranties(SecurityHelper.Context, AuditHelper.Context).
                ManufacturerWarranties.FindAll(m => m.MakeId == Guid.Parse(data["MakeId"].ToString()));

                List<ManufacturerWarrantyResponseDto> ret = new List<ManufacturerWarrantyResponseDto>();
                foreach (var item in ManufacturerWarrantyData)
                {
                    List<ManufacturerWarrantyResponseDto> temp = ManufacturerWarrantyData.FindAll(m => m.MakeId == item.MakeId );
                    if (ret.FindAll(z => z.Id == item.Id).Count == 0)
                    {
                        ret.Add(item);
                    }
                        

                    //ManufacturerWarrantyResponseDto mw = temp.OrderBy(x => x.ApplicableFrom).First();
                    //if (ret.FindAll(m => m.Id == mw.Id).Count == 0)
                    //{
                    //    ret.Add(mw);
                    //}
                }
                return ret.ToArray();
                
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
                     

        }


        [HttpPost]
        public object GetManufacturerWarranty(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IManufacturerWarrantyManagementService ManufacturerWarrantyManagementService = ServiceFactory.GetManufacturerWarrantyManagementService();

                ManufacturerWarrantiesResponseDto ManufacturerWarrantyData = ManufacturerWarrantyManagementService.GetManufacturerWarranties(
                SecurityHelper.Context,
                AuditHelper.Context);
                if (ManufacturerWarrantyData == null)
                    return null;
                //return ManufacturerWarrantyData.ManufacturerWarranties.FindAll(m => m.MakeId == Guid.Parse(data["MakeId"].ToString())
                //    && m.ModelId == Guid.Parse(data["ModelId"].ToString())
                //    && m.CountryId == Guid.Parse(data["CountryId"].ToString())).OrderBy(mm => mm.ApplicableFrom).First();
                return null; //--- delete this 
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
            
        }


        [HttpPost]
        public object SearchManufacturerWarrantySchemes(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                ManufacturerWarrentySearchRequestDto manufacturerWarrentySearchRequest = data.ToObject<ManufacturerWarrentySearchRequestDto>();
                IManufacturerWarrantyManagementService ManufacturerWarrantyManagementService = ServiceFactory.GetManufacturerWarrantyManagementService();
                response = ManufacturerWarrantyManagementService.SearchManufacturerWarrantySchemes(manufacturerWarrentySearchRequest,
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            {
                response = "Error occured";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }
    }
    public class ManufacturerWarrantyInfo:ManufacturerWarrantyResponseDto
    {
        public List<ManufacturerWarrantyResponseDto> Expired { get; set; }
    }
}
