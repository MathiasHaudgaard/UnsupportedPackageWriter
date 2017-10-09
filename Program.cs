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

            List<String> namesFromBiotools = new List<string>();

            foreach (JObject jobject in bioToolsList)
            {
                namesFromBiotools.Add(jobject["id"].ToString().ToLower());
                Console.Out.WriteLine(jobject["id"].ToString());
            }

            GenerateCSVFile(GenerateListOfIntersectingPackagesBetweenCranAndBiotools(namesFromBiotools));




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

        public static List<Tuple<string,string>> GenerateListOfIntersectingPackagesBetweenBiocondaAndBiotools(List<String> namesFromBiotools, List<Tuple<String,String>> namesFromBioconda)
        {
            List<Tuple<String, String>> intersectingPackages = new List<Tuple<string, string>>();
            foreach(Tuple<String,String> tupleFromBioconda in namesFromBioconda)
            {
                if (namesFromBiotools.Contains(tupleFromBioconda.Item1)){
                    intersectingPackages.Add(tupleFromBioconda);
                }
            }
            Console.Out.WriteLine(intersectingPackages.Count);
            return intersectingPackages;

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

        public static void GenerateCSVFileFromTuple(List<Tuple<String,String>> listOfPackages)
        {
            string packages = String.Join(",", listOfPackages.Select(x => x.Item1.ToString()).ToArray());
            string prefixes = String.Join(",", listOfPackages.Select(x => x.Item2.ToString()).ToArray());
            Console.Out.WriteLine(Directory.GetCurrentDirectory());
            File.WriteAllText(Directory.GetCurrentDirectory() + "/../packages.csv", packages);
            Console.Out.WriteLine("Written file to: " + Directory.GetCurrentDirectory() + "/../packages.csv");
            File.WriteAllText(Directory.GetCurrentDirectory() + "/../prefixes.csv", prefixes);
            Console.Out.WriteLine("Written file to: " + Directory.GetCurrentDirectory() + "/../prefixes.csv");
            Console.Out.WriteLine("Press any key to exit");

            Console.ReadLine();
        }

        public static void GenerateMappingBetweenBiocondaAndBiotools()
        {
            Console.Out.WriteLine("Retrieving packages from BioTools ...");
            BioToolsRetriever bioToolsRetriever = new BioToolsRetriever();
            List<JObject> bioToolsList = bioToolsRetriever.GetJsonObjectFromBioTools();



            Console.Out.WriteLine(bioToolsList.Count);

            BiocondaRetriever biocondaRetriever = new BiocondaRetriever();
            List<String> biocondaList = biocondaRetriever.GenerateListAsync();
            Console.Out.WriteLine(biocondaList.Count);



            List<String> namesFromBiotools = new List<string>();
            List<Tuple<string, string>> namesFromBioConda = new List<Tuple<string, string>>();



            foreach (JObject jobject in bioToolsList)
            {
                namesFromBiotools.Add(jobject["id"].ToString().ToLower());
            }
            Console.Out.WriteLine("CCAT exists in biotools: " + namesFromBiotools.Contains("ccat"));

            string prefix;
            foreach (String package in biocondaList)
            {
                prefix = "none";
                string biocondaName = package.ToLower();
                if (biocondaName.Equals("ccat"))
                    Console.Out.WriteLine("CCAT exists in bioconda!!!");

                if (biocondaName.StartsWith("bioconductor-"))
                {
                    biocondaName = biocondaName.Substring("bioconductor-".Length);
                    prefix = "bioconductor";
                }
                else if (biocondaName.StartsWith("r-"))
                {
                    biocondaName = biocondaName.Substring("r-".Length);
                    prefix = "r";
                }
                else if (biocondaName.StartsWith("perl-"))
                {
                    biocondaName = biocondaName.Substring("perl-".Length);
                    prefix = "perl";
                }


                namesFromBioConda.Add(new Tuple<string, string>(biocondaName, prefix));
            }

            GenerateCSVFileFromTuple(GenerateListOfIntersectingPackagesBetweenBiocondaAndBiotools(namesFromBiotools, namesFromBioConda));
        }

       
    }
}