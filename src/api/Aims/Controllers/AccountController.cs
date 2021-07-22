using System;
using System.Threading.Tasks;
using Aims.AppService.Interfaces;
using Aims.Dto.Accounts;
using Aims.Dto.Users;
using AIMS.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aims.Controllers
{
    /// <summary>
    /// 账号控制器
    /// </summary>
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize]
    public class AccountController:ControllerBase
    {
        private IAccountAppService _accountAppService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="accountAppService"></param>
        public AccountController(IAccountAppService accountAppService)
        {
            this._accountAppService = accountAppService;
        }

        /// <summary>
        /// JWT方式用户名密码登录
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost("[Action]")]
        [AllowAnonymous]
        public ActionResult<LoginDto> JwtLoginByPwd([FromBody]LoginByPwdArgs args)
        {
            return StatusCode(200);
            //return _accountAppService.JwtLoginByPwd(args);
        }

        /// <summary>
        /// 简单方式的用户名密码登录
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost("[Action]")]
        [AllowAnonymous]
        public async Task<LoginDto> SimplgLoginByPwd([FromBody]LoginByPwdArgs args)
        {
            var remoteIP = HttpContext.Connection.RemoteIpAddress?.ToString();
            return await _accountAppService.SimplgLoginByPwd(args,remoteIP);
        }

        /// <summary>
        /// 用户缓存
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public async Task<UserDto> UserCache()
        {
            return await _accountAppService.UserCache();
        }
    }
}
