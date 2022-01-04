 # CodeWhateverYouWant :computer:
```Express your idea through code``` 

:+1: Good Luck to Me - Time to code :computer: :smile:

# Projects in this Repo
1. C# Hello World Console [Link](https://github.com/Kshitij-Kafle-123/codeWhateverYouWant/tree/main/HelloWorldC%23)

1. Web Scraping with .NET MVC [Link](https://github.com/Kshitij-Kafle-123/codeWhateverYouWant/tree/main/WebScarping)
 
---
# Project Description
- Web Scraping (Top 25 Keywords searched by people in Wiki)

*This project is based on .Net MVC to filter out data of certain time interval using the concept of web-scrap.*

## Steps
Create MVC Project it will automatically provides you a Controller and a View
```csharp
public class HomeController : Controller
{

}

```
```csharp
<div class="text-center">
//blablabla...
</div>
```
Add these libraries in a HomeController (For HtmlAgilityPack install html agility pack from NUGET Package Manager)

```csharp
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using WebScarping.Models;
```

### Now Actual/Final Code


```Controller``` 




```csharp

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
```
```Model``` 

```csharp
namespace WebScarping.Models
{
    public class Statistics
    {
        public int Rank { get; set; }
        public string? KeyWord { get; set; }
        public string? WatchNumberCount { get; set; }
    }
}

```

```View```

```csharp
@model IEnumerable<WebScarping.Models.Statistics>

<div class="text-center">
    <h1 style="color:darkblue">Most Searched KeyWords of The Week (December 19 to 25, 2021)</h1>

    <br />
    <br />

    <table class="table table-info">
        <thead>
            <tr>
                <th>Rank</th>
                <th>KeyWord</th>
                <th>ViewCount</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var statistics in Model)
            {
                <tr>
                    <td>@statistics.Rank</td>
                    <td>@statistics.KeyWord</td>
                    <td>@statistics.WatchNumberCount</td>
                </tr>
            }

        </tbody>
    </table>
</div>

```

```Result```

![alt Output](https://github.com/Kshitij-Kafle-123/codeWhateverYouWant/blob/public/WebScarping/wwwroot/img/output.png)


---
[![github](https://cloud.githubusercontent.com/assets/17016297/18839843/0e06a67a-83d2-11e6-993a-b35a182500e0.png)][1]
---
[1]:http://www.github.com/Kshitij-Kafle-123
