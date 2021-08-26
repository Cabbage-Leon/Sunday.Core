using Microsoft.AspNetCore.Mvc;

namespace Sunday.Core.Api.Controllers
{
    /// <summary>
    /// 健康检查
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        /// <summary>
        /// 健康检查接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}