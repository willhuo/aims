using Aims.AppService;
using Aims.AppService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Aims.Conf
{
    /// <summary>
    /// 接口注入配置扩展
    /// </summary>
    public static class DIConf
    {
        /// <summary>
        /// 接口注入
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDIConfig(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<IAccountAppService, AccountAppService>();
            services.AddScoped<ITestAppService, TestAppService>();

            return services;
        }
    }
}
