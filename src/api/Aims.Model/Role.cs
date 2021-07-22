using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Aims.Model
{
    /// <summary>
    /// 角色
    /// </summary>
    public class Role
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 导航属性-角色菜单列表
        /// </summary>
        public virtual List<RoleMenu> RoleMenus { get; set; }

        /// <summary>
        /// 导航属性-关联菜单列表
        /// </summary>
        [NotMapped]
        public List<Menu> Menus => RoleMenus != null ? RoleMenus.Select(p => p.Menu).ToList() : new List<Menu>();

        /// <summary>
        /// 导航属性-关联菜单ID列表
        /// </summary>
        [NotMapped]
        public List<int> MenuIdList
        {
            get
            {
                return RoleMenus?.Select(x => x.MenuId).ToList();
            }
        }

        /// <summary>
        /// 导航属性-关联菜单名称列表
        /// </summary>
        [NotMapped]
        public List<string> MenuNameList
        {
            get
            {
                return RoleMenus?.Select(x => x.Menu?.Name).ToList();
            }
        }
    }
}
