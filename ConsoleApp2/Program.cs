using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Konference.Models;

namespace Konference
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter project ID of your target project:");
            string targetProjectId = Console.ReadLine();
            Console.WriteLine("Enter CM API key of the target project:");
            string apiKey = Console.ReadLine();

            //TaxonomyMigrator migrator = new TaxonomyMigrator("70a30bfb-3dcb-01c1-950b-bd7218c5f583", "ew0KICAiYWxnIjogIkhTMjU2IiwNCiAgInR5cCI6ICJKV1QiDQp9.ew0KICAianRpIjogIjg4NmUzYjAyYmM1MTQ2YTg5Njk4YTZjNTQ5NDgwNjM3IiwNCiAgImlhdCI6ICIxNTc4OTkyOTM0IiwNCiAgImV4cCI6ICIxOTI0NTkyOTM0IiwNCiAgInByb2plY3RfaWQiOiAiNzBhMzBiZmIzZGNiMDFjMTk1MGJiZDcyMThjNWY1ODMiLA0KICAidmVyIjogIjIuMS4wIiwNCiAgInVpZCI6ICJOdGpncGVsaTlhUm1zZ2t1OGZYeUJGb3VBNTJiM1prWmJQcDYydWN2TkVJIiwNCiAgImF1ZCI6ICJtYW5hZ2Uua2VudGljb2Nsb3VkLmNvbSINCn0._1_SdD55xt9_BAUi-IaW62jbEQHZ9sTVL4c-LP3kCdA");
            //Taxonomy taxonomy = migrator.GetTaxonomy();
            //string result = migrator.SetTaxonomy(taxonomy).Result;
            //Console.WriteLine(result);
            //Console.ReadLine();

            //TypeMigrator typeMigrator = new TypeMigrator("70a30bfb-3dcb-01c1-950b-bd7218c5f583", "ew0KICAiYWxnIjogIkhTMjU2IiwNCiAgInR5cCI6ICJKV1QiDQp9.ew0KICAianRpIjogIjg4NmUzYjAyYmM1MTQ2YTg5Njk4YTZjNTQ5NDgwNjM3IiwNCiAgImlhdCI6ICIxNTc4OTkyOTM0IiwNCiAgImV4cCI6ICIxOTI0NTkyOTM0IiwNCiAgInByb2plY3RfaWQiOiAiNzBhMzBiZmIzZGNiMDFjMTk1MGJiZDcyMThjNWY1ODMiLA0KICAidmVyIjogIjIuMS4wIiwNCiAgInVpZCI6ICJOdGpncGVsaTlhUm1zZ2t1OGZYeUJGb3VBNTJiM1prWmJQcDYydWN2TkVJIiwNCiAgImF1ZCI6ICJtYW5hZ2Uua2VudGljb2Nsb3VkLmNvbSINCn0._1_SdD55xt9_BAUi-IaW62jbEQHZ9sTVL4c-LP3kCdA");
            //ContentTypes contentTypes = typeMigrator.GetContentTypes();
            //typeMigrator.SetContentTypes(contentTypes);
            //Console.ReadLine();

            //AssetMigrator assetMigrator = new AssetMigrator("70a30bfb-3dcb-01c1-950b-bd7218c5f583", "ew0KICAiYWxnIjogIkhTMjU2IiwNCiAgInR5cCI6ICJKV1QiDQp9.ew0KICAianRpIjogIjg4NmUzYjAyYmM1MTQ2YTg5Njk4YTZjNTQ5NDgwNjM3IiwNCiAgImlhdCI6ICIxNTc4OTkyOTM0IiwNCiAgImV4cCI6ICIxOTI0NTkyOTM0IiwNCiAgInByb2plY3RfaWQiOiAiNzBhMzBiZmIzZGNiMDFjMTk1MGJiZDcyMThjNWY1ODMiLA0KICAidmVyIjogIjIuMS4wIiwNCiAgInVpZCI6ICJOdGpncGVsaTlhUm1zZ2t1OGZYeUJGb3VBNTJiM1prWmJQcDYydWN2TkVJIiwNCiAgImF1ZCI6ICJtYW5hZ2Uua2VudGljb2Nsb3VkLmNvbSINCn0._1_SdD55xt9_BAUi-IaW62jbEQHZ9sTVL4c-LP3kCdA");

            //Folders folders = assetMigrator.GetFolders();
            //assetMigrator.SetFolders(folders);


            //List<AssetBinary> assetBinaries = assetMigrator.GetAssetBinaries();
            //assetMigrator.SetAssets(assetBinaries);
            //Console.ReadLine();

            ItemMigrator itemMigrator = new ItemMigrator("70a30bfb-3dcb-01c1-950b-bd7218c5f583", "ew0KICAiYWxnIjogIkhTMjU2IiwNCiAgInR5cCI6ICJKV1QiDQp9.ew0KICAianRpIjogIjg4NmUzYjAyYmM1MTQ2YTg5Njk4YTZjNTQ5NDgwNjM3IiwNCiAgImlhdCI6ICIxNTc4OTkyOTM0IiwNCiAgImV4cCI6ICIxOTI0NTkyOTM0IiwNCiAgInByb2plY3RfaWQiOiAiNzBhMzBiZmIzZGNiMDFjMTk1MGJiZDcyMThjNWY1ODMiLA0KICAidmVyIjogIjIuMS4wIiwNCiAgInVpZCI6ICJOdGpncGVsaTlhUm1zZ2t1OGZYeUJGb3VBNTJiM1prWmJQcDYydWN2TkVJIiwNCiAgImF1ZCI6ICJtYW5hZ2Uua2VudGljb2Nsb3VkLmNvbSINCn0._1_SdD55xt9_BAUi-IaW62jbEQHZ9sTVL4c-LP3kCdA");
            ContentItems contentItems = itemMigrator.GetContentItems();
            itemMigrator.SetContentItems(contentItems);
            Console.WriteLine("success");
            Console.ReadLine();

            VariantMigrator variantMigrator = new VariantMigrator("70a30bfb-3dcb-01c1-950b-bd7218c5f583", "ew0KICAiYWxnIjogIkhTMjU2IiwNCiAgInR5cCI6ICJKV1QiDQp9.ew0KICAianRpIjogIjg4NmUzYjAyYmM1MTQ2YTg5Njk4YTZjNTQ5NDgwNjM3IiwNCiAgImlhdCI6ICIxNTc4OTkyOTM0IiwNCiAgImV4cCI6ICIxOTI0NTkyOTM0IiwNCiAgInByb2plY3RfaWQiOiAiNzBhMzBiZmIzZGNiMDFjMTk1MGJiZDcyMThjNWY1ODMiLA0KICAidmVyIjogIjIuMS4wIiwNCiAgInVpZCI6ICJOdGpncGVsaTlhUm1zZ2t1OGZYeUJGb3VBNTJiM1prWmJQcDYydWN2TkVJIiwNCiAgImF1ZCI6ICJtYW5hZ2Uua2VudGljb2Nsb3VkLmNvbSINCn0._1_SdD55xt9_BAUi-IaW62jbEQHZ9sTVL4c-LP3kCdA");
            LanguageVariants languageVariants = variantMigrator.GetLanguageVariants();
            variantMigrator.SetLanguageVariants(languageVariants);
            Console.WriteLine("success variants");
            Console.ReadLine();

            //WebRequest req = WebRequest.Create("https://failtecdn.azureedge.net/eventsportal-dev/4f588249-6939-0e25-717c-71b7ce7b8f7f.jpg");
            //WebResponse response = req.GetResponse();
            //Stream stream = response.GetResponseStream();
            //stream.



        }
    }
}
