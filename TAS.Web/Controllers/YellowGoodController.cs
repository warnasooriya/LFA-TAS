using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TAS.DataTransfer.Requests;
using TAS.Services;
using TAS.Web.Common;

namespace TAS.Web.Controllers
{
    public class YellowGoodController : ApiController
    {
        [HttpPost]
        public object GetAllItemsForSearchGrid(YellowGoodSearchGridRequestDto YellowGoodSearchGridRequestDto)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IYellowGoodManagementService YellowGoodManagementService = ServiceFactory.GetYellowGoodManagementService();
            return YellowGoodManagementService.GetAllItemsForSearchGrid(YellowGoodSearchGridRequestDto,
                SecurityHelper.Context,
            AuditHelper.Context);
        }
    }
}
