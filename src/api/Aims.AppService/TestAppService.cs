using Aims.AppService.Interfaces;

namespace Aims.AppService
{
    public class TestAppService: ITestAppService
    {
        private IUserContext _userContext { get; set; }

        public TestAppService(IUserContext userContext)
        {
            this._userContext = userContext;
        }

        public string UserCache()
        {
            return _userContext.Uname;
        }
    }
}
