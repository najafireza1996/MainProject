using Features.Authentication.Domain.Entities;
using Features.Authentication.Infrastructure.IRepositories;
using Features.Core.Infrastructure.DataAccess;
using Features.Core.Infrastructure.Repositories;

namespace Features.Authentication.Infrastructure.Repositories
{
    public class TokenRepository : Repository<RefreshToken>, ITokenRepository
    {
        public TokenRepository(AppDbContext appContext) : base(appContext)
        {
        }
    }
}
