using Features.Authentication.Infrastructure.IRepositories;
using Features.User.Domain.IRepositories;

namespace Features.Core.Domain.IRepositories
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepo { get; }
        ITokenRepository TokenRepo { get; }
        Task SaveAsync();
    }
}
