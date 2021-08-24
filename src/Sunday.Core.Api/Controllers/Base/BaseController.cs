using Homsom.Friday.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Sunday.Core.Api.Controllers.Base
{
    [ServiceFilter(typeof(IApiResultWrapAttribute))]
    [Produces("application/json")]
    [ApiController]
    public class BaseController : ControllerBase
    {
    }
}