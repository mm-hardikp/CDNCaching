using System.Diagnostics;
using CDNCaching.Models;
using Microsoft.AspNetCore.Mvc;

namespace CDNCaching.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _cloudflareBaseUrl;

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient, IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = httpClient;
            _cloudflareBaseUrl = configuration["CDNBaseUrl"];
        }

        [HttpGet("/")]
        public async Task<IActionResult> Index()
        {
            var cloudflareUrl = $"{_cloudflareBaseUrl}/index.html";
            var response = await _httpClient.GetAsync(cloudflareUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                ViewData["CloudflareContent"] = content;
                return View();
            }

            return NotFound("Cloudflare page not found.");
        }
        public async Task<IActionResult> About()
        {
            var cloudflareUrl = $"{_cloudflareBaseUrl}/about.html";

            var response = await _httpClient.GetAsync(cloudflareUrl);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                ViewData["PageContent"] = content;
                return View();
            }

            return NotFound("About page not found on Cloudflare.");
        }

        public async Task<IActionResult> Services()
        {
            var cloudflareUrl = $"{_cloudflareBaseUrl}/services.html";

            var response = await _httpClient.GetAsync(cloudflareUrl);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                ViewData["PageContent"] = content;
                return View();
            }

            return NotFound("Services page not found on Cloudflare.");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
