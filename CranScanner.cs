using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace UnsupportedPackageWriter
{
    class CranScanner
    {
        string URL = "https://cran.r-project.org/web/packages/available_packages_by_name.html";

        public List<String> ScanCran()
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

            HtmlNodeCollection Nodes = cranHtml.DocumentNode.SelectNodes("//a[@href]");
            int skipAlphabet = 0;
            foreach (var link in Nodes)
            {
                if(skipAlphabet < 26)
                {
                    skipAlphabet++;
                    continue;
                }
                listOfAllCranPackages.Add(link.InnerHtml);
               
            }

            return listOfAllCranPackages;

        }
    }
}
