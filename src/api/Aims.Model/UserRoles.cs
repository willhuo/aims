using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aims.Model
{
    /// <summary>
    /// 用户角色关系
    /// </summary>
    public class UserRole 
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 导航属性-用户
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        /// <summary>
        /// 导航属性-角色
        /// </summary>
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        public UserRole(Guid userId, int roleId)
        {
            this.UserId = userId;
            this.RoleId = roleId;
        }
    }
}
