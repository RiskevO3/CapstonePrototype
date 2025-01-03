namespace CapstonePrototype.Services.FileUploadService;
public interface IFileUploadService
{
    public Task<string> UploadPdfAsync(string fileName, string base64Content);
    public Task<string> UploadImageAsync(string fileName, string base64Content);
}