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

            Console.Out.WriteLine(bioToolsList.Count);
            Console.ReadLine();

            /*BiocondaDirectorySearch bio = new BiocondaDirectorySearch();
            List<JObject> convertedYamlList = bio.LookThroughYaml("C:/Users/Mathias/bioconda-recipes/recipes");

            List<String> namesFromBiotools = new List<string>();
            List<String> namesFromBioConda = new List<string>();*/

            

            /*foreach (JObject jobject in bioToolsList)
            {
                namesFromBiotools.Add(jobject["id"].ToString().ToLower());

            }*/

            /*foreach (JObject jobject in convertedYamlList)
            {
                namesFromBioConda.Add(jobject["package"]["name"].ToString());
            }
            */
            


        }

        public static List<String> GenerateListOfIntersectingPackagesBetweenCranAndBiotools(List<String> namesFromBiotools)
        {
            CranScanner cranscanner = new CranScanner();
            List<String> cranPackages = cranscanner.ScanCran();
            List<string> packagesInterceptingOnCranAndBiotools = new List<string>();

            foreach (string cranPackage in cranPackages)
            {
                if (namesFromBiotools.Contains(cranPackage.ToLower()))
                {
                    packagesInterceptingOnCranAndBiotools.Add(cranPackage);
                }
            }

            return packagesInterceptingOnCranAndBiotools;

        }

        public static List<String> GenerateListOfIntersectingPackagesBetweenPypiAndBiotools(List<String> namesFromBiotools)
        {
            PyPiScanner pypiscanner = new PyPiScanner();
            List<string> listOfAllPypiPackages = pypiscanner.ScanPyPi();
            List<string> listofPackagesinterceptingOnPyPiAndBiotools = new List<string>();

            foreach (string pypiPackage in listOfAllPypiPackages)
            {
                if (namesFromBiotools.Contains(pypiPackage.ToLower()))
                {
                    listofPackagesinterceptingOnPyPiAndBiotools.Add(pypiPackage);
                }
            }

            return listofPackagesinterceptingOnPyPiAndBiotools;
        }

        public static List<String> GenerateListOfIntersectingPackagesBetweenBioconductorAndBiotools(List<String> namesFromBiotools)
        {
            BioconductorScanner bioconductorScanner = new BioconductorScanner();
            List<string> listOfAllBioconductorPackages = bioconductorScanner.ScanBioconductor();
            List<string> listofPackagesinterceptingOnBioconductorAndBiotools = new List<string>();

            foreach (string BioconductorPackage in listOfAllBioconductorPackages)
            {
               
                if (namesFromBiotools.Contains(BioconductorPackage.ToLower()))
                {
                    listofPackagesinterceptingOnBioconductorAndBiotools.Add(BioconductorPackage);
                }
            }

            return listofPackagesinterceptingOnBioconductorAndBiotools;
        }

        public static List<String> GenerateListOfIntersectingPackagesBetweenBioconductorAndBioconda(List<String> namesFromBioconda)
        {
            BioconductorScanner bioconductorScanner = new BioconductorScanner();
            List<string> listOfAllBioconductorPackages = bioconductorScanner.ScanBioconductor();
            List<string> listofPackagesinterceptingOnBioconductorAndBioconda = new List<string>();

            foreach(string biocondaPackage in namesFromBioconda)
            {
                string tempBiocondaPackageName = biocondaPackage;

                if (biocondaPackage.StartsWith("bioconductor-"))
                {
                     tempBiocondaPackageName = biocondaPackage.Substring(13);  
                }

                if (listOfAllBioconductorPackages.Contains(tempBiocondaPackageName.ToLower()))
                {
                    listofPackagesinterceptingOnBioconductorAndBioconda.Add(tempBiocondaPackageName);
                }
            }

            return listofPackagesinterceptingOnBioconductorAndBioconda;
        }


        public static void GenerateCSVFile(List<String> listOfPackages)
        {
            string csv = String.Join(",", listOfPackages.ToArray());
            Console.Out.WriteLine(Directory.GetCurrentDirectory());
            File.WriteAllText(Directory.GetCurrentDirectory() + "/../packages.csv", csv);
            Console.Out.WriteLine("Written file to: " + Directory.GetCurrentDirectory() + "/../packages.csv");
            Console.Out.WriteLine("Press any key to exit");
            Console.ReadLine();
        }

       
    }
}