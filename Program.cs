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
            Console.Out.WriteLine("Retrieving packages from BioTools ...");
            BioToolsRetriever bioToolsRetriever = new BioToolsRetriever();
            List<JObject> bioToolsList = bioToolsRetriever.GetJsonObjectFromBioTools();



            /*Console.Out.WriteLine("Retrieving packages from BioConda...");
            BiocondaRetriever bioCondaRetriever = new BiocondaRetriever();
            List<String> biocondaList= bioCondaRetriever.GenerateListAsync();*/

            BiocondaDirectorySearch bio = new BiocondaDirectorySearch();
            List<JObject> convertedYamlList = bio.LookThroughYaml("C:/Users/Mathias/bioconda-recipes/recipes");

            List<String> namesFromBiotools = new List<string>();
            List<String> namesFromBioConda = new List<string>();

            foreach (JObject jobject in bioToolsList)
            {
                namesFromBiotools.Add(jobject["id"].ToString().ToLower());
                
            }
            /*foreach (JObject jobject in convertedYamlList)
            {
                namesFromBioConda.Add(jobject["package"]["name"].ToString());
            }

            List<String> listOfPackagesNotIntersecting =  namesFromBiotools.Except(namesFromBioConda, new IdComparer()).ToList();

            foreach (JObject jobject in bioToolsList)
            {
               jobject["id"].Equals()
                
            }*/


            PyPiScanner pypiscanner = new PyPiScanner();
            List<string> listOfAllPypiPackages = pypiscanner.ScanPyPi();
            List<string> listofPackagesinterceptingOnPyPiAndBiotools = new List<string>();
            int pypipackageCounter = 0;

            foreach(string pypiPackage in listOfAllPypiPackages)
            {
                if (namesFromBiotools.Contains(pypiPackage.ToLower()))
                {
                    listofPackagesinterceptingOnPyPiAndBiotools.Add(pypiPackage);
                    Console.WriteLine(pypiPackage);
                    Console.ReadLine();
                }
            }
            //List<string> listOfPackagesNotIntersecting = bioToolsList.Except(biocondaList, new IdComparer()).ToList();
            /*string csv = String.Join(",", listofPackagesinterceptingOnPyPiAndBiotools.ToArray());
            Console.Out.WriteLine(Directory.GetCurrentDirectory());
            File.WriteAllText(Directory.GetCurrentDirectory() + "/../packages.csv", csv);
            Console.Out.WriteLine("Written file to: " + Directory.GetCurrentDirectory() + "/../packages.csv");
            Console.Out.WriteLine("Press any key to exit");
            Console.ReadLine();*/

        }

       
    }
}