using AutoMapper;
using TimeSlice.ApiService.Data;
using TimeSlice.ApiService.Models.Auth;
using TimeSlice.ApiService.Models.Timebox;

namespace TimeSlice.ApiService.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<ApplicationUser, ApplicationUserDto>().ReverseMap();
			CreateMap<TimeboxEntry, TimeboxEntryDto>().ReverseMap();
			CreateMap<TimeboxEntry, TimeboxCreateEntryDto>().ReverseMap();
			CreateMap<TimeboxEntryDto, TimeboxCreateEntryDto>().ReverseMap();
		}
    }
}
