using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Validators
{
    public class FileValidator
    {
        public static bool IsValidImage(IFormFile ImageFile)
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
            if (ImageFile.FileName.Length > 120)
            {
                throw new ArgumentException("Image file name is too long.", nameof(ImageFile));
            }
            if (ImageFile.FileName.Contains(".."))
            {
                throw new ArgumentException("Image file name is not valid.", nameof(ImageFile));
            }
            if (ImageFile.FileName.Contains("/"))
            {
                throw new ArgumentException("Image file name is not valid.", nameof(ImageFile));
            }
            if (ImageFile.FileName.Contains("\\"))
            {
                throw new ArgumentException("Image file name is not valid.", nameof(ImageFile));
            }
            if (ImageFile.FileName.Contains(":"))
            {
                throw new ArgumentException("Image file name is not valid.", nameof(ImageFile));
            }
            if (ImageFile.FileName.Contains("*"))
            {
                throw new ArgumentException("Image file name is not valid.", nameof(ImageFile));
            }
            if (ImageFile.FileName.Contains("?"))
            {
                throw new ArgumentException("Image file name is not valid.", nameof(ImageFile));
            }
            if (ImageFile.FileName.Contains("\""))
            {
                throw new ArgumentException("Image file name is not valid.", nameof(ImageFile));
            }
            if (ImageFile.FileName.Contains("<"))
            {
                throw new ArgumentException("Image file name is not valid.", nameof(ImageFile));
            }
            if (ImageFile.FileName.Contains(">"))
            {
                throw new ArgumentException("Image file name is not valid.", nameof(ImageFile));
            }
            if (ImageFile.FileName.Contains("|"))
            {
                throw new ArgumentException("Image file name is not valid.", nameof(ImageFile));
            }
            if (ImageFile.FileName.Contains(";"))
            {
                throw new ArgumentException("Image file name is not valid.", nameof(ImageFile));
            }
            if (ImageFile.FileName.Contains(","))
            {
                throw new ArgumentException("Image file name is not valid.", nameof(ImageFile));
            }
            if (ImageFile.FileName.Contains(" "))
            {
                throw new ArgumentException("Image file name is not valid.", nameof(ImageFile));
            }
            if (ImageFile.FileName.Contains("="))
            {
                throw new ArgumentException("Image file name is not valid.", nameof(ImageFile));
            }
            if (ImageFile.FileName.Contains("["))
            {
                throw new ArgumentException("Image file name is not valid.", nameof(ImageFile));
            }
            if (ImageFile.FileName.Contains("]"))
            {
                throw new ArgumentException("Image file name is not valid.", nameof(ImageFile));
            }
            return true;
        }
    }
}
