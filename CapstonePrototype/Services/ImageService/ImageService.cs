using System.Text.RegularExpressions;
using SealBackend.Dto;

namespace CapstonePrototype.services.ImageService;
public class ImageService(IWebHostEnvironment environment) : IImageService
{
    private readonly IWebHostEnvironment environment = environment;
    public async Task<ServiceResponse<bool>> UploadImage(string base64Image,string folder,int maxImageSizeInMB=3)
    {
        try
        {
            string Bae64ImageWithoutHeader = base64Image[(base64Image.IndexOf(",") + 1)..];
            string uploadsFolder = Path.Combine(environment.WebRootPath, folder);
            var isDirectoryAndBase64Valid = IsBase64AndDirectoryValid(base64Image, folder, 3);
            if (!isDirectoryAndBase64Valid.Data) return isDirectoryAndBase64Valid;
            byte[] imageBytes = Convert.FromBase64String(Bae64ImageWithoutHeader);
            string fileName = Guid.NewGuid().ToString() + ".png";
            string filePath = Path.Combine(uploadsFolder, fileName);
            if (File.Exists(filePath)) File.Delete(filePath);
            await File.WriteAllBytesAsync(filePath, imageBytes);
            return new ServiceResponse<bool>{Data = true,Message=$"{folder}/{fileName}"};
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error from UploadItemMallImage: {e.Message}");
            return new ServiceResponse<bool>{Data = false, Message = "Terjadi kesalahan saat membuat image"};
        }
    }
    public ServiceResponse<bool> IsBase64AndDirectoryValid(string base64Image, string folder, int sizeInMB)
    {
        var isBase64Image = IsBase64ImageString(base64Image);
        if (!isBase64Image.Data) return isBase64Image;
        var isBase64ImageSizeValid = IsBase64ImageSizeValid(base64Image, sizeInMB);
        if (!isBase64ImageSizeValid.Data) return isBase64ImageSizeValid;
        var isDirectoryValid = CheckOrCreateDirectory(Path.Combine(environment.WebRootPath, folder));
        if (!isDirectoryValid.Data) return isDirectoryValid;
        return new ServiceResponse<bool>{Data = true};
    }
    private static ServiceResponse<bool> IsBase64ImageString(string base64Image)
    {
        if (string.IsNullOrEmpty(base64Image))
        {
            return new ServiceResponse<bool>{Data = false, Message = "Base64 string is empty"};
        }

        // Regex pattern to match base64 image strings
        string pattern = @"^data:image\/(jpeg|jpg|png|gif|bmp|webp);base64,[A-Za-z0-9+/]+={0,2}$";

        // Validate the base64 string using regex
        var regRes = Regex.IsMatch(base64Image, pattern, RegexOptions.IgnoreCase);
        if(!regRes) return new ServiceResponse<bool>{Data = false, Message = "Base64 string tidak valid"};
        return new ServiceResponse<bool>{Data = true};
    }
    private static ServiceResponse<bool> IsBase64ImageSizeValid(string base64String, int sizeInMB)
    {
        if (string.IsNullOrEmpty(base64String))
        {
            return new ServiceResponse<bool>{Data = false, Message = "Base64 string is empty"};
        }

        try
        {
            // Remove the data:image/...;base64, prefix if it exists
            var base64Data = base64String.Contains(",") ? base64String.Split(',')[1] : base64String;

            // Convert base64 string to byte array
            byte[] imageBytes = Convert.FromBase64String(base64Data);

            // Calculate the size in megabytes
            double imageSizeInMB = (double)imageBytes.Length / (1024 * 1024);

            // Check if the image size is more than the specified size
            var isSizeValid = imageSizeInMB <= sizeInMB;
            if(!isSizeValid) return new ServiceResponse<bool>{Data = false, Message = "Ukuran gambar terlalu besar"};
            return new ServiceResponse<bool>{Data = true};
        }
        catch (FormatException)
        {
            // Handle invalid base64 string
            return new ServiceResponse<bool>{Data = false, Message = "Terjadi kesalahan saat mengonversi base64 string"};
        }
    }
    private static ServiceResponse<bool> CheckOrCreateDirectory(string path)
    {
        try
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return new ServiceResponse<bool>{Data = true};
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error from CheckOrCreateDirectory: {e.Message}");
            return new ServiceResponse<bool>{Data = false, Message = "Terjadi kesalahan saat membuat image"};
        }
    }
}