using Aims.Option;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aims.Conf
{
    /// <summary>
    /// 配置扩展
    /// </summary>
    public static class OptionConf
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddOptionConfig(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddOptions();
            services.Configure<JwtOption>(Configuration.GetSection("Jwt"));
            return services;
        }
    }
}
