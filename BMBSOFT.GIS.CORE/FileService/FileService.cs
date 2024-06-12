using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace BASE.CORE.FileService
{
    public class FileService : IFileService
    {        
        private readonly IHostEnvironment _env;

        public FileService(IHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveFileAsync(string targetFolder, IFormFile file)
        {
            var uploadPath = Path.Combine(_env.ContentRootPath, targetFolder);

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var fileName = $"{Guid.NewGuid()}{file.FileName}";

            var fullPath = Path.Combine(uploadPath, fileName);

            var filePath = Path.Combine(targetFolder, fileName); //$"{targetFolder}/{Path.Combine(fileName)}";

            await using var fileStream = new FileStream(fullPath, FileMode.Create);

            await file.CopyToAsync(fileStream);

            return filePath;
        }

        public void RemoveFile(string urlFile)
        {
            var fullPath = Path.Combine(_env.ContentRootPath, urlFile ?? string.Empty); //$"{_env.ContentRootPath}{urlFile}";
            if (File.Exists(fullPath)) File.Delete(fullPath);
        }
       

        public string CreateFolder(string targetFolder, string folderName)
        {
            //var rootPath = $"{_env.ContentRootPath}{targetFolder}/{folderName}";
            var rootPath = Path.Combine(_env.ContentRootPath, targetFolder, folderName);
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }
            return Path.Combine(targetFolder, folderName); //$"{targetFolder}/{folderName}";
        }

        public bool RenameFolder(string pathFolderOld, string pathFolderNew)
        {
            var rootPathOld = Path.Combine(_env.ContentRootPath, pathFolderOld); //$"{_env.ContentRootPath}{pathFolderOld}";
            if (!Directory.Exists(rootPathOld))
                return false;
            var rootPathNew = Path.Combine(_env.ContentRootPath, pathFolderNew); //$"{_env.ContentRootPath}{pathFolderNew}";
            Directory.Move(rootPathOld, rootPathNew);
            return true;
        }

        public bool RenameFile(string pathFolderOld, string pathFolderNew)
        {
            var rootPathOld = Path.Combine(_env.ContentRootPath, pathFolderOld);
            if (!File.Exists(rootPathOld))
                return false;
            var rootPathNew = Path.Combine(_env.ContentRootPath, pathFolderNew); 
            File.Move(rootPathOld, rootPathNew);
            return true;
        }

        public bool DeleteFolder(string path)
        {
            var rootPath = Path.Combine(_env.ContentRootPath, path); //$"{_env.ContentRootPath}/{path}";
            if (!Directory.Exists(rootPath)) return false;
            Directory.Delete(rootPath, true);
            return true;
        }

        public string ConvertToBase64(string path)
        {
            var contentPath = $"{_env.ContentRootPath}/{path}";
            return File.Exists(contentPath) ? Convert.ToBase64String(File.ReadAllBytes(contentPath)) : null;
        }

        public async Task<string> SaveFileAsyncCurrentName(string targetFolder, IFormFile file)
        {
            var uploadPath = Path.Combine(_env.ContentRootPath, targetFolder);

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            var fileName = file.FileName;
            var fullPath = Path.Combine(uploadPath, fileName); //$@"{uploadPath}\{fileName}";
            var filePath = Path.Combine(targetFolder, fileName); //$"{targetFolder}/{Path.Combine(fileName)}";
            await using var fileStream = File.Create(fullPath);
            await file.CopyToAsync(fileStream);
            return filePath;
        }

        public async Task<string> SaveShapeFileAsyncCurrentName(string targetFolder, IFormFile file)
        {
            var uploadPath = _env.ContentRootPath + targetFolder;

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            var fileName = Path.GetFileName($@"\{file.FileName}");
            var fullPath = $@"{uploadPath}\{fileName}";
            var filePath = $"{targetFolder}/{Path.Combine(fileName)}";
            using (var fileStream = File.Create(fullPath))
            {
                await file.CopyToAsync(fileStream);
            };

            return filePath;
        }
    }
}
