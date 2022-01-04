using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using System.Diagnostics;
using System.Net;
using WebScarping.Models;

namespace WebScarping.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private string _url;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _url = "";
        }

        public IActionResult Index()
        {
            _url = _configuration.GetSection("Urls").GetSection("wiki-url").Value;
            var response = ParseUrl(_url).Result;
            var result = HtmlParse(response);

            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private static async Task<string> ParseUrl(string url)
        {
            HttpClient httpClient = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            httpClient.DefaultRequestHeaders.Accept.Clear();
            var resultResponse = httpClient.GetStringAsync(url);
            return await resultResponse;
        }
        private List<Statistics> HtmlParse(string htmlData)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlData);

            var element = htmlDocument.DocumentNode.SelectNodes("//*[@class='wikitable']//tr");

            List<Statistics> keyWordList = new List<Statistics>();
            foreach (var row in element)
            {
                var heading = row.SelectNodes("td");
                if (heading != null)
                {

                    var statistics = new Statistics
                    {
                        Rank = Convert.ToInt32(heading[0].InnerText),
                        KeyWord = heading[1].InnerText,
                        WatchNumberCount = heading[3].InnerText
                    };
                    keyWordList.Add(statistics);
                }

            }

            return keyWordList;
        }
        private async Task<string> ParseHeadless()
        {
            _url = _configuration.GetSection("Urls").GetSection("wiki-url").Value;
            List<string> list = new();
            var options = new LaunchOptions()
            {
                Headless = true,
                ExecutablePath = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe"
            };

            var browser = await Puppeteer.LaunchAsync(options, null);
            var page = await browser.NewPageAsync();

            await page.GoToAsync(_url);

            var links = @"Array.from(document.querySelectorAll('a')).map(a => a.href);";
            var urls = await page.EvaluateExpressionAsync<string[]>(links);


            foreach (string url in urls)
            {
                list.Add(url);
            }

            return null;
        }
    }
}