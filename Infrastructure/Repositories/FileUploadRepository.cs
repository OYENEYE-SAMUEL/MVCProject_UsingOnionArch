using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Configuration;
using Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Infrastructure.Repositories
{
    public class FileUploadRepository : IFileUploadRepository
    {
        private readonly FileConfiguration _config;
        public FileUploadRepository(IOptions<FileConfiguration> config)
        {
            _config = config.Value;
        }

        public string UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Kindly upload a valid image.");
            }
            var appUploadPath = _config.Path;
            if (!Directory.Exists(appUploadPath))
            {
                Directory.CreateDirectory(appUploadPath);
            }
            var appFileName = file.ContentType.Split('/');
            var fileName = $"IMG{appFileName[0]}{Guid.NewGuid().ToString().Substring(6, 5)}.{appFileName[1]}";
            var fullPath = Path.Combine(appUploadPath, fileName);

            try
            {
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (optional: log the error, rethrow, etc.)
                throw new Exception("An Error Occur when trying to upload image.", ex);
            }

            // Return the name of the uploaded file
            return fileName;
        
        }
    }
}
