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
            logRepository = LogManager.CreateRepository("King"); //项目名称                                                              
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));  //指定配置文件          
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region 数据访问
            services.AddDbContext<DataBase>(options =>
                 ////MSSQL
                 options.UseSqlServer(_configuration.GetConnectionString("sqlconn"), b => b.MigrationsAssembly("King.Data"))

                 ////Mysql
                 //options.UseMySql(Configuration.GetConnectionString("sqlconn"), b => b.MigrationsAssembly("King.Data"))
                 );

            DIBllRegister bllRegister = new DIBllRegister();
            bllRegister.DIRegister(services);
            #endregion

            #region  序列化数据
            services.AddMvc().AddNewtonsoftJson(options =>
            {
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //不更改元数据的key的大小写
                options.SerializerSettings.ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
                //配置序列化时时间格式为yyyy-MM-dd HH:mm:ss            
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
            #endregion

            #region  缓存
            services.AddMemoryCache();
            services.Configure<RedisConfig>(_configuration.GetSection("Redis"));
            //由于初始化的时候我们就需要用，所以使用Bind的方式读取配置          
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
            services.AddScoped<IPermission, Permission>();//权限

            #region 微信
            services.Configure<WecatConfig>(_configuration.GetSection("WecatConfig"));
            var wecatConfig = new WecatConfig();
            _configuration.Bind("WecatConfig", wecatConfig);

            services.AddScoped<ResponseMessage>();
            services.AddScoped<WeixinUtils>();
            services.AddScoped<WeixinEvent>();
            #endregion           

            #region cookies 认证
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    //认证失败，会自动跳转到这个地址
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


            #region 跨域
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
                options.Filters.Add<GlobalExceptionFilter>(); //加入全局异常类
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

            app.UseAuthentication();//一定要在这个位置（app.UseAuthorization()上面）jwt

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
            //配置任务属性
            var jobOptions = new BackgroundJobServerOptions()
            {
                Queues = new[] { "default", "task", "job" },//队列名称，只能小写
                WorkerCount = Environment.ProcessorCount * 5,//并发任务数
                //SchedulePollingInterval = TimeSpan.FromSeconds(30), //计划轮询间隔  支持任务到秒
                ServerName = "hangfire"//服务器名称
            };
            app.UseHangfireServer(jobOptions);

            //控制仪表盘的访问路径和授权配置
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] {
                    new HangfireAuthorizationFilter(_configuration["Hangfire:Name"],_configuration["Hangfire:Password"])
                }
            });         
          
            #endregion


            #region 初始化数据
            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            var serviceScope = serviceScopeFactory.CreateScope();

            using (var dbContext = serviceScope.ServiceProvider.GetService<DataBase>())
            {
                //数据库是否存在:  true=未创建， false=已创建
                if (!dbContext.Database.EnsureCreated())
                {
                    new DataInitializer().Create(dbContext);//注册默认超级管理员
                }
            }
            #endregion
        }
    }
}
