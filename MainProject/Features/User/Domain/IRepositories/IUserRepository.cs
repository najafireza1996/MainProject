
using Features.Core.Domain.IRepositories;
using Features.User.Domain.Entities;

namespace Features.User.Domain.IRepositories
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser> UnSuspendByUsername(string username);
        Task<ApplicationUser> SuspendByUsername(string username);
        Task<bool> UsernameAlreadyExists(string newUsername);
        Task ChangeUsername(string Username, string newUsername);
        Task<bool> IsSuspendedById(string userId);
        Task<bool> RemoveByUsername(string username);
    }
}
