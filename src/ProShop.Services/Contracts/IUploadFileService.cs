using Microsoft.AspNetCore.Http;

public interface IUploadFileService
{
    Task SaveFile(IFormFile file, string fileName,string oldFileName, params string[] destinationDirectoryNames);
    void DeleteFile(string fileName, params string[] destinationDirectoryNames);
}