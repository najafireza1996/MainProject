
using Features.User.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Features.User.Application.IServices
{
    public interface IUserService
    {
        public Task ChangeUsername(string Username, string newUsername);

        public Task<ApplicationUser> SuspendByUsername(string username);

        public Task<ApplicationUser> UnSuspendByUsername(string username);

        public Task<bool> UsernameAlreadyExists(string newUsername);

        public Task<bool> IsSuspendedById(string userId);

        public Task<bool> RemovePicture(string userId);

        public Task<bool> ChangeProfilePhoto(string userId, IFormFile photoFile);

        public Task<ApplicationUser> GetUser(string username);

        public Task<IEnumerable<ApplicationUser>> GetUsers([FromQuery] bool suspended = false);

    }
}
