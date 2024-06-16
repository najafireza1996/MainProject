using System;
using Features.User.Domain.Entities;

namespace MainProject.Features.Core.Application.IServices
{
    public interface ISmsService
    {
        public Task SendAsync(ApplicationUser user);
    }
}

