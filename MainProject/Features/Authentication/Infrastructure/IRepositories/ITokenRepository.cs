using Features.Authentication.Domain.Entities;
using Features.Core.Domain.IRepositories;

namespace Features.Authentication.Infrastructure.IRepositories
{
    public interface ITokenRepository : IRepository<RefreshToken>
    {
    }
}