using System;
using System.IO;
using Aims.Conf;
using Aims.Filters;
using Aims.Repo;
using Aims.Validator;
using CSRedis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Aims
{
    /// <summary>
    /// 配置入口
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        /// <summary>
        ///  This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers(options =>
            {
                options.Filters.Add<AimsExceptionFilter>();
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "智能流媒体服务", 
                    Version = "v1",
                    Description = "智能流媒体服务接口文档",
                    Contact = new OpenApiContact() {Name = "willhuo", Email = "willhuo@outlook.com"}
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"请输入Bearer Token.例如: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                var xmls = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
                foreach (var xml in xmls)
                {
                    c.IncludeXmlComments(xml, true);
                }
                c.CustomSchemaIds(s => s.FullName);
            });

            //Cors
            services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(builder =>
                {
                    var origins = Configuration["Cors:WithOrigins"].Split(';');
                    builder.SetIsOriginAllowedToAllowWildcardSubdomains()
                        .WithOrigins(origins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithExposedHeaders("WWW-Authenticate");
                });
            });

            //反向代理
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
            });

            //DB
            services.AddDbContext<AimsDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("Connstr"), new MariaDbServerVersion("5.5.68"));
            });

            //redis静态实例
            var redisConnStr = Configuration.GetConnectionString("Redis");
            var csredis = new CSRedisClient(redisConnStr);
            RedisHelper.Initialization(csredis);

            //配置
            services.AddOptionConfig(Configuration);
            services.AddAutoMapper(typeof(MapProfile));

            //注入配置
            services.AddDIConfig();

            //jwt token校验配置(可以和简单校验配置进行切换)
            //var jwtOption= Configuration.GetSection("Jwt").Get<JwtOption>();
            //services.AddAuthentication(x=>
            //    {
            //        x.DefaultAuthenticateScheme= JwtBearerDefaults.AuthenticationScheme;
            //        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //    })
            //    .AddJwtBearer(options =>
            //    {
            //        options.SaveToken = true;
            //        options.RequireHttpsMetadata = false;
            //        options.TokenValidationParameters = new TokenValidationParameters()
            //        {
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.Secret)),
            //            ValidateIssuer = false,
            //            ValidIssuer = jwtOption.Issuer,
            //            ValidateAudience = false,
            //            ValidAudience = jwtOption.Audience,
            //            ValidateLifetime = true,
            //            ClockSkew = TimeSpan.FromMinutes(jwtOption.AccessExpiration),

            //        };
            //    });

            //简单校验配置(可以jwt配置进行切换)
            services.AddAuthentication(SimpleAuthenticationDefaults.AuthenticationSchema)
                .AddScheme<AuthenticationSchemeOptions, SimpleAuthHandler>(SimpleAuthenticationDefaults.AuthenticationSchema,x=> { });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Aims v1"));

            app.UseForwardedHeaders();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
