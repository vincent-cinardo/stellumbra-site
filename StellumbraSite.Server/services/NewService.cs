using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StellumbraSite.Shared.Models;

namespace StellumbraSite.Server.Services
{
    public class NewsService
    {
        private readonly IWebHostEnvironment _env;
        public NewsService(IWebHostEnvironment env)
        {
            _env = env;
        }
        public async Task<List<NewsItem>> GetNewsAsync()
        {
            var path = Path.Combine(_env.WebRootPath, "data", "news.json");
            var json = await File.ReadAllTextAsync(path);
            JObject jObject = JObject.Parse(json);
            return jObject["news"].ToObject<List<NewsItem>>();
        }
    }
}
