using StellumbraSite.Data;
using Microsoft.AspNetCore.Mvc;

namespace StellumbraSite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        [HttpPost("AddImage")]
        public async Task<IActionResult> AddImage([FromBody] ImageData imageData)
        {
            if (string.IsNullOrEmpty(imageData.base64str))
            { 
                return BadRequest("No image data provided.");
            }

            try
            {
                // Remove data URL prefix if it exists
                string base64 = imageData.base64str;

                if (base64.Contains(","))
                {
                    base64 = base64.Substring(base64.IndexOf(",") + 1);
                }

                // Convert base64 string to byte array
                var imageBytes = Convert.FromBase64String(base64);

                // Save file to disk
                var filepath = Path.Combine("wwwroot", imageData.filepath);
                var directory = Path.GetDirectoryName(filepath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                await System.IO.File.WriteAllBytesAsync(filepath, imageBytes);

                return Ok();
            }
            catch (FormatException)
            {
                return BadRequest("Invalid base64 string.");
            }
        }
    }
}
