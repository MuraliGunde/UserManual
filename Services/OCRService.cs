using System;
using System.IO;
using Tesseract;

namespace UserManualNew.Services
{

    public class OCRService
    {
        /*public string ConvertBase64ToImageAndExtractText(string base64String)
        {
            var filePath = "screenshot.png";
            byte[] imageBytes = Convert.FromBase64String(base64String);
            File.WriteAllBytes(filePath, imageBytes);

            return ExtractTextFromImage(filePath);
        }

        public string ExtractTextFromImage(string filePath)
        {
            var tessDataPath = @"C:\tessdata"; // Path to tessdata folder
            using (var engine = new TesseractEngine(tessDataPath, "eng", EngineMode.Default))
            {
                using (var img = Pix.LoadFromFile(filePath))
                {
                    var result = engine.Process(img);
                    return result.GetText();
                }
            }
        }*/
        public string ConvertBase64ToImageAndExtractText(string filePath)
        {
            try
            {
                /*// Remove Base64 prefix if present
                if (base64String.Contains(","))
                {
                    base64String = base64String.Split(',')[1];
                }

                var filePath = Path.Combine(Path.GetTempPath(), $"screenshot_{Guid.NewGuid()}.png");
                byte[] imageBytes = Convert.FromBase64String(base64String);
                File.WriteAllBytes(filePath, imageBytes);*/

                // Extract text
                var extractedText = ExtractTextFromImage(filePath);

                // Clean up file
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                return extractedText;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public string ExtractTextFromImage(string filePath)
        {
            var tessDataPath = @"C:\Program Files\Tesseract-OCR\tessdata"; // Path to tessdata folder
            try
            {
                using (var engine = new TesseractEngine(tessDataPath, "eng", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(filePath))
                    {
                        using (var result = engine.Process(img))
                        {
                            return result.GetText().Trim(); // Trim whitespace
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error during OCR: {ex.Message}";
            }
        }

    }
}   

