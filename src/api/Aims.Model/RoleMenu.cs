using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aims.Model
{
    /// <summary>
    /// 角色菜单关系
    /// </summary>
    public class RoleMenu
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        public int MenuId { get; set; }

        /// <summary>
        /// 导航属性-角色
        /// </summary>
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        /// <summary>
        /// 导航属性-角色
        /// </summary>
        [ForeignKey("MenuId")]
        public virtual Menu Menu { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="menuId"></param>
        public RoleMenu(int roleId, int menuId)
        {
            this.RoleId = roleId;
            this.MenuId = menuId;
        }
    }
}
