using Microsoft.AspNetCore.Http;

public interface IUploadFileService
{
    Task SaveFile(IFormFile file, string fileName, params string[] destinationDirectoryNames);
}