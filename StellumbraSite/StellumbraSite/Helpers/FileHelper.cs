using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;

namespace StellumbraSite.Helpers
{
    public static class FileHelper
    {
        public static async Task<string> UploadImage(IBrowserFile file, IJSRuntime jsRuntime)
        {
            // TODO: Ensure it is an image of the right format before uploading.

            if (file == null)
            {
                return string.Empty;
            }

            var base64String = await ConvertToBase64(file);
            string format = Path.GetExtension(file.Name).Replace(".", "");

            string filepath = await jsRuntime.InvokeAsync<string>("apiRequests.addImage", base64String, format);
            return filepath;
        }
        public static async Task<string> ConvertToBase64(IBrowserFile file, bool fullURL = false)
        {
            using var stream = file.OpenReadStream(5 * 1024 * 1024);
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);

            var bytes = ms.ToArray();

            if (fullURL)
            {
                return $"data:{file.ContentType};base64,{Convert.ToBase64String(bytes)}";
            }
            else
            {
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
