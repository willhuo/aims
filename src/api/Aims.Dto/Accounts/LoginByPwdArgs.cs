namespace Aims.Dto.Accounts
{
    /// <summary>
    /// 登录参数
    /// </summary>
    public class LoginByPwdArgs
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Uname { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }
    }
}
