using Microsoft.AspNetCore.Http;

public static class FileHelpers
{

    public static bool IsFileUploaded(this IFormFile file)
    {
        return file is {Length: > 0};
    }

}