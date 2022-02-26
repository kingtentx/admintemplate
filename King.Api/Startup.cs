using King.Api.Filters;
using King.Api.Models.MapperConfig;
using King.BLL;
using King.Data;
using King.Helper;
using King.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Text;

namespace King.Api
{
    public class Startup
    {

        private readonly string AllowSpecificOrigin = "AllowSpecificOrigin";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            #region 数据访问
            services.AddDbContext<DataBase>(options =>
                 //MSSQL
                 options.UseSqlServer(Configuration.GetConnectionString("sqlconn"), b => b.MigrationsAssembly("King.Data"))

                 //Mysql
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
            services.Configure<RedisConfig>(Configuration.GetSection("RedisServer"));
            //由于初始化的时候我们就需要用，所以使用Bind的方式读取配置          
            var redisConfig = new RedisConfig();
            Configuration.Bind("RedisServer", redisConfig);
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

            #region  AutoMapper
            services.AddAutoMapper(typeof(AutomapperConfig));
            #endregion

            #region jwt认证
            //将appsettings.json中的JwtSettings部分文件读取到JwtSettings中，这是给其他地方用的
            services.Configure<JwtConfig>(Configuration.GetSection("JwtSettings"));
            //由于初始化的时候我们就需要用，所以使用Bind的方式读取配置
            //将配置绑定到JwtSettings实例中
            var jwtSettings = new JwtConfig();
            Configuration.Bind("JwtSettings", jwtSettings);

            services.AddAuthentication(options =>
            {
                //认证middleware配置
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                //主要是jwt  token参数设置
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    //Token颁发机构
                    ValidIssuer = jwtSettings.Issuer,
                    //颁发给谁
                    ValidAudience = jwtSettings.Audience,
                    //这里的key要进行加密，需要引用Microsoft.IdentityModel.Tokens
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ////是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                    ValidateLifetime = true,
                    ////允许的服务器时间偏移量
                    ClockSkew = TimeSpan.Zero
                };
            });
            #endregion

            #region 注册Swagger服务
            services.AddSwaggerGen(options =>
            {
                // 添加文档信息
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "King_Api",
                    Description = ".NET Core 3.x的API项目",
                    //Contact = new OpenApiContact
                    //{
                    //    Name = "Microfisher",
                    //    Email = "80789097@qq.com",
                    //    Url = new Uri("http://cnblogs.com/kingtentx"),
                    //}
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer {Token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "King.Api.xml");
                options.IncludeXmlComments(xmlPath, true); //添加控制器层注释（true表示显示控制器注释）

            });
            #endregion

            #region 跨域         

            services.AddCors(options =>
            {
                options.AddPolicy(AllowSpecificOrigin,
                    builder =>
                    {
                        builder.AllowAnyMethod()
                            .AllowAnyOrigin()
                            .AllowAnyHeader();
                    });
            });
            #endregion

            services.AddMvc(options =>
            {
                //options.Filters.Add<ActionFilter>();
                //options.Filters.Add<GlobalExceptionFilter>(); //加入全局异常类               

            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            //CORS 中间件必须配置为在对 UseRouting 和 UseEndpoints的调用之间执行。 配置不正确将导致中间件停止正常运行。
            app.UseCors(AllowSpecificOrigin);

            //app.UseHttpsRedirection();           

            app.UseAuthentication();//一定要在这个位置（app.UseAuthorization()上面）jwt

            app.UseAuthorization();

            //启用Swagger中间件
            app.UseSwagger();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api/{documentName}/swagger.json";
            });
            // 配置SwaggerUI       
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "api";
                c.SwaggerEndpoint("/api/v1/swagger.json", "King_Api");
            });

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
