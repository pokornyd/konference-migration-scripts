using System;
using Newtonsoft.Json;
using Konference.Models;
using System.Net;
using System.Threading.Tasks;
using Konference.Interfaces;

namespace Konference
{
    class VariantMigrator : Migrator, IMigrator
    {
        public VariantMigrator(MigrationClient client) : base(client)
        {
        }

        public async Task Migrate()
        {
            LanguageVariants languageVariants = GetLanguageVariants();
            await SetLanguageVariants(languageVariants);
        }

        private LanguageVariants GetLanguageVariants()
        {
            string variantsJson = GetJsonResource("Jsons.Variants.json");
            LanguageVariants languageVariants = JsonConvert.DeserializeObject<LanguageVariants>(variantsJson);

            return languageVariants;
        }

        private async Task SetLanguageVariants(LanguageVariants languageVariants)
        {
            foreach (Variant variant in languageVariants.Variants)
            {
                string endpoint = "/items/codename/" + variant.Item.Codename + "/variants/codename/default";

                try
                {
                    string jsonBody = JsonConvert.SerializeObject(variant);

                    await PublishLanguageVariant(variant.Item.Codename, false);
                    await MigrationClient.SendRequestToEndpoint(endpoint, "PUT", jsonBody);
                    await PublishLanguageVariant(variant.Item.Codename, true);

                    Console.WriteLine("Language variant for '" + variant.Item.Codename + "' upserted.");
                }
                catch (WebException ex)
                {
                    string errorMessage = ex.Message;
                    Error error = JsonConvert.DeserializeObject<Error>(errorMessage);

                    if (error.ErrorCode == 103 || error.ErrorCode == 213)
                    {
                        try
                        {
                            string jsonBody = JsonConvert.SerializeObject(variant);

                            await MigrationClient.SendRequestToEndpoint(endpoint, "PUT", jsonBody);
                            await PublishLanguageVariant(variant.Item.Codename, true);

                            Console.WriteLine("Language variant for '" + variant.Item.Codename + "' upserted.");
                            continue;
                        }

                        catch (WebException wex)
                        {
                            errorMessage = wex.Message;
                            error = JsonConvert.DeserializeObject<Error>(errorMessage);
                        }
                        if (error.ValidationErrors != null)
                        {
                            foreach (ValidationError validationError in error.ValidationErrors)
                            {
                                Console.WriteLine("Variant not upserted for item: " + variant.Item.Codename + " Error: " + validationError.Message);
                                continue;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Variant not upserted for item: " + variant.Item.Codename + " Error: " + error.Message);
                            continue;
                        }
                    }

                    if (error.ValidationErrors != null)
                    {
                        foreach (ValidationError validationError in error.ValidationErrors)
                        {
                            Console.WriteLine("Variant not upserted for item: "+ variant.Item.Codename + " Error: " + validationError.Message);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Variant not upserted for item: " + variant.Item.Codename + " Error: " + error.Message);
                    }
                }
                catch (Exception ex)
                {
                    ErrorFlag = true;
                    Console.WriteLine(ex.Message);
                }
            }
            if (ErrorFlag)
            {
                Console.WriteLine("\nErrors encountered during variant upsertion, some variants may not have been upserted.\n");
            }
            else
            {
                Console.WriteLine("\nAll variants upserted successfully.\n");
            }
        }

        private async Task PublishLanguageVariant(string codename, bool publish)
        {
            string changeState = publish ? "publish" : "unpublish";
            {
                string endpoint = "/items/codename/" + codename + "/variants/codename/default/" + changeState;
                await MigrationClient.SendRequestToEndpoint(endpoint, "PUT", "");
            }
        }
    }
}
