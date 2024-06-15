

using Features.Authentication.Infrastructure.IRepositories;
using Features.Authentication.Infrastructure.Repositories;
using Features.Core.Domain.IRepositories;
using Features.Core.Infrastructure.DataAccess;
using Features.User.Domain.IRepositories;
using Features.User.Infrastructure.Repositories;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            UserRepo = new UserRepository(_dbContext);
            TokenRepo = new TokenRepository(_dbContext);

        }
        public IUserRepository UserRepo { get; private set; }
        public ITokenRepository TokenRepo { get; }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
