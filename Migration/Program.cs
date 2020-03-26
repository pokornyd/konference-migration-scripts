using System;
using Konference.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Konference
{
    class Program
    {
        static IConfiguration _config;
        static async Task Main()
        {

            await Init();
            Console.WriteLine("Migration finished.");
        }

        static async Task Init()
        {
            _config = new ConfigurationBuilder()
                .AddJsonStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("Konference.Config.json"))
                .Build();

            MigrationClient client = new MigrationClient(_config["apiKey"], _config["projectId"]);

            Console.WriteLine("\nChoose project type:\n");
            Console.WriteLine("(f)ull -- includes all items and their published variants");
            Console.WriteLine("(m)inimal -- only includes content types and an empty taxonomy");
            Console.Write("\nType (f/m): ");

            string projectType = Console.ReadLine();

            switch (projectType)
            {
                case "f":
                    Console.WriteLine("\nFull migration chosen.\n");
                    await MigrateFull(client);
                    break;
                case "m":
                    Console.WriteLine("Minimal migration chosen.\n");
                    await MigrateMin(client);
                    break;
                default:
                    Console.WriteLine("Invalid input, defaulting to full migration.");
                    await MigrateFull(client);
                    break;
            }        
        }

        static async Task MigrateFull(MigrationClient client)
        {
            IMigrator[] migrators =
            {
                new AssetMigrator(client),
                new TaxonomyMigrator(client, false),
                new TypeMigrator(client),
                new ItemMigrator(client),
                new VariantMigrator(client)
            };

            foreach (IMigrator migrator in migrators)
            {
                await migrator.Migrate();
            }
        }

        static async Task MigrateMin(MigrationClient client)
        {
            IMigrator[] migrators =
            {
                new TaxonomyMigrator(client, true),
                new TypeMigrator(client)
            };

            foreach (IMigrator migrator in migrators)
            {
                await migrator.Migrate();
            }
        }
    }
}
