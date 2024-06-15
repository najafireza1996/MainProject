using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Constants;
using Common.Helpers;
using Microsoft.AspNetCore.Http.HttpResults;
using Common.Exceptions.Users;
using Common.Exceptions;
using Features.Authentication.Application.IServices;
using Features.Authentication.Domain.ApiModels.Response;
using Features.Authentication.Domain.ApiModels.Request;
using Features.User.Domain.Entities;
using Features.Authentication.Domain.Entities;
using Features.Core.Domain.IRepositories;

namespace Features.Authentication.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly TokenValidationParameters _tokenValidationParams;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly JWTOptions _jwtConfig;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            TokenValidationParameters tokenValidationParams,
            IOptions<JWTOptions> jwt,
            IUnitOfWork unitOfWork
            )
        {
            _userManager = userManager;
            _jwtConfig = jwt.Value;
            _unitOfWork = unitOfWork;
            _tokenValidationParams = tokenValidationParams;
        }


        public async Task<RegisterModelResponse> RegisterAsync(RegistrationModelRequest model)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    throw new CustomException("Email already exists!");
                }

                var newUser = new ApplicationUser()
                {
                    Email = model.Email,
                    UserName = model.Username,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                };

                var isCreated = await _userManager.CreateAsync(newUser, model.Password);
                if (!isCreated.Succeeded)
                {
                    var errors = string.Join(", ", isCreated.Errors.Select(x => x.Description));
                    throw new CustomException(errors);
                }
                var isAddedToRole = await _userManager.AddToRoleAsync(newUser, Roles.User);
                if (!isAddedToRole.Succeeded)
                {
                    var errors = string.Join(", ", isAddedToRole.Errors.Select(x => x.Description));
                    throw new CustomException(errors);
                }

                return new RegisterModelResponse()
                {
                    Email = newUser.Email,
                    UserName = newUser.UserName
                };
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }

        }
        public async Task<LoginModelResponse> LoginAsync(LoginModelRequest model)
        {


            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return new LoginModelResponse()
                {
                    IsAuthenticated = false,
                    ErrorMessage = "Email or Password is incorrect!"
                };
            }

            return await CreateJwtToken(user);
        }


        public async Task RevokeTokenAsync(string userId)
        {
            var userToken = await _unitOfWork.TokenRepo.GetOneAsync(x => x.UserId == userId);
            if (userToken != null)
            {
                await _unitOfWork.TokenRepo.RemoveAsync(userToken);
                await _unitOfWork.SaveAsync();
            }
        }


        private async Task<LoginModelResponse> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("username", user.UserName),
                new Claim("uid", user.Id.ToString())
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var accessToken = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(2),
                signingCredentials: signingCredentials
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);

            var refreshToken = new RefreshToken()
            {
                JwtId = accessToken.Id,
                IsUsed = false,
                IsRevorked = false,
                UserId = user.Id.ToString(),
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                Token = RandomString(35) + Guid.NewGuid()
            };

            await _unitOfWork.TokenRepo.AddAsync(refreshToken);
            await _unitOfWork.SaveAsync();

            return new LoginModelResponse()
            {
                Token = tokenString,
                IsAuthenticated = true,
                RefreshToken = refreshToken.Token,
                ExpiresOn = accessToken.ValidTo,
            };
        }


        // This function is inspired by : https://github.com/mohamadlawand087/v8-refreshtokenswithJWT/blob/cc8e7f45285b1067e1c6559af65d777d450b9679/TodoApp/Controllers/AuthManagementController.cs#L202
        public async Task<LoginModelResponse> VerifyAndGenerateToken(TokenRequest tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // Validation 1 - Validation JWT token format
                _tokenValidationParams.ValidateLifetime = false;
                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.AccessToken, _tokenValidationParams, out var validatedToken);
                _tokenValidationParams.ValidateLifetime = true;

                // Validation 2 - Validate encryption alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (result == false) return null;
                }

                // Validation 3 - validate expiry date
                var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

                if (expiryDate > DateTime.UtcNow)
                {
                    return new LoginModelResponse()
                    {
                        IsAuthenticated = false,
                        ErrorMessage = "Token has not yet expired"
                    };
                }

                // validation 4 - validate existence of the token
                var storedToken = await _unitOfWork.TokenRepo.GetOneAsync(x => x.Token == tokenRequest.RefreshToken);

                if (storedToken == null)
                {
                    return new LoginModelResponse()
                    {
                        IsAuthenticated = false,
                        ErrorMessage = "Token does not exist"
                    };
                }

                // Validation 6 - validate if revoked
                if (storedToken.IsRevorked)
                {
                    return new LoginModelResponse()
                    {
                        IsAuthenticated = false,
                        ErrorMessage = "Token has been revoked"
                    };
                }

                // Validation 7 - validate the id
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (storedToken.JwtId != jti)
                {
                    return new LoginModelResponse()
                    {
                        IsAuthenticated = false,
                        ErrorMessage = "Token doesn't match"
                    };
                }

                // Validation 8 - validate stored token expiry date
                if (storedToken.ExpiryDate < DateTime.UtcNow)
                {
                    return new LoginModelResponse()
                    {
                        IsAuthenticated = false,
                        ErrorMessage = "Refresh token has expired"
                    };
                }

                // update current token 
                await _unitOfWork.TokenRepo.RemoveAsync(storedToken);
                await _unitOfWork.SaveAsync();

                // Generate a new token
                var user = await _userManager.FindByIdAsync(storedToken.UserId);
                return await CreateJwtToken(user);
            }
            catch (Exception ex)
            {
                return new LoginModelResponse()
                {
                    IsAuthenticated = false,
                    ErrorMessage = ex.Message
                };

            }
        }


        // https://stackoverflow.com/a/250400
        private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToUniversalTime();

            return dateTime;
        }


        private string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable
                .Repeat(chars, length)
                .Select(x => x[random.Next(x.Length)])
                .ToArray()
                );
        }

    }
}