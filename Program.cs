using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnsupportedPackageWriter
{

    class Program
    {
        static void Main(string[] args)
        {

            /*Console.Out.WriteLine("Retrieving packages from BioTools ...");
            BioToolsRetriever bioToolsRetriever = new BioToolsRetriever();
            List<JObject> bioToolsList = bioToolsRetriever.GetJsonObjectFromBioTools();*/

            /*Console.Out.WriteLine("Retrieving packages from BioConda...");
            BiocondaRetriever bioCondaRetriever = new BiocondaRetriever();
            List<String> biocondaList= bioCondaRetriever.GenerateListAsync();*/

            BiocondaDirectorySearch bio = new BiocondaDirectorySearch("C:/Users/Mathias/bioconda-recipes/recipes");
            List<string> biocondaHomepageURL = bio.homePageUrlList;

            /*foreach (JObject jobject in bioToolsList)
            {
                if (biocondaHomepageURL.Contains(jobject["homepage"].ToString())){

                }
                
            }*/

            /*List<string> listOfPackagesNotIntersecting = bioToolsList.Except(biocondaList, new IdComparer()).ToList();
            string csv = String.Join(",", listOfPackagesNotIntersecting.ToArray());
            Console.Out.WriteLine(Directory.GetCurrentDirectory());
            File.WriteAllText(Directory.GetCurrentDirectory() + "/../packages.csv", csv);
            Console.Out.WriteLine("Written file to: " + Directory.GetCurrentDirectory() + "/../packages.csv");
            Console.Out.WriteLine("Press any key to exit");
            Console.ReadLine();*/

            


        }

       
    }
}