
using Features.Core.Application.IServices;
using Microsoft.AspNetCore.Http;

namespace Features.Core.Application.Services
{
    public class StorageService : IStorageService
    {
        public string ProfileRootPath = "./Resources/Images/Profile";
        public void DeleteProfileImage(string username)
        {
            var imagePath = Path.Combine(ProfileRootPath, username);
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }

        public string UploadProfileImage(IFormFile ImageFile, string username)
        {

            var imagePath = Path.Combine(ProfileRootPath, username);
            using (var stream = File.Create(imagePath))
            {
                ImageFile.CopyTo(stream);
            }
            return imagePath;
        }

        public bool IsValidImage(IFormFile ImageFile)
        {
            if (ImageFile == null)
            {
                throw new ArgumentNullException(nameof(ImageFile));
            }
            if (ImageFile.Length == 0)
            {
                throw new ArgumentException("Image file is empty.", nameof(ImageFile));
            }
            if (ImageFile.Length > 2 * 1024 * 1024)
            {
                throw new ArgumentException("Image file is too large.", nameof(ImageFile));
            }
            if (!ImageFile.ContentType.StartsWith("image/"))
            {
                throw new ArgumentException("Image file is not an image.", nameof(ImageFile));
            }
            if (!ImageFile.FileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                                              && !ImageFile.FileName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)
                                                                                           && !ImageFile.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Image file is not a valid image.", nameof(ImageFile));
            }
            return true;
        }   
    }
}
