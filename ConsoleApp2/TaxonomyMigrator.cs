using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Konference.Models;
using Newtonsoft.Json;

namespace Konference
{
    class TaxonomyMigrator : Migrator
    {
        public TaxonomyMigrator(string projectId, string apiKey) : base(projectId, apiKey)
        {
        }

        public Taxonomy GetTaxonomy()
        {
            using (StreamReader reader = new StreamReader("C:\\Users\\DanielP\\source\\repos\\konference-migration-scripts\\ConsoleApp2\\Jsons\\Taxonomies.json"))
            {
                string jsonBody = reader.ReadToEnd();
                Taxonomy taxonomy = JsonConvert.DeserializeObject<Taxonomy>(jsonBody);
                return taxonomy;
            }
        }

        public async Task<String> SetTaxonomy(Taxonomy taxonomy)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Authorization", "Bearer " + ApiKey);
                client.Headers.Add("Content-type", "application/json");
                string jsonBody = JsonConvert.SerializeObject(taxonomy);

                if (jsonBody != null)
                {
                    Uri uri = new Uri("https://manage.kontent.ai/v2/projects/" + ProjectId + "/taxonomies");
                    string response = await client.UploadStringTaskAsync(uri, "POST", jsonBody);
                    return "Taxonomy: " + taxonomy.name + " migrated successfully";
                }
            }
            return null;
        }

    }


}
