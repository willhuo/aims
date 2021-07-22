using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Aims.Model
{
    /// <summary>
    /// 用户表
    /// </summary>
    public class User
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [StringLength(50)]
        public string Uname { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [StringLength(50)]
        public string Pwd { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [StringLength(50)]
        public string Nickname { get; set; }

        /// <summary>
        /// 最后登录IP
        /// </summary>
        public string LastLoginIP { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegTime { get; set; }

        /// <summary>
        /// 导航属性-用户角色列表
        /// </summary>
        public virtual List<UserRole> UserRoles { get; set; }

        /// <summary>
        /// 角色列表
        /// </summary>
        [NotMapped]
        public virtual List<Role> Roles => UserRoles?.Select(p => p.Role).ToList();

        /// <summary>
        /// 角色ID列表
        /// </summary>
        [NotMapped]
        public List<int> RoleIds => UserRoles?.Select(p => p.RoleId).ToList();

        /// <summary>
        /// 角色名称列表
        /// </summary>
        [NotMapped]
        public List<string> RoleNames => UserRoles?.Select(p => p.Role?.Name).ToList();
    }
}
