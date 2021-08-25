using Microsoft.AspNetCore.Http;

namespace Sunday.Core.Serilog.ES.HttpInfo
{
    public static class HttpContextProvider
    {
        private static IHttpContextAccessor _accessor;

        public static HttpContext GetCurrent()
        {
            var context = _accessor?.HttpContext;
            return context;
        }
        public static void ConfigureAccessor(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
    }

}
