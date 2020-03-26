using System;
using System.Net;
using System.Threading.Tasks;
using Konference.Interfaces;
using Konference.Models;
using Newtonsoft.Json;

namespace Konference
{
    class TaxonomyMigrator : Migrator, IMigrator
    {
        private bool IsMinimalMigration { get; set; }
        public TaxonomyMigrator(MigrationClient client, bool isMinimalMigration) : base(client)
        {
            IsMinimalMigration = isMinimalMigration;
        }

        public async Task Migrate()
        {
            Taxonomy taxonomy = GetTaxonomy();
            if (IsMinimalMigration)
            {
                taxonomy.Terms = new Term[] {};
            }

            await SetTaxonomy(taxonomy);
        }

        private Taxonomy GetTaxonomy()
        {
            var taxonomyJson = GetJsonResource("Jsons.Taxonomies.json");
            Taxonomy taxonomy = JsonConvert.DeserializeObject<Taxonomy>(taxonomyJson);

            return taxonomy;
        }

        private async Task SetTaxonomy(Taxonomy taxonomy)
        {
            string jsonBody = JsonConvert.SerializeObject(taxonomy);

            try
            {
                if (jsonBody != null)
                {  
                    await MigrationClient.SendRequestToEndpoint("/taxonomies", "POST", jsonBody);
                    Console.WriteLine("Taxonomy: " + taxonomy.Name + " migrated successfully");
                }
            }
            catch (WebException ex)
            {
                ErrorFlag = true;
                string errorMessage = ex.Message;
                Error error = JsonConvert.DeserializeObject<Error>(errorMessage);

                if (error.ValidationErrors != null)
                {
                    foreach (ValidationError validationError in error.ValidationErrors)
                    {
                        Console.WriteLine("Taxonomies not migrated, error: " + validationError.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Taxonomies not migrated, error: " + error.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (ErrorFlag)
            {
                Console.WriteLine("\nError encountered during taxonomy creation, some taxonomies were not created.\n");
            }
            else
            {
                Console.WriteLine("\nTaxonomies created successfully.\n");
            }
        }
    }
}
