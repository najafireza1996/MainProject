using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.BackOffice.Application.IServices
{
    public interface IAdminService
    {
        public Task<bool> SuspendUser(string userId);
        public Task<bool> UnSuspendUser(string userId);
        public Task<bool> RemoveUser(string userId);
    }
}
