using Microsoft.EntityFrameworkCore;
using System;
using King.Data;

namespace King.Data
{
    public class DataBase : DbContext
    {
        //构造方法
        public DataBase(DbContextOptions<DataBase> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

        #region 数据区域
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<RoleMenu> RoleMenu { get; set; }      
        public DbSet<Article> Article { get; set; }
        public DbSet<Album> Album { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<AdminLogin> AdminLogin { get; set; }
        public DbSet<User_Reply> User_Reply { get; set; }
        public DbSet<PictureGallery> PictureGallery { get; set; }
        public DbSet<Attachments> Attachments { get; set; }
        public DbSet<SiteConfig> SiteConfig { get; set; }
        public DbSet<Wx_Media> Wx_Media { get; set; }
        public DbSet<Wx_MenuViewLog> Wx_MenuViewLog { get; set; }
        public DbSet<Wx_KeyWordsReply> Wx_KeyWordsReply { get; set; }
        public DbSet<Wx_Keywords> Wx_Keywords { get; set; }
        public DbSet<Navigation> Navigation { get; set; } 
        public DbSet<Tags> Ct_Tags { get; set; }
       
        #endregion

    }
}
