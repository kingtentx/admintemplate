using AutoMapper;
using King.Data;

namespace King.Api.Models.MapperConfig
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {            
            CreateMap<Article, ArticleDto>().ReverseMap();
            CreateMap<Category, CategoryDto>()
                 .ForMember(dest => dest.Id, opts => opts.MapFrom(c => c.CategoryId))
                 .ForMember(dest => dest.Pid, opts => opts.MapFrom(c => c.ParentId))
                 .ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
