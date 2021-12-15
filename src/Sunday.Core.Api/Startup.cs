using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sunday.Core.Api.Filter;
using Sunday.Core.Extensions;
using Sunday.Core.Infrastructure;
using Sunday.Core.Middlewares;
using Sunday.Core.Project.Persistence.Seed;
using Sunday.Core.Tasks;
using System.Reflection;
using System.Text;
using Sunday.Core.Extensions.LogExtend;
using Sunday.Core.Extensions.ElasticSearchLogExtend;

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
            //ElasticSearch��־
            //services.Configure<ESOptions>(Configuration.GetSection("ElasticSearch"));
            //services.AddSingleton<ESClientProvider>();

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

            //��Ȩ
            services.AddAuthorizationSetup();

            //��֤ -- �������ʵ���˶�����֤
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
                // ȫ���쳣����
                o.Filters.Add(typeof(GloalActionWrapFilter));
                o.Conventions.Insert(0, new GlobalRoutePrefixFilter(new RouteAttribute(RoutePrefix.Name)));
            }).AddNewtonsoftJson(options =>
             {
                 //����ѭ������
                 options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                 //��ʹ���շ���ʽ��key
                 options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                 //����ʱ���ʽ
                 options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
                 //����Model��Ϊnull������
                 //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                 //���ñ���ʱ�����UTCʱ��
                 options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
             });

            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            //֧�ֱ����ȫ ����:֧�� System.Text.Encoding.GetEncoding("GB2312")  System.Text.Encoding.GetEncoding("GB18030")
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _services = services;
        }

        // ע����Program.CreateHostBuilder�����Autofac���񹤳�
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModuleRegister());
            builder.RegisterModule<AutofacPropertityModuleReg>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MyContext myContext, ISchedulerCenter schedulerCenter, IHostApplicationLifetime lifetime, ILoggerFactory loggerFactory)
        {
            // Ip����,�����Źܵ����
            app.UseIpLimitMildd();
            // ��¼�����뷵������
            app.UseReuestResponseLog();
            // �û����ʼ�¼(����ŵ���㣬��Ȼ��������쳣���ᱨ����Ϊ���ܷ�����)
            app.UseRecordAccessLogsMildd();
            // signalr
            app.UseSignalRSendMildd();
            // ��¼ip����
            app.UseIPLogMildd();
            // �鿴ע������з���
            app.UseAllServicesMildd(_services);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                #region ���Log4Net�����Զ����
                //loggerFactory.AddLog4Net();
                //loggerFactory.AddProvider(new CustomConsoleLoggerProvider())
                //loggerFactory.AddCustomConsoleLogger();

                //loggerFactory.AddCustomConsoleLogger(
                //    new CustomConsoleLoggerConfiguration
                //    {
                //        LogLevel = LogLevel.Debug,
                //        EventId = 0
                //    }) ;

                loggerFactory.AddCustomConsoleLogger(c =>
                {
                    c.LogLevel = LogLevel.Debug;
                    c.Init("Sunday");
                });
                //�Զ���ELK��־
                //loggerFactory.AddESLogger(new ESLoggerConfiguration()
                //{
                //    ServiceProvider = app.ApplicationServices,
                //    IndexName = "log-",
                //    Filter = new FilterLoggerSettings
                //    {
                //        {"*", LogLevel.Information}
                //    }
                //});
                    
                #endregion
            }

            // ��װSwaggerչʾ
            app.UseSwaggerMildd(() => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("Sunday.Core.Api.index.html"));

            // ������������ ע���±���Щ�м����˳�򣬺���Ҫ ������������
            app.UseCors(Appsettings.App(new string[] { "Startup", "Cors", "PolicyName" }));
            // ʹ�þ�̬�ļ�
            app.UseStaticFiles();
            // ʹ��cookie
            app.UseCookiePolicy();
            // ���ش�����
            app.UseStatusCodePages();
            // Routing
            app.UseRouting();
            // �����û�������ͨ����Ȩ
            if (Configuration.GetValue<bool>("AppSettings:UseLoadTest"))
            {
                app.UseMiddleware<ByPassAuthMidd>();
            }
            // �ȿ�����֤
            app.UseAuthentication();
            // Ȼ������Ȩ�м��
            app.UseAuthorization();
            //�������ܷ���
            app.UseMiniProfilerMildd();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<ChatHub>("/api2/chatHub");
            });

            // ������������
            app.UseSeedDataMildd(myContext, Env.WebRootPath);

            // ����ע��
            app.UseConsulMildd(Configuration, lifetime);

            // �¼����ߣ����ķ���
            app.ConfigureEventBus();
        }
    }
}