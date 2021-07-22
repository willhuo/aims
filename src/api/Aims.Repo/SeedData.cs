using System;
using System.Collections.Generic;
using System.Linq;
using Aims.Model;
using Dijing.Common.Core.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Aims.Repo
{
    /// <summary>
    /// 数据库初始化种子
    /// </summary>
    public static class SeedData
    {
        private static Dictionary<int, string> RoleList = new Dictionary<int, string>()
        {
            {1, "管理员"}
        };

        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <param name="host"></param>
        public static void Initialize(IHost host)
        {
            using var scope = host.Services.CreateScope();
            using var db = scope.ServiceProvider.GetRequiredService<AimsDbContext>();
            try
            {
                //DB迁移
                if (db.Database.GetPendingMigrations().Any())
                {
                    Serilog.Log.Warning("开始执行DB迁移");
                    db.Database.Migrate();
                    Serilog.Log.Warning("DB迁移完成");
                }

                //初始化角色
                var roleIdList = db.Roles.Select(x=>x.Id).ToList();
                foreach (KeyValuePair<int,string> kv in RoleList)
                {
                    if (!roleIdList.Contains(kv.Key))
                    {
                        db.Roles.Add(new Role()
                        {
                            Id = kv.Key,
                            Name = kv.Value
                        });
                    }
                }

                //初始化管理员
                var entity = db.Users.FirstOrDefault(x=>x.Uname=="admin");
                if (entity != null)
                {
                    entity.Uname = "admin";
                    entity.Pwd = EncryptHelper.Default.MD5Encode32("asdasd").ToLower();
                    entity.Nickname = "超级管理员";
                    db.Users.Update(entity);

                    var oldRoleIdList = db.UserRoles.Where(x => x.UserId == entity.Id).Select(x => x.RoleId).ToList();
                    foreach (KeyValuePair<int, string> kv in RoleList)
                    {
                        if (!oldRoleIdList.Contains(kv.Key))
                        {
                            db.UserRoles.Add(new UserRole(entity.Id,kv.Key));
                        }
                    }
                }
                else
                {
                    var user = new User
                    {
                        Id = Guid.NewGuid(),
                        Uname = "admin",
                        Pwd = EncryptHelper.Default.MD5Encode32("asdasd").ToLower(),
                        Nickname = "超级管理员",
                    };
                    db.Users.Add(user);

                    foreach (KeyValuePair<int, string> kv in RoleList)
                    {
                        db.UserRoles.Add(new UserRole(user.Id,kv.Key));
                    }
                }
                db.SaveChanges();

                Serilog.Log.Warning("系统初始化完成");
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "InitUser error");
            }
        }
    }
}
