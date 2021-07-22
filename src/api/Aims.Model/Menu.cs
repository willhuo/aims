using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aims.Model
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [StringLength(50)]
        public string Url { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [StringLength(50)]
        public string Icon { get; set; }

        /// <summary>
        /// 上级菜单ID
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// 导航属性-上级菜单
        /// </summary>
        [ForeignKey("ParentId")]
        public virtual Menu Parent { get; set; }

        /// <summary>
        /// 权重，数值越大排名越靠前
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string Remark { get; set; }

        /// <summary>
        /// 导航属性-子菜单
        /// </summary>
        public virtual List<Menu> SubMenus { get; set; } = new List<Menu>();
    }
}
