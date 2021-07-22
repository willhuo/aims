using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Aims.AppService.Interfaces;
using Dijing.Common.Core.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Aims.Validator
{
    /// <summary>
    /// 简单授权处理程序
    /// </summary>
    public class SimpleAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private IAccountAppService _accountAppService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="accountAppService"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <param name="encoder"></param>
        /// <param name="clock"></param>
        public SimpleAuthHandler(IAccountAppService accountAppService,
            IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
            this._accountAppService = accountAppService;
        }


        /// <summary>
        /// 授权处理
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string token = null;
            try
            {
                string authorization = Request.Headers["Authorization"];
                if (!authorization.IsNullorEmpty() &&
                    authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    token = authorization.Substring("Bearer ".Length).Trim();
                }

                if (token.IsNullorEmpty())
                {
                    token = Request.Query["token"];
                }

                if (token.IsNullorEmpty())
                {
                    return await Task.FromResult(AuthenticateResult.Fail("not find token"));
                }

                //redis用户缓存信息
                var userInfo = await _accountAppService.UserCache(token);
                if (userInfo != null)
                {
                    List<Claim> claims = new List<Claim>
                    {
                        new Claim("id", userInfo.Id.ToString()),
                        new Claim("uname", userInfo.Uname??""),
                        new Claim("name", userInfo.Name??""),
                        new Claim("sex", userInfo.Sex.ToString()),
                        new Claim("age", userInfo.Age.ToString()),
                        new Claim("nickName", userInfo.Nickname),
                        new Claim("token", token)
                    };
                    userInfo.RoleIds?.ForEach(r => { claims.Add(new Claim("roleId", r.ToString())); });
                    userInfo.RoleNames?.ForEach(r => { claims.Add(new Claim("roleName", r)); });
                    var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, Scheme.Name));
                    var properties = new AuthenticationProperties();
                    properties.StoreTokens(new[] {new AuthenticationToken {Name = "token", Value = token}});
                    var ticket = new AuthenticationTicket(principal, properties, Scheme.Name);
                    return await Task.FromResult(AuthenticateResult.Success(ticket));
                }

                return await Task.FromResult(AuthenticateResult.Fail("无效的token:" + token));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "A Exception Occur When Handle Authenticate!");
                throw;
            }
        }
    }
}
