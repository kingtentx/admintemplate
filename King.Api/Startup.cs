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

            #region ���ݷ���
            services.AddDbContext<DataBase>(options =>
                 //MSSQL
                 options.UseSqlServer(Configuration.GetConnectionString("sqlconn"), b => b.MigrationsAssembly("King.Data"))

                 //Mysql
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
            services.Configure<RedisConfig>(Configuration.GetSection("RedisServer"));
            //���ڳ�ʼ����ʱ�����Ǿ���Ҫ�ã�����ʹ��Bind�ķ�ʽ��ȡ����          
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

            #region jwt��֤
            //��appsettings.json�е�JwtSettings�����ļ���ȡ��JwtSettings�У����Ǹ������ط��õ�
            services.Configure<JwtConfig>(Configuration.GetSection("JwtSettings"));
            //���ڳ�ʼ����ʱ�����Ǿ���Ҫ�ã�����ʹ��Bind�ķ�ʽ��ȡ����
            //�����ð󶨵�JwtSettingsʵ����
            var jwtSettings = new JwtConfig();
            Configuration.Bind("JwtSettings", jwtSettings);

            services.AddAuthentication(options =>
            {
                //��֤middleware����
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                //��Ҫ��jwt  token��������
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    //Token�䷢����
                    ValidIssuer = jwtSettings.Issuer,
                    //�䷢��˭
                    ValidAudience = jwtSettings.Audience,
                    //�����keyҪ���м��ܣ���Ҫ����Microsoft.IdentityModel.Tokens
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ////�Ƿ���֤Token��Ч�ڣ�ʹ�õ�ǰʱ����Token��Claims�е�NotBefore��Expires�Ա�
                    ValidateLifetime = true,
                    ////����ķ�����ʱ��ƫ����
                    ClockSkew = TimeSpan.Zero
                };
            });
            #endregion

            #region ע��Swagger����
            services.AddSwaggerGen(options =>
            {
                // ����ĵ���Ϣ
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "King_Api",
                    Description = ".NET Core 3.x��API��Ŀ",
                    //Contact = new OpenApiContact
                    //{
                    //    Name = "Microfisher",
                    //    Email = "80789097@qq.com",
                    //    Url = new Uri("http://cnblogs.com/kingtentx"),
                    //}
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "���¿�����������ͷ����Ҫ���Jwt��ȨToken��Bearer {Token}",
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
                options.IncludeXmlComments(xmlPath, true); //��ӿ�������ע�ͣ�true��ʾ��ʾ������ע�ͣ�

            });
            #endregion

            #region ����         

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
                //options.Filters.Add<GlobalExceptionFilter>(); //����ȫ���쳣��               

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
            //CORS �м����������Ϊ�ڶ� UseRouting �� UseEndpoints�ĵ���֮��ִ�С� ���ò���ȷ�������м��ֹͣ�������С�
            app.UseCors(AllowSpecificOrigin);

            //app.UseHttpsRedirection();           

            app.UseAuthentication();//һ��Ҫ�����λ�ã�app.UseAuthorization()���棩jwt

            app.UseAuthorization();

            //����Swagger�м��
            app.UseSwagger();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api/{documentName}/swagger.json";
            });
            // ����SwaggerUI       
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
