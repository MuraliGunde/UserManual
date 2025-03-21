using System.Drawing.Imaging;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using UserManualNew.DTOs;
using Microsoft.Extensions.Options;
using UserManualNew.Services;

public class ScreenshotService
{
    private readonly string _outputFolder;
    private readonly HttpClient _httpClient;
    private readonly DeepSeekSettings _settings;
    private readonly OCRService _oCRService;

    public ScreenshotService(HttpClient httpClient, IOptions<DeepSeekSettings> settings, OCRService oCRService)
    {
        _outputFolder = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");
        if (!Directory.Exists(_outputFolder))
            Directory.CreateDirectory(_outputFolder);

        _httpClient = httpClient;
        _settings = settings.Value;
        _oCRService = oCRService;
    }

    public async Task<ScreenshotDetailsDTO> CaptureScreenshot()
    {
        ScreenshotDetailsDTO detailsDTO = new ScreenshotDetailsDTO();
        var fileName = $"screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png";
        detailsDTO.filePath = Path.Combine(_outputFolder, fileName);

        // ✅ Capture screenshot dynamically
        int width, height;
        using (var graphics = Graphics.FromHwnd(IntPtr.Zero))
        {
            width = (int)graphics.VisibleClipBounds.Width;
            height = (int)graphics.VisibleClipBounds.Height;
        }

        await Task.Run(() =>
        {
            using (var bitmap = new Bitmap(width, height))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
                }
                bitmap.Save(detailsDTO.filePath, ImageFormat.Png);
            }
        });

        // ✅ Convert to Base64
        detailsDTO.base64 = await Task.Run(() =>
        {
            using (var ms = new MemoryStream())
            {
                using (var bitmap = new Bitmap(detailsDTO.filePath))
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    var bytes = ms.ToArray();
                    return Convert.ToBase64String(bytes);
                }
            }
        });

        detailsDTO.transcription = _oCRService. ConvertBase64ToImageAndExtractText(detailsDTO.filePath);

        return detailsDTO;
    }

}

