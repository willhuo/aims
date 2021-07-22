using Aims.Dto.Users;

namespace Aims.Dto.Accounts
{
    /// <summary>
    /// 登录结果
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 用户对象
        /// </summary>
        public UserDto UserDto { get; set; }
    }
}
