using Features.User.Domain.ApiModels.ResponseDTO;

namespace Features.BackOffice.Domain.ApiModels.ResponseDTO
{
    public class AppUserAdminResponse : AppUserResponse
    {
        public bool IsSuspended { get; set; }
    }
}
