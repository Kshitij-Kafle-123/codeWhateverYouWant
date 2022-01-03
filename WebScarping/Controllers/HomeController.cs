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
            _url = "https://en.wikipedia.org/wiki/Wikipedia:Top_25_Report";
        }

        public IActionResult Index()
        {
            var response = ParseUrl(_url).Result;
            var result = HtmlParse(response);
            return View();
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
            Statistics statistics = new Statistics();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlData);

            var element = htmlDocument.DocumentNode.SelectNodes("//*[@class='wikitable']//tr");
            //.Where(node => node.GetAttributeValue("class", "").Contains("wikitable")).ToList();
            //var mostSearched = element.Where();
            List<Statistics> keyWordList = new List<Statistics>();
            foreach (var row in element)
            {
                var heading = row.SelectNodes("td");
                if (heading != null)
                {
                    statistics.Rank = Convert.ToInt32(heading[0].InnerText);
                    statistics.KeyWord = heading[1].InnerText;
                    statistics.WatchNumberCount = heading[3].InnerText;

                    keyWordList.Add(statistics);
                }
            }
            return keyWordList;
        }
    }
}