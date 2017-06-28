using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using YamlDotNet.Serialization;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UnsupportedPackageWriter
{
    

    class BiocondaDirectorySearch
    {

        public List<string> homePageUrlList { get; }

        public BiocondaDirectorySearch(string path)
        {
            homePageUrlList = new List<string>();
            LookThroughYaml(path);

        }


        private List<string> LookThroughYaml(string path)
        {
            int exceptionCounter = 0;
            try
            {
                //these two for loops searches through every meta.yaml file
                foreach(string recipe in Directory.GetDirectories(path)){
                    foreach(string yamlFile in Directory.GetFiles(recipe, "meta.yaml"))
                    {
                        //This section we convert yaml into json and skip the yaml file with bad syntax
                        try {
                            string yamlText = File.ReadAllText(yamlFile);
                            var deserializer = new Deserializer();
                            var yamlObject = deserializer.Deserialize(new StringReader(yamlText));

                            StringBuilder sb = new StringBuilder();
                            var serializer = new JsonSerializer();

                            using(StringWriter sw = new StringWriter(sb))
                            using (JsonWriter jw = new JsonTextWriter(sw))
                            {
                                serializer.Serialize(jw, yamlObject);
                            }

                            JObject convertedYaml = JObject.Parse(sb.ToString());
                            
                            

                        }
                        catch(Exception e)
                        {
                            exceptionCounter++;
                        }
                        

                        /*string homeUrlfirst = yamlText.Substring(yamlText.IndexOf("home: ")+6);
                        string homeUrltrimming = homeUrlfirst.Substring(homeUrlfirst.IndexOf("//") + 2);
                        string homeUrlSecond = homeUrltrimming.Substring(0, homeUrltrimming.IndexOf("\n"));
                        string homeUrlScdTrimming = homeUrlSecond.Replace("'","").Replace("\"","");

                        homePageUrlList.Add(homeUrlScdTrimming);

                        Console.Out.WriteLine(homeUrlScdTrimming);
                        Console.ReadLine()*/
                        

                    }
                }

            }
            catch (IOException e)
            {
                Console.Out.WriteLine(e);
            }
            Console.Out.WriteLine(exceptionCounter);
            Console.ReadLine();
            return homePageUrlList;
        }

    }
}
