using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Konference.Interfaces;
using Konference.Models;
using Newtonsoft.Json;

namespace Konference
{
    class TaxonomyMigrator : Migrator, IMigrator
    {
        public TaxonomyMigrator(string projectId, string apiKey) : base(projectId, apiKey)
        {
        }

        public async Task Migrate()
        {
            Taxonomy taxonomy = GetTaxonomy();
            await SetTaxonomy(taxonomy);
        }

        public Taxonomy GetTaxonomy()
        {
            var taxonomyJson = GetJsonResource("Jsons.Taxonomies.json");
            Taxonomy taxonomy = JsonConvert.DeserializeObject<Taxonomy>(taxonomyJson);

            return taxonomy;
        }

        public async Task SetTaxonomy(Taxonomy taxonomy)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Authorization", "Bearer " + ApiKey);
                client.Headers.Add("Content-type", "application/json");
                string jsonBody = JsonConvert.SerializeObject(taxonomy);

                try
                {
                    if (jsonBody != null)
                    {
                        Uri uri = new Uri(BaseEndpoint + "/taxonomies");
                        string response = await client.UploadStringTaskAsync(uri, "POST", jsonBody);
                        Console.WriteLine("Taxonomy: " + taxonomy.name + " migrated successfully");
                    }
                }
                catch (WebException ex)
                {
                    ErrorFlag = true;
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        string errorStream = reader.ReadToEnd();
                        Error error = JsonConvert.DeserializeObject<Error>(errorStream);

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
}
