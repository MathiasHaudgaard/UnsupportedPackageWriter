using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace UnsupportedPackageWriter
{
    class BioconductorScanner
    {
        string URL = "https://www.bioconductor.org/packages/release/bioc/";

        public List<String> ScanBioconductor()
        {

            string cranHtmlPage = "";
            List<string> listOfAllCranPackages = new List<string>();

            using (HttpClient hc = new HttpClient())
            {
                var json = hc.GetAsync(URL).ContinueWith((taskwithresponse) =>
                {
                    var response = taskwithresponse.Result;
                    var task = response.Content.ReadAsStringAsync();
                    task.Wait();
                    cranHtmlPage = task.Result;

                });
                json.Wait();
            };

            HtmlDocument cranHtml = new HtmlDocument();
            cranHtml.LoadHtml(cranHtmlPage);
            string innerHtml = cranHtml.GetElementbyId("PageContent").SelectSingleNode("//div[last()]").SelectSingleNode("//table").InnerHtml;
            HtmlDocument bioconductorListHtml = new HtmlDocument();
            bioconductorListHtml.LoadHtml(innerHtml);
            HtmlNodeCollection nodes = bioconductorListHtml.DocumentNode.SelectNodes("//a[@href]");
            
            foreach (var link in nodes)
            {
                listOfAllCranPackages.Add(link.InnerHtml);
            }

            return listOfAllCranPackages;

        }
    }
}
