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


        public BiocondaDirectorySearch()
        {

        }


        public List<JObject> LookThroughYaml(string path)
        {
            int exceptionCounter = 0;
            List<JObject> convertedYamlList = new List<JObject>();
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
                            convertedYamlList.Add(convertedYaml);
                            

                        }
                        catch(Exception e)
                        {
                            exceptionCounter++;
                        }

                    }
                }

            }
            catch (IOException e)
            {
                Console.Out.WriteLine(e);
            }
           
            return convertedYamlList;
        }

    }
}
