using AutoMapper;
using Features.BackOffice.Domain.ApiModels.ResponseDTO;
using Features.User.Domain.ApiModels.ResponseDTO;
using Features.User.Domain.Entities;

namespace MainProject.ProfileMappers
{
    public class ApiProfile : Profile
    {
        public ApiProfile()
        {

            CreateMap<ApplicationUser, AppUserResponse>()
                .ForMember(
                     dest => dest.FullName,
                     opt => opt.MapFrom(
                         source => source.FirstName + " " + source.LastName
                         )
                );
            CreateMap<ApplicationUser, AppUserAdminResponse>()
                .IncludeBase<ApplicationUser, AppUserResponse>();

        }
    }
}
