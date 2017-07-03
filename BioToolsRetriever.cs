using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace UnsupportedPackageWriter
{
    class BioToolsRetriever
    {

        protected string URL;
        private List<JObject> packagesWithoutWebappServiceDatabase;
        private int pageNumber = 1;

        public BioToolsRetriever()
        {
            packagesWithoutWebappServiceDatabase = new List<JObject>();
            URL = "https://bio.tools/api/tool/?format=json&language=python&page=";

            

        }

        public List<JObject> GetJsonObjectFromBioTools()
        {
            using (HttpClient hc = new HttpClient())
            {
               
                JObject jObject = null;

                do
                {
                    var json = hc.GetAsync(URL + pageNumber).ContinueWith((taskwithresponse) =>
                    {
                        var response = taskwithresponse.Result;
                        var jsonTask = response.Content.ReadAsStringAsync();
                        jsonTask.Wait();
                        jObject = JObject.Parse(jsonTask.Result);

                    });
                    json.Wait();

                    

                    String packageList = jObject["list"].ToString();

                    dynamic dynjason = JsonConvert.DeserializeObject(packageList);


                    foreach (var item in dynjason)
                    {

                        JObject package = JObject.Parse(item.ToString());
                        JArray toolTypes = JArray.Parse(package["toolType"].ToString());

                        //Console.Out.WriteLine(package);
                        
                        bool contiuneFlag = false;
                        foreach(String toolType in toolTypes)
                        {

                            if (!toolType.Equals("Web application") && !toolType.Equals("Database portal") && !toolType.Equals("Web service"))
                            {
                                contiuneFlag = false;
                                break;
                            } else
                            {
                                contiuneFlag = true;
                            }
                        }

                        if (contiuneFlag) {
                            contiuneFlag = false;
                            /*Console.Out.WriteLine("Did not at package");
                            Console.ReadLine();*/
                            continue;
                        }
                        //Console.Out.WriteLine("added package");
                        packagesWithoutWebappServiceDatabase.Add(package);
                        //Console.ReadLine();

                    }
                    pageNumber++;
                } while (jObject["next"].Type != JTokenType.Null);

            }
            return packagesWithoutWebappServiceDatabase;
        }

      

    }
}
