using AutoMapper;
using notes_firebase.DTOs;
using notes_firebase.Models;

namespace notes_firebase.Configurations
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile() 
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<UserAuthDTO, User>();
            CreateMap<User, UserAuthDTO>();
        }
    }
}
