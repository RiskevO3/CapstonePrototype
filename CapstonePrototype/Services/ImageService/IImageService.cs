using SealBackend.Dto;

namespace CapstonePrototype.services.ImageService;
public interface IImageService
{
    public Task<ServiceResponse<bool>> UploadImage(string base64Image,string folder,int maxImageSizeInMB=3);
    public ServiceResponse<bool> IsBase64AndDirectoryValid(string base64Image, string folder, int sizeInMB);
}