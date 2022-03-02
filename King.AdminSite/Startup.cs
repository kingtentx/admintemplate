using Hangfire;
using King.AdminSite.Config;
using King.AdminSite.Filters;
using King.AdminSite.Models.MapperConfig;
using King.AdminSite.WeCat;
using King.BLL;
using King.Data;
using King.Helper;
using King.Interface;
using King.Jobs;
using log4net;
using log4net.Config;
using log4net.Repository;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;

namespace King.AdminSite
{
    public class Startup
    {
        public static ILoggerRepository logRepository { get; set; }
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            logRepository = LogManager.CreateRepository("King"); //��Ŀ����                                                              
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));  //ָ�������ļ�          
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region ���ݷ���
            services.AddDbContext<DataBase>(options =>
                 ////MSSQL
                 options.UseSqlServer(_configuration.GetConnectionString("sqlconn"), b => b.MigrationsAssembly("King.Data"))

                 ////Mysql
                 //options.UseMySql(Configuration.GetConnectionString("sqlconn"), b => b.MigrationsAssembly("King.Data"))
                 );

            DIBllRegister bllRegister = new DIBllRegister();
            bllRegister.DIRegister(services);
            #endregion

            #region  ���л�����
            services.AddMvc().AddNewtonsoftJson(options =>
            {
                //����ѭ������
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //������Ԫ���ݵ�key�Ĵ�Сд
                options.SerializerSettings.ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
                //�������л�ʱʱ���ʽΪyyyy-MM-dd HH:mm:ss            
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
            #endregion

            #region  ����
            services.AddMemoryCache();
            services.Configure<RedisConfig>(_configuration.GetSection("Redis"));
            //���ڳ�ʼ����ʱ�����Ǿ���Ҫ�ã�����ʹ��Bind�ķ�ʽ��ȡ����          
            var redisConfig = new RedisConfig();
            _configuration.Bind("Redis", redisConfig);
            if (redisConfig.IsEnabled)
            {
                services.AddSingleton(typeof(ICacheService), new RedisHelper(new RedisCacheOptions
                {
                    Configuration = redisConfig.ConnectionStrings,
                    InstanceName = redisConfig.InstanceName,
                }, redisConfig.DefaultDB));
            }
            else
            {
                services.AddSingleton<IMemoryCache>(factory =>
                {
                    var cache = new MemoryCache(new MemoryCacheOptions());
                    return cache;
                });
                services.AddSingleton<ICacheService, CacheHelper>();
            }
            #endregion

            //services.AddTransient<IPermissionService, Permission>();
            services.AddAutoMapper(typeof(AutomapperConfig));
            services.AddScoped<IPermission, Permission>();//Ȩ��

            #region ΢��
            services.Configure<WecatConfig>(_configuration.GetSection("WecatConfig"));
            var wecatConfig = new WecatConfig();
            _configuration.Bind("WecatConfig", wecatConfig);

            services.AddScoped<ResponseMessage>();
            services.AddScoped<WeixinUtils>();
            services.AddScoped<WeixinEvent>();
            #endregion           

            #region cookies ��֤
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    //��֤ʧ�ܣ����Զ���ת�������ַ
                    options.LoginPath = "/Admin/ReLogin";
                    options.LogoutPath = "/Admin/ReLogin";
                });

            services.Configure<CookiePolicyOptions>(options =>
            {
                //This lambda determines whether user consent for non-essential cookies is needed for a given request.
                //options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            #endregion

            #region  Hangfire  

            services.AddHangfire(x => x.UseSqlServerStorage(_configuration.GetConnectionString("sqlconn")));
            services.AddHostedService<JobService>();
            #endregion


            #region ����
            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            #endregion

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddMvc(options =>
            {
                options.Filters.Add<ActionFilter>();
                options.Filters.Add<GlobalExceptionFilter>(); //����ȫ���쳣��
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Admin/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }



            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(builder =>
                  builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()
                );

            app.UseAuthentication();//һ��Ҫ�����λ�ã�app.UseAuthorization()���棩jwt

            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute(
                //name: "Home",
                //pattern: "/Home/Article/info-{id}.html", new
                //{
                //    controller = "Home",
                //    action = "Article"
                //});

                //endpoints.MapControllerRoute(
                //name: "Home",
                //pattern: "/Home/{path}/tags-{id}.html", new
                //{
                //    controller = "Home",
                //    action = "Index"
                //});

                //endpoints.MapControllerRoute(
                //name: "Home",
                //pattern: "/Home/{path}.html", new
                //{
                //    controller = "Home",
                //    action = "Index"
                //});


                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Admin}/{action=Index}/{id?}");
            });

            #region Hangfire
            //������������
            var jobOptions = new BackgroundJobServerOptions()
            {
                Queues = new[] { "default", "task", "job" },//�������ƣ�ֻ��Сд
                WorkerCount = Environment.ProcessorCount * 5,//����������
                //SchedulePollingInterval = TimeSpan.FromSeconds(30), //�ƻ���ѯ���  ֧��������
                ServerName = "hangfire"//����������
            };
            app.UseHangfireServer(jobOptions);

            //�����Ǳ��̵ķ���·������Ȩ����
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] {
                    new HangfireAuthorizationFilter(_configuration["Hangfire:Name"],_configuration["Hangfire:Password"])
                }
            });         
          
            #endregion


            #region ��ʼ������
            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            var serviceScope = serviceScopeFactory.CreateScope();

            using (var dbContext = serviceScope.ServiceProvider.GetService<DataBase>())
            {
                //���ݿ��Ƿ����:  true=δ������ false=�Ѵ���
                if (!dbContext.Database.EnsureCreated())
                {
                    new DataInitializer().Create(dbContext);//ע��Ĭ�ϳ�������Ա
                }
            }
            #endregion
        }
    }
}
