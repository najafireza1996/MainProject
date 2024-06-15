
using Features.User.Domain.Entities;
using Features.Core.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Features.Core.Infrastructure.DataAccess;
using Features.User.Domain.IRepositories;

namespace Features.User.Infrastructure.Repositories
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext appContext) : base(appContext)
        {
            _dbContext = appContext;
        }

        public async Task ChangeUsername(string Username, string newUsername)
        {
            var user = await _dbContext.Users
                .AsTracking()
                .FirstOrDefaultAsync(x => x.UserName.Equals(Username));
            user.UserName = newUsername;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ApplicationUser> SuspendByUsername(string username)
        {
            var user = await _dbContext.Users
                .AsTracking()
                .FirstOrDefaultAsync(u => u.UserName.Equals(username));
            if (user != null)
            {
                user.IsSuspended = true;
                await _dbContext.SaveChangesAsync();
            }

            return user;
        }

        public async Task<ApplicationUser> UnSuspendByUsername(string username)
        {
            var user = await _dbContext.Users
                .AsTracking()
                .FirstOrDefaultAsync(u => u.UserName.Equals(username));
            if (user != null)
            {
                user.IsSuspended = true;
                await _dbContext.SaveChangesAsync();
            }

            return user;
        }

        public async Task<bool> UsernameAlreadyExists(string newUsername)
        {
            return await _dbContext.Users
                .AnyAsync(x => x.UserName.Equals(newUsername));
        }

        public async Task<bool> IsSuspendedById(string userId)
        {
            return (await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.Equals(userId)))
                .IsSuspended;
        }

        public async Task<bool> RemoveByUsername (string username)
        {
            var user = await _dbContext.Users
                .AsTracking()
                .FirstOrDefaultAsync(u => u.UserName.Equals(username));
            if (user != null)
            {
                user.IsDeleted = true;
                await _dbContext.SaveChangesAsync();
            }

            return user.IsDeleted;
        }
    }
}
