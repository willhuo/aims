using System.Collections.Generic;

namespace Aims.AppService.Interfaces
{
    /// <summary>
    /// 用户上下文
    /// </summary>
    public interface IUserContext
    {
        /// <summary>
        /// 用户ID，GUID
        /// </summary>
        string Id { get; }

        /// <summary>
        /// 登录名称
        /// </summary>
        string Uname { get; }

        /// <summary>
        /// 姓名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 授权码
        /// </summary>
        string Token { get; }

        /// <summary>
        /// 角色ID列表
        /// </summary>
        List<int> RoleIds { get; }
    }
}
