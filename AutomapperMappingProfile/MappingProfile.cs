using AutoMapper;
using PCM.ElasticSearchModels;
using PCM.Models;

namespace PCM.AutomapperMappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Item, EsItem>();

            CreateMap<Comment, EsComment>();
        }
    }
}
