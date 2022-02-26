using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace King.Data
{
    /// <summary>
    /// 初始化数据
    /// </summary>
    public class DataInitializer
    {
        public void Create(DataBase context)
        {
            var system_user = context.Admin.FirstOrDefault(p => p.UserName == "admin");
            if (system_user == null)
            {
                //    //添加角色
                //    var role = new List<Role>
                //{
                //     new Role{ RoleName= "管理员",Description = "系统管理员",CreateTime=DateTime.Now,IsActive=true}
                //};
                //    role.ForEach(s => context.Role.Add(s));
                //    context.SaveChanges();

                //添加管理员

                var pwd = "111111"; //密码
                var admin = new List<Admin>
                {
                     new Admin{ UserName="admin",RealName= "管理员",Password = ToMD5(pwd),CreateTime=DateTime.Now,IsAdmin=true,IsActive=true,Roles="0"}
                };
                admin.ForEach(s => context.Admin.Add(s));
                context.SaveChanges();
            }
        }

        private string ToMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes_out = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));
            string result = BitConverter.ToString(bytes_out).Replace("-", "");
            return result;
        }
    }
}
