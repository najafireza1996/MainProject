using Microsoft.AspNetCore.Http;

namespace Features.Core.Application.IServices
{
    public interface IStorageService
    {
        public string UploadProfileImage(IFormFile ImageFile, string username);
        public void DeleteProfileImage(string username);
    }
}
