using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using WebScarping.Models;

namespace WebScarping.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private string _url;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _url = "";
        }

        public IActionResult Index()
        {
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
    }
}