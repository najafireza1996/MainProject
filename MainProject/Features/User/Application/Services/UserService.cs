using Common.Exceptions;
using Common.Exceptions.Users;
using Features.Core.Application.IServices;
using Features.Core.Domain.IRepositories;
using Features.User.Application.IServices;
using Features.User.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.User.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageService _storageService;

        public UserService(IUnitOfWork unitOfWork, IStorageService storageService)
        {
            _unitOfWork = unitOfWork;
            _storageService = storageService;
        }

        public async Task ChangeUsername(string Username, string newUsername)
        {
            try
            {
                if (await _unitOfWork.UserRepo.UsernameAlreadyExists(newUsername))
                {
                    throw new UsernameAlreadyExistsException(newUsername);
                }
                await _unitOfWork.UserRepo.ChangeUsername(Username, newUsername);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }

        }

        public async Task<bool> IsSuspendedById(string userId)
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

        public async Task<ApplicationUser> SuspendByUsername(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    throw new ArgumentNullException(nameof(username));
                }
                var user = await _unitOfWork.UserRepo.SuspendByUsername(username);
                if (user == null)
                {
                    throw new UserNotFoundException(username);
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<ApplicationUser> UnSuspendByUsername(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    throw new ArgumentNullException(nameof(username));
                }
                var user = await _unitOfWork.UserRepo.UnSuspendByUsername(username);
                if (user == null)
                {
                    throw new UserNotFoundException(username);
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<bool> UsernameAlreadyExists(string newUsername)
        {
            try
            {
                if (string.IsNullOrEmpty(newUsername))
                {
                    throw new ArgumentNullException(nameof(newUsername));
                }
                return await _unitOfWork.UserRepo.UsernameAlreadyExists(newUsername);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<bool> RemovePicture(string userId)
        {
            try
            {
                var user = await _unitOfWork.UserRepo.GetOneAsync(
                        user => user.Id.Equals(userId),
                        tracked: true
                    );
                
                if (user == null)
                {
                    throw new UserNotFoundException(userId);
                }

                if (string.IsNullOrEmpty(user.ProfileImagePath))
                {
                    throw new PictureNotFoundException(userId);
                }
                _storageService.DeleteProfileImage(user.ProfileImagePath);

                user.ProfileImagePath = null;

                await _unitOfWork.UserRepo.UpdateAsync(user);

                return true;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
        
        public async Task<bool> ChangeProfilePhoto(string userId, IFormFile photoFile)
        {
            try
            {
                var user = await _unitOfWork.UserRepo.GetOneAsync(
                                           user => user.Id.Equals(userId),
                                                                  tracked: true
                                                                                     );
                
                if (user == null)
                {
                    throw new UserNotFoundException(userId);
                }

                if (!string.IsNullOrEmpty(user.ProfileImagePath))
                {
                    _storageService.DeleteProfileImage(user.UserName);
                }
                var photoUrl = _storageService.UploadProfileImage(photoFile, user.UserName);
                user.ProfileImagePath = photoUrl;

                await _unitOfWork.UserRepo.UpdateAsync(user);

                return true;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
        
        public async Task<ApplicationUser> GetUser(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    throw new ArgumentNullException(nameof(username));
                }
                var user = await _unitOfWork.UserRepo.GetOneAsync(
                                       user => user.UserName.Equals(username),
                                                          tracked: true);
                if (user == null)
                {
                    throw new UserNotFoundException(username);
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsers(bool suspended = false)
        {
            try
            {
                IEnumerable<ApplicationUser> users;
                if (suspended)
                {
                    users = await _unitOfWork.UserRepo.GetAllAsync(x => x.IsSuspended.Equals(true));
                }
                else
                {
                    users = await _unitOfWork.UserRepo.GetAllAsync();
                }
                return users;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
        
    }
}
