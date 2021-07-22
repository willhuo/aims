using Aims.Dto.Menus;
using Aims.Dto.Roles;
using Aims.Dto.Users;
using Aims.Model;
using AutoMapper;

namespace Aims.Conf
{
    /// <summary>
    /// 映射扩展
    /// </summary>
    public class MapProfile : Profile
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MapProfile()
        {
            //用户
            CreateMap<User,UserDto>();

            //角色
            CreateMap<Role, RoleDto>();

            //菜单
            CreateMap<Menu, MenuDto>();
        }
    }
}
