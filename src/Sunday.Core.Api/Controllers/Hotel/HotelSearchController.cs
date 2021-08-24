using Microsoft.AspNetCore.Mvc;
using Sunday.Core.Api.Controllers.Base;

namespace Sunday.Core.Api.Controllers.Hotel
{
    [Route("api/hotelsearch")]
    public class HotelSearchController : BaseController
    {
        public HotelSearchController()
        {
        }

        [HttpGet]
        public string Test()
        {
            return "1";
        }
    }
}