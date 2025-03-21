using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManualNew.DTOs;

namespace UserManualNew.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaptureController : ControllerBase
    {
        private readonly ScreenshotService _screenshotService;

        public CaptureController(ScreenshotService screenshotService)
        {
            _screenshotService = screenshotService;
        }

        [HttpPost("start")]
        public async Task<ActionResult<ScreenshotDetailsDTO>> StartCapture()
        {
            var result = await _screenshotService.CaptureScreenshot();
            return Ok(result);
        }
        [HttpPost("ExtractText")]
        public async Task<ActionResult<ScreenshotDetailsDTO>> ExtractTextFromImage([FromBody] IFormFile file)
        {
            var result = await _screenshotService.CaptureScreenshot();
            return Ok(result);
        }
    }
}

