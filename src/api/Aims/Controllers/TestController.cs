using Aims.AppService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aims.Controllers
{
    /// <summary>
    /// 测试控制器
    /// </summary>
    [ApiController]
    [Route("api/[Controller]/[Action]")]
    [Authorize]
    public class TestController:ControllerBase
    {
        private ITestAppService _testAppService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="testAppService"></param>
        public TestController(ITestAppService testAppService)
        {
            this._testAppService = testAppService;
        }

        /// <summary>
        /// 授权测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string AuthTest()
        {
            return "hello world";
        }

        /// <summary>
        /// 匿名测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public string AllowAnonymousTest()
        {
            return "hello world 2";
        }

        /// <summary>
        /// 用户缓存
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string UserCache()
        {
            return _testAppService.UserCache();
        }
    }
}
