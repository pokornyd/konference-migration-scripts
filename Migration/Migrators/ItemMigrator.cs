using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Konference.Interfaces;
using Konference.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Konference
{
    class ItemMigrator : Migrator, IMigrator
    {
        public ItemMigrator(string projectId, string apiKey) : base(projectId, apiKey)
        {
        }

        public async Task Migrate()
        {
            ContentItems contentItems = GetContentItems();
            await SetContentItems(contentItems);
        }

        public ContentItems GetContentItems()
        {
            var itemsJson = GetJsonResource("Jsons.Items.json");
            ContentItems items = JsonConvert.DeserializeObject<ContentItems>(itemsJson);

            return items;
        }

        public async Task SetContentItems(ContentItems contentItems)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Authorization", "Bearer " + ApiKey);
                client.Headers.Add("Content-type", "application/json");
                Uri endpoint = new Uri(BaseEndpoint + "/items");

                foreach (Item item in contentItems.Items)
                {
                    try
                    {
                        string jsonBody = JsonConvert.SerializeObject(item);
                        string response = await client.UploadStringTaskAsync(endpoint, "POST", jsonBody);
                        Console.WriteLine("Item \"" + item.Name + "\" created successfully");
                    }
                    catch (WebException ex)
                    {
                        ErrorFlag = true;
                        using (var stream = ex.Response.GetResponseStream())
                        using (var reader = new StreamReader(stream))
                        {
                            string errorStream = reader.ReadToEnd();
                            Error error = JsonConvert.DeserializeObject<Error>(errorStream);
                            foreach (ValidationError validationError in error.ValidationErrors)
                            {
                                Console.WriteLine("Item \"" + item.Name + "\" not migrated, error: " + validationError.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorFlag = true;
                        Console.WriteLine("Item \"" + item.Name + "\" not migrated, error: " + ex.Message);
                    }
                }

                if (ErrorFlag)
                {
                    Console.WriteLine("\nErrors were encountered, some items may not have been created.\n");
                }
                else
                {
                    Console.WriteLine("\nItems created successfully\n");
                }
            }
        }
    }
}
