using AutoMapper;
using TimeSlice.ApiService.Models.Auth;
using TimeSlice.ApiService.Models.Timebox;

namespace TimeSlice.ApiService.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Data.ApplicationUser, ApplicationUserDto>().ReverseMap();
            CreateMap<Data.TimeboxEntry, TimeboxEntryDto>().ReverseMap();
        }
    }
}
