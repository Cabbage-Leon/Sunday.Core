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
                // ȫ���쳣����
                o.Filters.Add(typeof(GloalActionWrapFilter));
            }).AddNewtonsoftJson(options =>
             {
                 //����ѭ������
                 options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                 //��ʹ���շ���ʽ��key
                 options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                 //����ʱ���ʽ
                 //options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MyContext myContext, ISchedulerCenter schedulerCenter, IHostApplicationLifetime lifetime)
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
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
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