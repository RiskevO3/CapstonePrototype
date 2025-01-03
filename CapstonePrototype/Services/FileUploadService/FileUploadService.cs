using System.Text;
using System.Text.RegularExpressions;

namespace CapstonePrototype.Services.FileUploadService;
public class FileUploadService:IFileUploadService
{
    private readonly string _pdfSavePath;
    private readonly string _imageSavePath;
    private readonly IWebHostEnvironment _env;

    public FileUploadService(IWebHostEnvironment env)
    {
        // In a real project, you might inject these paths from configuration (e.g., appsettings.json)
        _env = env;
        _pdfSavePath = Path.Combine(_env.WebRootPath, "PDFs");
        _imageSavePath = Path.Combine(_env.WebRootPath, "Images");
        if(!Directory.Exists(_pdfSavePath))Directory.CreateDirectory(_pdfSavePath);
        if(!Directory.Exists(_imageSavePath))Directory.CreateDirectory(_imageSavePath);
    }

    public async Task<string> UploadPdfAsync(string fileName, string base64Content)
    {
        if (string.IsNullOrWhiteSpace(base64Content))
            throw new ArgumentException("Base64Content is required.", nameof(base64Content));

        // Strip out possible "data:application/pdf;base64," prefix if present.
        var base64Data = RemoveBase64Prefix(base64Content, "pdf");
        byte[] fileBytes = Convert.FromBase64String(base64Data);

        if (!IsPdf(fileBytes))
            throw new InvalidOperationException("Invalid PDF file.");

        string safeFileName = string.IsNullOrWhiteSpace(fileName)
            ? $"uploaded_{Guid.NewGuid()}.pdf"
            : Path.GetFileNameWithoutExtension(fileName) + ".pdf"; // ensure .pdf extension

        string filePath = Path.Combine(_pdfSavePath, safeFileName);
        await File.WriteAllBytesAsync(filePath, fileBytes);

        return $"http://0.0.0.0:5173/PDFs/{safeFileName}";
    }

    public async Task<string> UploadImageAsync(string fileName, string base64Content)
    {
        if (string.IsNullOrWhiteSpace(base64Content))
            throw new ArgumentException("Base64Content is required.", nameof(base64Content));

        var (base64Data, fileExt) = ExtractImageBase64AndExtension(base64Content);
        byte[] fileBytes = Convert.FromBase64String(base64Data);

        if (!IsImage(fileBytes))
            throw new InvalidOperationException("Invalid image file.");

        string safeExt = fileExt ?? "png";
        string safeFileName = string.IsNullOrWhiteSpace(fileName)
            ? $"image_{Guid.NewGuid()}.{safeExt}"
            : $"{Path.GetFileNameWithoutExtension(fileName)}.{safeExt}";

        string filePath = Path.Combine(_imageSavePath, safeFileName);
        await File.WriteAllBytesAsync(filePath, fileBytes);

        return $"http://0.0.0.0:5173/Images/{safeFileName}";
    }

    private static bool IsPdf(byte[] fileBytes)
    {
        if (fileBytes.Length < 4) return false;
        var header = Encoding.ASCII.GetString(fileBytes, 0, 4);
        return header.StartsWith("%PDF");
    }

    private static bool IsImage(byte[] fileBytes)
    {
        if (fileBytes.Length < 4) return false;

        // PNG check
        if (fileBytes[0] == 0x89 && fileBytes[1] == 0x50 && fileBytes[2] == 0x4E && fileBytes[3] == 0x47)
            return true;

        // JPG check (FF D8 FF at start and FF D9 at end)
        if (fileBytes[0] == 0xFF && fileBytes[1] == 0xD8 && fileBytes[fileBytes.Length - 2] == 0xFF && fileBytes[fileBytes.Length - 1] == 0xD9)
            return true;

        // GIF check (GIF87a or GIF89a)
        var headerStr = Encoding.ASCII.GetString(fileBytes, 0, Math.Min(6, fileBytes.Length));
        if (headerStr.StartsWith("GIF87a") || headerStr.StartsWith("GIF89a"))
            return true;

        return false;
    }

    private static string RemoveBase64Prefix(string base64Content, string expectedType)
    {
        // Example: data:application/pdf;base64,XXXX
        var pattern = @$"^data:application/{expectedType};base64,";
        return Regex.Replace(base64Content, pattern, "", RegexOptions.IgnoreCase);
    }

    private static (string base64Data, string extension) ExtractImageBase64AndExtension(string base64Content)
    {
        // Typical data URI: data:image/png;base64,<BASE64_DATA>
        var match = Regex.Match(base64Content, @"^data:image/(?<ext>[a-zA-Z0-9]+);base64,(?<data>.*)$", RegexOptions.IgnoreCase);
        if (match.Success)
        {
            return (match.Groups["data"].Value, match.Groups["ext"].Value);
        }
        else
        {
            // No data URI prefix, return as-is with no extension
            return (base64Content, null!);
        }
    }
}

