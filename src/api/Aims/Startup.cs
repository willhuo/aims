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
    /// �������
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// ���캯��
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
                    Title = "������ý�����", 
                    Version = "v1",
                    Description = "������ý�����ӿ��ĵ�",
                    Contact = new OpenApiContact() {Name = "willhuo", Email = "willhuo@outlook.com"}
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"������Bearer Token.����: 'Bearer 12345abcdef'",
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

            //�������
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
            });

            //DB
            services.AddDbContext<AimsDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("Connstr"), new MariaDbServerVersion("5.5.68"));
            });

            //redis��̬ʵ��
            var redisConnStr = Configuration.GetConnectionString("Redis");
            var csredis = new CSRedisClient(redisConnStr);
            RedisHelper.Initialization(csredis);

            //����
            services.AddOptionConfig(Configuration);
            services.AddAutoMapper(typeof(MapProfile));

            //ע������
            services.AddDIConfig();

            //jwt tokenУ������(���Ժͼ�У�����ý����л�)
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

            //��У������(����jwt���ý����л�)
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
