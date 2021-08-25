using Sunday.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Sunday.Core.Extensions;

using Sunday.Core.Infrastructure;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Sunday.Core.Api.Filter;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Text;
using Autofac;
using Sunday.Core.Middlewares;
using System.Reflection;
using Sunday.Core.Project.Persistence.Seed;
using Sunday.Core.Tasks;

namespace Sunday.Core.Api
{
    public class Startup
    {
        private IServiceCollection _services;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IWebHostEnvironment Env { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new Appsettings(Env.ContentRootPath, Env.EnvironmentName));

            services.AddMemoryCacheSetup();
            services.AddRedisCacheSetup();
            services.AddSqlSugarSetup();
            services.AddDbSetup();
            services.AddAutoMapperSetup();
            services.AddCorsSetup();
            services.AddSwaggerSetup();
            services.AddJobSetup();
            services.AddHttpContextSetup();
            services.AddAppConfigSetup(Env);
            services.AddHttpApi();
            services.AddRedisInitMqSetup();

            services.AddRabbitMQSetup();
            services.AddKafkaSetup(Configuration);
            services.AddEventBusSetup();

            services.AddAuthorizationSetup();
            if (Permissions.IsUseIds4)
            {
                services.AddAuthentication_Ids4Setup();
            }
            else
            {
                services.AddAuthentication_JWTSetup();
            }

            services.AddIpPolicyRateLimitSetup(Configuration);

            services.AddSignalR().AddNewtonsoftJsonProtocol();

            services.Configure<KestrelServerOptions>(x => x.AllowSynchronousIO = true)
                    .Configure<IISServerOptions>(x => x.AllowSynchronousIO = true);

            services.AddControllers(o =>
            {
                // 全局异常过滤
                o.Filters.Add(typeof(GloalActionWrapFilter));
            }).AddNewtonsoftJson(options =>
             {
                 //忽略循环引用
                 options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                 //不使用驼峰样式的key
                 options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                 //设置时间格式
                 //options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
                 //忽略Model中为null的属性
                 //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                 //设置本地时间而非UTC时间
                 options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
             });

            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            //支持编码大全 例如:支持 System.Text.Encoding.GetEncoding("GB2312")  System.Text.Encoding.GetEncoding("GB18030")
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _services = services;
        }

        // 注意在Program.CreateHostBuilder，添加Autofac服务工厂
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModuleRegister());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MyContext myContext, ISchedulerCenter schedulerCenter, IHostApplicationLifetime lifetime)
        {
            // Ip限流,尽量放管道外层
            app.UseIpLimitMildd();
            // 记录请求与返回数据
            app.UseReuestResponseLog();
            // 用户访问记录(必须放到外层，不然如果遇到异常，会报错，因为不能返回流)
            app.UseRecordAccessLogsMildd();
            // signalr
            app.UseSignalRSendMildd();
            // 记录ip请求
            app.UseIPLogMildd();
            // 查看注入的所有服务
            app.UseAllServicesMildd(_services);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // 封装Swagger展示
            app.UseSwaggerMildd(() => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("Sunday.Core.Api.index.html"));

            // ↓↓↓↓↓↓ 注意下边这些中间件的顺序，很重要 ↓↓↓↓↓↓
            app.UseCors(Appsettings.App(new string[] { "Startup", "Cors", "PolicyName" }));
            // 使用静态文件
            app.UseStaticFiles();
            // 使用cookie
            app.UseCookiePolicy();
            // 返回错误码
            app.UseStatusCodePages();
            // Routing
            app.UseRouting();
            // 测试用户，用来通过鉴权
            if (Configuration.GetValue<bool>("AppSettings:UseLoadTest"))
            {
                app.UseMiddleware<ByPassAuthMidd>();
            }
            // 先开启认证
            app.UseAuthentication();
            // 然后是授权中间件
            app.UseAuthorization();
            //开启性能分析
            app.UseMiniProfilerMildd();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            // 生成种子数据
            app.UseSeedDataMildd(myContext, Env.WebRootPath);

            // 服务注册
            app.UseConsulMildd(Configuration, lifetime);

            // 事件总线，订阅服务
            app.ConfigureEventBus();
        }
    }
}