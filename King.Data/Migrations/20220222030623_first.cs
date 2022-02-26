using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace King.Data.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    AdminId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    UserName = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(maxLength: 50, nullable: false),
                    Telphone = table.Column<string>(maxLength: 50, nullable: true),
                    RealName = table.Column<string>(maxLength: 50, nullable: true),
                    Remark = table.Column<string>(maxLength: 500, nullable: true),
                    Roles = table.Column<string>(maxLength: 100, nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.AdminId);
                });

            migrationBuilder.CreateTable(
                name: "AdminLogin",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(maxLength: 50, nullable: false),
                    Client = table.Column<string>(maxLength: 500, nullable: true),
                    LoginDate = table.Column<DateTime>(nullable: false),
                    LoginIp = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminLogin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Album",
                columns: table => new
                {
                    AlbumId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdateBy = table.Column<string>(maxLength: 50, nullable: true),
                    ImageUrl = table.Column<string>(maxLength: 500, nullable: true),
                    Title = table.Column<string>(maxLength: 250, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Author = table.Column<string>(maxLength: 50, nullable: true),
                    TagsType = table.Column<int>(nullable: false),
                    TagsId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Album", x => x.AlbumId);
                });

            migrationBuilder.CreateTable(
                name: "Article",
                columns: table => new
                {
                    ArticleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdateBy = table.Column<string>(maxLength: 50, nullable: true),
                    Title = table.Column<string>(maxLength: 250, nullable: false),
                    Keyword = table.Column<string>(maxLength: 250, nullable: true),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Detail = table.Column<string>(nullable: true),
                    Author = table.Column<string>(maxLength: 50, nullable: true),
                    Source = table.Column<string>(maxLength: 100, nullable: true),
                    SourceUrl = table.Column<string>(maxLength: 250, nullable: true),
                    LinkUrl = table.Column<string>(maxLength: 250, nullable: true),
                    ImageUrl = table.Column<string>(maxLength: 500, nullable: true),
                    TagsType = table.Column<int>(nullable: false),
                    TagsId = table.Column<int>(nullable: false),
                    Sort = table.Column<int>(nullable: false),
                    ViewCount = table.Column<int>(nullable: false),
                    RecommendPosition = table.Column<string>(maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.ArticleId);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    FileName = table.Column<string>(maxLength: 100, nullable: true),
                    Url = table.Column<string>(maxLength: 250, nullable: true),
                    ExtensionName = table.Column<string>(maxLength: 10, nullable: true),
                    MD5 = table.Column<string>(maxLength: 100, nullable: true),
                    Size = table.Column<long>(nullable: false),
                    CreateBy = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdateBy = table.Column<string>(maxLength: 50, nullable: true),
                    ParentId = table.Column<int>(nullable: false),
                    CategoryName = table.Column<string>(maxLength: 50, nullable: true),
                    CategoryType = table.Column<int>(nullable: false),
                    Sort = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Ct_Content",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdateBy = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Content = table.Column<string>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ct_Content", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ct_Image",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdateBy = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    ImageJson = table.Column<string>(maxLength: 5000, nullable: true),
                    ImageType = table.Column<int>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ct_Image", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ct_Tags",
                columns: table => new
                {
                    TagsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdateBy = table.Column<string>(maxLength: 50, nullable: true),
                    TagsName = table.Column<string>(maxLength: 50, nullable: true),
                    TagsType = table.Column<int>(nullable: false),
                    Sort = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ct_Tags", x => x.TagsId);
                });

            migrationBuilder.CreateTable(
                name: "Ct_Title",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdateBy = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Title = table.Column<string>(maxLength: 500, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    IsDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ct_Title", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Job",
                columns: table => new
                {
                    JobId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdateBy = table.Column<string>(maxLength: 50, nullable: true),
                    JobName = table.Column<string>(maxLength: 250, nullable: false),
                    Author = table.Column<string>(maxLength: 50, nullable: true),
                    Detail = table.Column<string>(nullable: true),
                    TagsType = table.Column<int>(nullable: false),
                    TagsId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job", x => x.JobId);
                });

            migrationBuilder.CreateTable(
                name: "Navigation",
                columns: table => new
                {
                    NavigationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdateBy = table.Column<string>(maxLength: 50, nullable: true),
                    ParentId = table.Column<int>(nullable: false),
                    NavigationName = table.Column<string>(maxLength: 50, nullable: true),
                    RewriteName = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    LinkUrl = table.Column<string>(maxLength: 500, nullable: true),
                    Sort = table.Column<int>(nullable: false),
                    IsHomePage = table.Column<bool>(nullable: false),
                    IsShow = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Navigation", x => x.NavigationId);
                });

            migrationBuilder.CreateTable(
                name: "PageConfig",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdateBy = table.Column<string>(maxLength: 50, nullable: true),
                    NavigationId = table.Column<int>(nullable: false),
                    ControlJson = table.Column<string>(maxLength: 8000, nullable: true),
                    IsDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PictureGallery",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    ImageName = table.Column<string>(maxLength: 100, nullable: true),
                    Url = table.Column<string>(maxLength: 500, nullable: true),
                    ThumbnailUrl = table.Column<string>(maxLength: 500, nullable: true),
                    ExtensionName = table.Column<string>(maxLength: 10, nullable: true),
                    MD5 = table.Column<string>(maxLength: 100, nullable: true),
                    Size = table.Column<long>(nullable: false),
                    Width = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false),
                    CreateBy = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PictureGallery", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    RoleName = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "RoleMenu",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    RoleID = table.Column<int>(nullable: false),
                    Permission = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleMenu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteConfig",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CompanyName = table.Column<string>(maxLength: 100, nullable: true),
                    Keywords = table.Column<string>(maxLength: 250, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Logo = table.Column<string>(maxLength: 500, nullable: true),
                    Email = table.Column<string>(maxLength: 250, nullable: true),
                    Address = table.Column<string>(maxLength: 500, nullable: true),
                    Phone = table.Column<string>(maxLength: 50, nullable: true),
                    RecordNo = table.Column<string>(maxLength: 100, nullable: true),
                    Copyright = table.Column<string>(maxLength: 250, nullable: true),
                    Location_X = table.Column<double>(nullable: false),
                    Location_Y = table.Column<double>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    Telphone = table.Column<string>(maxLength: 20, nullable: true),
                    Password = table.Column<string>(maxLength: 250, nullable: true),
                    Openid = table.Column<string>(maxLength: 50, nullable: true),
                    Nickname = table.Column<string>(maxLength: 20, nullable: true),
                    Sex = table.Column<int>(nullable: false),
                    Language = table.Column<string>(maxLength: 10, nullable: true),
                    Province = table.Column<string>(maxLength: 10, nullable: true),
                    City = table.Column<string>(maxLength: 20, nullable: true),
                    Country = table.Column<string>(maxLength: 20, nullable: true),
                    Headimgurl = table.Column<string>(maxLength: 250, nullable: true),
                    Subscribe = table.Column<int>(nullable: false),
                    SubscribeTime = table.Column<long>(nullable: false),
                    Unionid = table.Column<string>(maxLength: 50, nullable: true),
                    Remark = table.Column<string>(maxLength: 250, nullable: true),
                    Groupid = table.Column<int>(nullable: false),
                    SubscribeScene = table.Column<string>(maxLength: 250, nullable: true),
                    TagidList = table.Column<string>(maxLength: 250, nullable: true),
                    QrScene = table.Column<int>(nullable: false),
                    QrSceneStr = table.Column<string>(maxLength: 250, nullable: true),
                    UnsubscribeTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User_Reply",
                columns: table => new
                {
                    ReplyId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    Openid = table.Column<string>(nullable: true),
                    MsgType = table.Column<string>(maxLength: 50, nullable: true),
                    MediaId = table.Column<string>(maxLength: 250, nullable: true),
                    Content = table.Column<string>(maxLength: 500, nullable: true),
                    PicUrl = table.Column<string>(maxLength: 250, nullable: true),
                    Format = table.Column<string>(maxLength: 100, nullable: true),
                    Recognition = table.Column<string>(maxLength: 100, nullable: true),
                    ThumbMediaId = table.Column<string>(maxLength: 250, nullable: true),
                    Location_X = table.Column<double>(nullable: false),
                    Location_Y = table.Column<double>(nullable: false),
                    Label = table.Column<string>(maxLength: 250, nullable: true),
                    Scale = table.Column<int>(nullable: false),
                    MsgId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Reply", x => x.ReplyId);
                });

            migrationBuilder.CreateTable(
                name: "Wx_KeyWordsReply",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    KeyName = table.Column<string>(maxLength: 60, nullable: true),
                    MsgType = table.Column<string>(maxLength: 20, nullable: true),
                    ReplyType = table.Column<int>(nullable: false),
                    MeId = table.Column<long>(nullable: false),
                    Content = table.Column<string>(maxLength: 250, nullable: true),
                    CoverUrl = table.Column<string>(maxLength: 250, nullable: true),
                    News_MediaId = table.Column<string>(maxLength: 250, nullable: true),
                    Image_MediaId = table.Column<string>(maxLength: 250, nullable: true),
                    Voice_MediaId = table.Column<string>(maxLength: 250, nullable: true),
                    Video_MediaId = table.Column<string>(maxLength: 250, nullable: true),
                    IsSubscribe = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wx_KeyWordsReply", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "Wx_Media",
                columns: table => new
                {
                    MeId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    FileName = table.Column<string>(maxLength: 100, nullable: true),
                    Introduction = table.Column<string>(maxLength: 500, nullable: true),
                    MediaId = table.Column<string>(maxLength: 250, nullable: true),
                    MediaType = table.Column<int>(nullable: false),
                    WxUrl = table.Column<string>(maxLength: 500, nullable: true),
                    Url = table.Column<string>(maxLength: 500, nullable: true),
                    CoverUrl = table.Column<string>(maxLength: 250, nullable: true),
                    ExtensionName = table.Column<string>(maxLength: 10, nullable: true),
                    Size = table.Column<long>(nullable: false),
                    Width = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false),
                    CreateBy = table.Column<string>(maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wx_Media", x => x.MeId);
                });

            migrationBuilder.CreateTable(
                name: "Wx_MenuViewLog",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    Url = table.Column<string>(maxLength: 250, nullable: true),
                    Openid = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wx_MenuViewLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wx_Keywords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    KeyId = table.Column<int>(nullable: false),
                    KeyType = table.Column<int>(nullable: false),
                    KeyWords = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wx_Keywords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wx_Keywords_Wx_KeyWordsReply_KeyId",
                        column: x => x.KeyId,
                        principalTable: "Wx_KeyWordsReply",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wx_Article",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdateBy = table.Column<string>(maxLength: 50, nullable: true),
                    MeId = table.Column<long>(nullable: false),
                    CoverUrl = table.Column<string>(maxLength: 500, nullable: true),
                    Sort = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 250, nullable: true),
                    ThumbMediaId = table.Column<string>(maxLength: 250, nullable: true),
                    Author = table.Column<string>(maxLength: 250, nullable: true),
                    Digest = table.Column<string>(maxLength: 250, nullable: true),
                    ShowCoverPic = table.Column<bool>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    ContentSourceUrl = table.Column<string>(maxLength: 250, nullable: true),
                    NeedOpenComment = table.Column<int>(nullable: false),
                    OnlyFansCanComment = table.Column<int>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wx_Article", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wx_Article_Wx_Media_MeId",
                        column: x => x.MeId,
                        principalTable: "Wx_Media",
                        principalColumn: "MeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wx_Article_MeId",
                table: "Wx_Article",
                column: "MeId");

            migrationBuilder.CreateIndex(
                name: "IX_Wx_Keywords_KeyId",
                table: "Wx_Keywords",
                column: "KeyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "AdminLogin");

            migrationBuilder.DropTable(
                name: "Album");

            migrationBuilder.DropTable(
                name: "Article");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Ct_Content");

            migrationBuilder.DropTable(
                name: "Ct_Image");

            migrationBuilder.DropTable(
                name: "Ct_Tags");

            migrationBuilder.DropTable(
                name: "Ct_Title");

            migrationBuilder.DropTable(
                name: "Job");

            migrationBuilder.DropTable(
                name: "Navigation");

            migrationBuilder.DropTable(
                name: "PageConfig");

            migrationBuilder.DropTable(
                name: "PictureGallery");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "RoleMenu");

            migrationBuilder.DropTable(
                name: "SiteConfig");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "User_Reply");

            migrationBuilder.DropTable(
                name: "Wx_Article");

            migrationBuilder.DropTable(
                name: "Wx_Keywords");

            migrationBuilder.DropTable(
                name: "Wx_MenuViewLog");

            migrationBuilder.DropTable(
                name: "Wx_Media");

            migrationBuilder.DropTable(
                name: "Wx_KeyWordsReply");
        }
    }
}
