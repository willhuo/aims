using System.Collections.Generic;
using System.Linq;
using Aims.AppService.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Aims.AppService
{
    public class UserContext : IUserContext
    {
        private HttpContext HttpContext { get; }

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            HttpContext = httpContextAccessor.HttpContext;
        }

        public string Id => HttpContext.User?.FindFirst("id")?.Value;

        public string Uname => HttpContext.User?.FindFirst("uname")?.Value;

        public string Name => HttpContext.User?.FindFirst("name")?.Value;

        public string Token => HttpContext.User?.FindFirst("token")?.Value;

        public List<int> RoleIds
        {
            get
            {
                return HttpContext.User.FindAll("roleId").Select(p => int.Parse(p.Value)).ToList();
            }
        }
    }
}
