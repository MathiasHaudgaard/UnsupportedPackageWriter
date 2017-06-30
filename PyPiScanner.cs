using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net.Http;
using HtmlAgilityPack;

namespace UnsupportedPackageWriter
{
    class PyPiScanner
    {
        string URL = "https://pypi.python.org/simple/";

        public List<String> ScanPyPi()
        {

            string pypiHtmlPage = "";
            List<string> listOfAllPypiPackages = new List<string>();

            using(HttpClient hc = new HttpClient())
            {
                var json = hc.GetAsync(URL).ContinueWith((taskwithresponse) =>
                {
                    var response = taskwithresponse.Result;
                    var task = response.Content.ReadAsStringAsync();
                    task.Wait();
                    pypiHtmlPage = task.Result;

                });
                json.Wait();
            };

            HtmlDocument pypihtml = new HtmlDocument();
            pypihtml.LoadHtml(pypiHtmlPage);

            HtmlNodeCollection Nodes = pypihtml.DocumentNode.SelectNodes("//a[@href]");

            foreach (var link in Nodes)
            {
                listOfAllPypiPackages.Add(link.Attributes["href"].Value);
                
            }

            return listOfAllPypiPackages;

        }
    }
}
