using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Konference.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Konference
{
    class ItemMigrator : Migrator
    {
        public ItemMigrator(string projectId, string apiKey) : base(projectId, apiKey)
        {
        }

        public ContentItems GetContentItems()
        {
            using (StreamReader reader = new StreamReader("C:\\Users\\DanielP\\source\\repos\\konference-migration-scripts\\ConsoleApp2\\Jsons\\Items.json"))
            {
                string jsonBody = reader.ReadToEnd();
                ContentItems contentItems = JsonConvert.DeserializeObject<ContentItems>(jsonBody);
                return contentItems;
            }
        }

        public async void SetContentItems(ContentItems contentItems)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Authorization", "Bearer " + ApiKey);
                client.Headers.Add("Content-type", "application/json");
                Uri uri = new Uri("https://manage.kontent.ai/v2/projects/" + ProjectId + "/items");

                foreach (Item item in contentItems.Items)
                {
                    try
                    {
                        string jsonBody = JsonConvert.SerializeObject(item);
                        string response = await client.UploadStringTaskAsync(uri, "POST", jsonBody);
                        Console.Write("Item \"" + item.Name + "\" created successfully\n");
                    }
                    catch (WebException ex)
                    {
                        using (var stream = ex.Response.GetResponseStream())
                        using (var reader = new StreamReader(stream))
                        {
                            string errorStream = reader.ReadToEnd();
                            Error error = JsonConvert.DeserializeObject<Error>(errorStream);
                            foreach (ValidationError validationError in error.ValidationErrors)
                            {
                                Console.Write("Type \"" + item.Name + "\" not migrated, error: " + validationError.Message + "\n");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write("Item \"" + item.Name + "\" not migrated, error: " + ex.Message + "\n");
                    }
                }
            }
        }
    }
}
