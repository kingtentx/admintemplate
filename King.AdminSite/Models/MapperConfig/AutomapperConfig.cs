using AutoMapper;
using King.Data;

namespace King.AdminSite.Models.MapperConfig
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<SiteConfig, SiteConfigModel>().ReverseMap();
            CreateMap<Article, ArticleModel>().ReverseMap();
            CreateMap<Album, AlbumModel>().ReverseMap();         
            CreateMap<User, WxUserModel>().ReverseMap();

            CreateMap<PictureGallery, PictureGalleryModel>().ReverseMap();
            CreateMap<Attachments, AttachmentsModel>().ReverseMap();
            CreateMap<Wx_KeyWordsReply, WxKeyWordsReplyModel>().ReverseMap();
            CreateMap<Wx_Keywords, WxKeyWordsModel>().ReverseMap();
            CreateMap<Wx_Media, WxMediaModel>().ReverseMap();
            CreateMap<Wx_Article, WxArticleModel>().ReverseMap();

            CreateMap<Navigation, NavigationModel>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(c => c.NavigationId))
                .ForMember(dest => dest.Pid, opts => opts.MapFrom(c => c.ParentId))
                .ReverseMap();

            CreateMap<Tags, TagsModel>().ReverseMap();

        }
    }
}
