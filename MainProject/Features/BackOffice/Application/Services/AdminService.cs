using Common.Exceptions;
using Common.Exceptions.Users;
using Features.BackOffice.Application.IServices;
using Features.Core.Domain.IRepositories;

namespace Features.BackOffice.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> SuspendUser(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new ArgumentNullException(nameof(userId));
                }
                var user = await _unitOfWork.UserRepo.SuspendByUsername(userId);
                if (user == null)
                {
                    throw new UserNotFoundException(userId);
                }
                return user.IsSuspended;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<bool> UnSuspendUser(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new ArgumentNullException(nameof(userId));
                }
                var user = await _unitOfWork.UserRepo.UnSuspendByUsername(userId);
                if (user == null)
                {
                    throw new UserNotFoundException(userId);
                }
                return user.IsSuspended;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<bool> RemoveUser(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new ArgumentNullException(nameof(userId));
                }
                var isDeleted = await _unitOfWork.UserRepo.RemoveByUsername(userId);
                return isDeleted;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

    }
}
