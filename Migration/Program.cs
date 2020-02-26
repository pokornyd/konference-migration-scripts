using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Konference.Models;
using Konference.Interfaces;
using System.Threading.Tasks;

namespace Konference
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Init();
            Console.WriteLine("Migration finished.");
            Console.Read();
        }

        static async Task Init()
        {
            Console.WriteLine("Enter project ID of your target project:");
            string projectId = Console.ReadLine();
            Console.WriteLine("Enter CM API key of the target project:");
            string apiKey = Console.ReadLine();

            projectId = "70a30bfb-3dcb-01c1-950b-bd7218c5f583";
            apiKey = "ew0KICAiYWxnIjogIkhTMjU2IiwNCiAgInR5cCI6ICJKV1QiDQp9.ew0KICAianRpIjogIjg4NmUzYjAyYmM1MTQ2YTg5Njk4YTZjNTQ5NDgwNjM3IiwNCiAgImlhdCI6ICIxNTc4OTkyOTM0IiwNCiAgImV4cCI6ICIxOTI0NTkyOTM0IiwNCiAgInByb2plY3RfaWQiOiAiNzBhMzBiZmIzZGNiMDFjMTk1MGJiZDcyMThjNWY1ODMiLA0KICAidmVyIjogIjIuMS4wIiwNCiAgInVpZCI6ICJOdGpncGVsaTlhUm1zZ2t1OGZYeUJGb3VBNTJiM1prWmJQcDYydWN2TkVJIiwNCiAgImF1ZCI6ICJtYW5hZ2Uua2VudGljb2Nsb3VkLmNvbSINCn0._1_SdD55xt9_BAUi-IaW62jbEQHZ9sTVL4c-LP3kCdA";

            Console.WriteLine("\nChoose project type:\n");
            Console.WriteLine("(f)ull -- includes all items and their published variants");
            Console.WriteLine("(m)inimal -- only includes content types, assets and taxonomies, no items");
            Console.Write("\nType (f/m): ");

            string projectType = Console.ReadLine();

            switch (projectType)
            {
                case "f":
                    await MigrateFull(projectId, apiKey);
                    break;
                case "m":
                    await MigrateMin(projectId, apiKey);
                    break;
                default:
                    await MigrateFull(projectId, apiKey);
                    break;
            }        
        }

        static async Task MigrateFull(string projectId, string apiKey)
        {
            Console.WriteLine("\nFull migration chosen.\n");

            IMigrator[] migrators =
            {
                new AssetMigrator(projectId, apiKey),
                new TaxonomyMigrator(projectId, apiKey),
                new TypeMigrator(projectId, apiKey),
                new ItemMigrator(projectId, apiKey),
                new VariantMigrator(projectId, apiKey)
            };

            foreach (IMigrator migrator in migrators)
            {
                await migrator.Migrate();
            }
        }

        static async Task MigrateMin(string projectId, string apiKey)
        {
            Console.WriteLine("Minimal migration chosen.\n");

            IMigrator[] migrators =
{
                new AssetMigrator(projectId, apiKey),
                new TaxonomyMigrator(projectId, apiKey),
                new TypeMigrator(projectId, apiKey),
            };

            foreach (IMigrator migrator in migrators)
            {
                await migrator.Migrate();
            }
        }
    }
}
