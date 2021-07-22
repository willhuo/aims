using System.Threading.Tasks;
using Aims.Dto.Accounts;
using Aims.Dto.Users;

namespace Aims.AppService.Interfaces
{
    public interface IAccountAppService
    {
        /// <summary>
        /// 用户名密码登录
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        LoginDto JwtLoginByPwd(LoginByPwdArgs args);

        /// <summary>
        /// 简单方式的用户名密码登录
        /// </summary>
        /// <param name="args"></param>
        /// <param name="remoteIP"></param>
        /// <returns></returns>
        Task<LoginDto> SimplgLoginByPwd(LoginByPwdArgs args,string remoteIP);

        /// <summary>
        /// 用户缓存
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<UserDto> UserCache(string token);

        /// <summary>
        /// 用户缓存
        /// </summary>
        /// <returns></returns>
        Task<UserDto> UserCache();
    }
}
