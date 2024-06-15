

using Features.User.Domain.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Domain.Filters
{
    public class SuspenededActionFilter : IAsyncActionFilter
    {
        private readonly IUserRepository _appUserRepository;
        public SuspenededActionFilter(IUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Result != null) return;
            if (context.HttpContext.Request.Method != HttpMethod.Get.ToString())
            {
                var userId = context.HttpContext.User.Claims.Where(x => x.Type == "uid").FirstOrDefault()?.Value;
                if (userId != null)
                {
                    if (await _appUserRepository.IsSuspendedById(userId))
                    {
                        context.Result =
                            new ObjectResult(context.ModelState)
                            {
                                Value = "User is currently suspeneded.",
                                StatusCode = StatusCodes.Status403Forbidden,

                            };
                        return;
                    }
                }
            }
            await next();
        }
    }
}
