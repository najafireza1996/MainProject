using AutoMapper;
using Common.Constants;
using Common.Exceptions.Users;
using Features.BackOffice.Application.IServices;
using Features.BackOffice.Domain.ApiModels.ResponseDTO;
using Features.User.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace MainProject.Features.BackOffice.Presentation
{
    [Authorize(Roles = Roles.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    [Tags("AdminController")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IMapper _mapper;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IAdminService adminService, IMapper mapper, ILogger<AdminController> logger)
        {
            _adminService = adminService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Suspend a user by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        /// <exception cref="UserNotFoundException"></exception>
        [HttpGet("suspend-user/{username}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SuspendUser([FromRoute] string username)
        {
            try
            {
                var user = await _adminService.SuspendUser(username);

                _logger.LogInformation("User with usrname : {} is now suspended.", username);

                return Ok(
                    _mapper.Map<AppUserAdminResponse>(user)
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "{Error} Executing {Action} with parameters {Parameters}.",
                        ex.Message, nameof(SuspendUser), username
                    );

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Unsuspend a user by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        /// <exception cref="UserNotFoundException"></exception>
        [HttpGet("unsuspend-user/{username}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UnSuspendUser([FromRoute] string username)
        {
            try
            {
                var user = await _adminService.UnSuspendUser(username);

                _logger.LogInformation("User with usrname : {} is now unsuspended.", username);

                return Ok(_mapper.Map<List<ApplicationUser>>(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "{Error} Executing {Action} with parameters {Parameters}.",
                        ex.Message, nameof(UnSuspendUser), username
                    );

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// remove a user by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpDelete("remove-user/{username}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveUser([FromRoute] string username)
        {
            try
            {
                var user = await _adminService.RemoveUser(username);

                _logger.LogInformation("User with usrname : {} is now removed.", username);

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                                   "{Error} Executing {Action} with parameters {Parameters}.",
                ex.Message, nameof(RemoveUser), username);

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}