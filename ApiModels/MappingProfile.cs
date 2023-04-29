using AutoMapper;

// https://code-maze.com/automapper-net-core-custom-projections/
namespace SummryApi.ApiModels
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Entities.Store, Store.StoreGet>(); // this line must be before stores != null check 

            // this means query params have no association to this method, which may be fine?
            CreateMap<Entities.Platform, Platform.PlatformGet>()
                // .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Stores, opt => opt.Condition(src => src.Stores != null));
        }

    }
}
