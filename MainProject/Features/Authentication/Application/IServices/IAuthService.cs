

using Features.Authentication.Domain.ApiModels.Request;
using Features.Authentication.Domain.ApiModels.Response;

namespace Features.Authentication.Application.IServices
{
    public interface IAuthService
    {
        Task<RegisterModelResponse> RegisterAsync(RegistrationModelRequest model);
        Task<LoginModelResponse> LoginAsync(LoginModelRequest model);
        Task<LoginModelResponse> VerifyAndGenerateToken(TokenRequest tokenRequest);
        Task RevokeTokenAsync(string userId);
    }
}