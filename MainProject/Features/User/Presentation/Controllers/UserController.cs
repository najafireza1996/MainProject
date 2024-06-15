using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using System.Net.Mime;
using Common.Exceptions.Users;
using ISession = Common.Managers.SessionManager.ISession;
using Common.Validators;
using Common.Constants;
using Features.User.Domain.ApiModels.RequestDTO;
using Features.User.Domain.ApiModels.ResponseDTO;
using Features.User.Application.IServices;
using Features.Core.Application.IServices;

namespace MainProject.Features.User.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [Tags("UserController")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;
        private readonly ISession _session;

        public UserController(IUserService userService, IStorageService storageService, IMapper mapper, ISession session)
        {
            _userService = userService;
            _storageService = storageService;
            _mapper = mapper;
            _session = session;
        }



        /// <summary>
        /// Get all users
        /// </summary>
        /// <param name="suspended"></param>
        /// <returns></returns>
        [HttpGet("users-list")]
        [Authorize(Roles = Roles.Admin)]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllUsers([FromQuery] bool suspended = false)
        {

            try
            {
                var users = await _userService.GetUsers(suspended);
                return Ok(_mapper.Map<IEnumerable<AppUserResponse>>(users));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("GetAllUsers", ex.Message);
                return BadRequest(ModelState);
            }
        }


        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        /// <exception cref="UserNotFoundException"></exception>
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUser([FromQuery] string? username)
        {
            try
            {
                if (username == null)
                {
                    username = User.Claims.Where(x => x.Type == "username").FirstOrDefault()?.Value;
                }
                var user = await _userService.GetUser(username);

                return Ok(
                    _mapper.Map<AppUserResponse>(user)
                    );
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("GetUser", ex.Message);
                return BadRequest(ModelState);
            }
        }



        /// <summary>
        /// Get user's profile image
        /// </summary>
        /// <param name="imageFile"></param>
        /// <returns></returns>
        [HttpPost("change-profile-photo")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeProfilePhoto(IFormFile imageFile)
        {
            try
            {
                if (!FileValidator.IsValidImage(imageFile))
                {
                    throw new ArgumentException("Image file is not valid.", nameof(imageFile));
                }
                var user = _session.UserId;
                await _userService.ChangeProfilePhoto(user, imageFile);

                return Ok(
                    _mapper.Map<AppUserResponse>(user)
                    );
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("change-profile-photo", ex.Message);
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Remove user's profile image
        /// </summary>
        /// <returns></returns>
        [HttpDelete("remove-profile-photo")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemovePicture()
        {
            try
            {
                var username = _session.UserId;
                await _userService.RemovePicture(username);
                return Ok("Profile photo removed successfully.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("remove-profile-photo", ex.Message);
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Change user's username
        /// </summary>
        /// <param name="newUsername"></param>
        /// <returns></returns>
        /// <exception cref="UsernameAlreadyExistsException"></exception>
        /// <exception cref="NotValidUsernameException"></exception>
        [HttpPost("change-username")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeUsername([FromBody] ChangeUsernameRequest changeUsernameRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!UsernameValidator.IsValidUsername(changeUsernameRequest.NewUsername))
                {
                    throw new NotValidUsernameException(changeUsernameRequest.NewUsername);
                }

                var username = _session.Username;
                if (username != changeUsernameRequest.CurrentUsername)
                {
                    throw new NotValidUsernameException(changeUsernameRequest.CurrentUsername);
                }

                await _userService.ChangeUsername(username, changeUsernameRequest.NewUsername);

                return Ok(
                    "Username changed successfully."
                    );
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("change-username", ex.Message);
            }
            return BadRequest(ModelState);
        }
    }
}



