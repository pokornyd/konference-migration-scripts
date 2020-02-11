using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Konference.Models;
using System.IO;
using System.Net;

namespace Konference
{
    class VariantMigrator : Migrator
    {
        public VariantMigrator(string projectId, string apiKey) : base(projectId, apiKey)
        {
        }

        public LanguageVariants GetLanguageVariants()
        {
            using (StreamReader reader = new StreamReader("C:\\Users\\DanielP\\source\\repos\\konference-migration-scripts\\ConsoleApp2\\Jsons\\Variants.json"))
            {
                string jsonBody = reader.ReadToEnd();
                LanguageVariants languageVariants = JsonConvert.DeserializeObject<LanguageVariants>(jsonBody);
                return languageVariants;
            }
        }

        public async void SetLanguageVariants(LanguageVariants languageVariants)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Authorization", "Bearer " + ApiKey);
                client.Headers.Add("Content-type", "application/json");

                foreach (Variant variant in languageVariants.Variants)
                {
                    Uri uri = new Uri("https://manage.kontent.ai/v2/projects/" + ProjectId + "/items/codename/" + variant.Item.Codename + "/variants/codename/default");

                    try
                    {
                        Console.Write(uri + "\n\n");
                        string jsonBody = JsonConvert.SerializeObject(variant);
                        Console.Write(jsonBody);
                        string response = await client.UploadStringTaskAsync(uri, "PUT", jsonBody);
                        Console.Write("success");
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
        }
    }
}
