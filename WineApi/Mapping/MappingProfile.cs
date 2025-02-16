using AutoMapper;
using WineApi.Database.Entities;
using WineApi.Models;

namespace WineApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Wine, WineDto>();
            CreateMap<WineDto, Wine>();
        }
    }
}
