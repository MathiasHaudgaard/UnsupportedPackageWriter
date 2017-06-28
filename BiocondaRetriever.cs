using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace UnsupportedPackageWriter
{
    class BiocondaRetriever
    {


        public List<string> GenerateListAsync()
        {
            List<string> packageList = new List<string>();
            using (HttpClient hc = new HttpClient())
            {
                try
                {
                    hc.DefaultRequestHeaders.UserAgent.ParseAdd("Everything");
                    JArray commits = null;
                    var json = hc.GetAsync("https://api.github.com/repos/bioconda/bioconda-recipes/commits").ContinueWith((taskwithresponse) =>
                    {
                        var response = taskwithresponse.Result;
                        var jsonTask = response.Content.ReadAsStringAsync();
                        jsonTask.Wait();
                        commits = JArray.Parse(jsonTask.Result);

                    });
                    json.Wait();
                    String newestCommitSHA = commits[0]["sha"].ToString();
                    JArray repoData = null;
                    json = hc.GetAsync("https://api.github.com/repos/bioconda/bioconda-recipes/git/trees/" + newestCommitSHA).ContinueWith((taskwithresponse) =>
                    {
                        var response = taskwithresponse.Result;
                        var jsonTask = response.Content.ReadAsStringAsync();
                        jsonTask.Wait();
                        repoData = (JArray)JObject.Parse(jsonTask.Result)["tree"];

                    });
                    json.Wait();
                    JObject recipeJObject = repoData.Children<JObject>().FirstOrDefault(o => o["path"] != null && o["path"].ToString().Equals("recipes"));
                    String recipeURL = recipeJObject["url"].ToString();
                    JArray packages = null;
                    json = hc.GetAsync(recipeURL).ContinueWith((taskwithresponse) =>
                    {
                        var response = taskwithresponse.Result;
                        var jsonTask = response.Content.ReadAsStringAsync();
                        jsonTask.Wait();
                        packages = JArray.Parse(JObject.Parse(jsonTask.Result)["tree"].ToString());

                    });
                    json.Wait();
                    foreach (var package in packages.Children())
                    {
                        packageList.Add(package["path"].ToString());
                    }
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine("Exception: " + e);
                }

            }
            return packageList;
        }
    }
}
