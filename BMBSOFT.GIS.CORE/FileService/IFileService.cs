using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

namespace BMBSOFT.GIS.CORE.FileService
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(string targetFolder, IFormFile file);
        Task<string> SaveFileAsyncCurrentName(string targetFolder, IFormFile file);
        Task<string> SaveShapeFileAsyncCurrentName(string targetFolder, IFormFile file);
        void RemoveFile(string urlFile);
        string CreateFolder(string targetFolder, string folderName);
        //bool MoveFile(long currentFilePathId, string targetFolder, string fileName);
        //bool MoveFolderAsync(string currentFolder, string targetLocation);
        bool RenameFolder(string pathFolderOld, string pathFolderNew);
        bool RenameFile(string pathFolderOld, string pathFolderNew);
        bool DeleteFolder(string path);
        string ConvertToBase64(string path);
    }
}
