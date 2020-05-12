using AutoMapper;
using MiniMdb.Backend.Models;
using MiniMdb.Backend.ViewModels;

namespace MiniMdb.Backend.Mappings
{
    public class VmMappingProfile : Profile
    {
        public VmMappingProfile()
        {
            CreateMap<MediaTitle, MediaTitleVm>();
            CreateMap<MediaTitleVm, MediaTitle>();
            CreateMap<MediaTitleVm, Movie>();
            CreateMap<MediaTitleVm, Series>();
        }
    }
}
