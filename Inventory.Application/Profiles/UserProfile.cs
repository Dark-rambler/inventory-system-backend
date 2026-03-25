using AutoMapper;
using Inventory.Application.DataTransferObjects.UserDto;
using Inventory.Domain.Entities;

namespace Inventory.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRequest, User>();
            CreateMap<User, UserResponse>();
        }
    }
}
