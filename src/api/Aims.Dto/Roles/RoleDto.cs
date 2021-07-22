using System.Collections.Generic;
using Aims.Dto.Menus;

namespace Aims.Dto.Roles
{
    /// <summary>
    /// 角色DTO
    /// </summary>
    public class RoleDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 导航属性-关联菜单列表
        /// </summary>
        public List<MenuDto> Menus { get; set; }

        /// <summary>
        /// 导航属性-关联菜单ID列表
        /// </summary>
        public List<int> MenuIdList { get; set; }

        /// <summary>
        /// 导航属性-关联菜单名称列表
        /// </summary>
        public List<string> MenuNameList { get; set; }
    }
}
