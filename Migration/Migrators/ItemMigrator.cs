using System;
using System.Net;
using System.Threading.Tasks;
using Konference.Interfaces;
using Konference.Models;
using Newtonsoft.Json;

namespace Konference
{
    class ItemMigrator : Migrator, IMigrator
    {
        public ItemMigrator(MigrationClient client) : base(client)
        {
        }

        public async Task Migrate()
        {
            ContentItems contentItems = GetContentItems();
            await SetContentItems(contentItems);
        }

        private ContentItems GetContentItems()
        {
            var itemsJson = GetJsonResource("Jsons.Items.json");
            ContentItems items = JsonConvert.DeserializeObject<ContentItems>(itemsJson);

            return items;
        }

        private async Task SetContentItems(ContentItems contentItems)
        {
            foreach (Item item in contentItems.Items)
            {
                await Task.Delay(100); //rate limit protection
                try
                {
                    string jsonBody = JsonConvert.SerializeObject(item);
                    await MigrationClient.SendRequestToEndpoint("/items", "POST", jsonBody);
                    Console.WriteLine("Item \"" + item.Name + "\" created successfully");
                }
                catch (WebException ex)
                {
                    ErrorFlag = true;
                    {
                        string errorMessage = ex.Message;
                        Error error = JsonConvert.DeserializeObject<Error>(errorMessage);
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
