using System;
using log4net;
using Microsoft.AspNetCore.Builder;
using Sunday.Core.Infrastructure;
using Sunday.Core.Project.Persistence.Seed;

namespace Sunday.Core.Extensions
{
    /// <summary>
    /// 生成种子数据中间件服务
    /// </summary>
    public static class SeedDataMildd
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SeedDataMildd));

        public static void UseSeedDataMildd(this IApplicationBuilder app, MyContext myContext, string webRootPath)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            try
            {
                if (Appsettings.App("AppSettings", "SeedDBEnabled").ObjToBool() || Appsettings.App("AppSettings", "SeedDBDataEnabled").ObjToBool())
                {
                    DBSeed.SeedAsync(myContext, webRootPath).Wait();
                }
            }
            catch (Exception e)
            {
                log.Error($"Error occured seeding the Database.\n{e.Message}");
                throw;
            }
        }
    }
}