using System;
using Newtonsoft.Json;
using Konference.Models;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Konference.Interfaces;

namespace Konference
{
    class VariantMigrator : Migrator, IMigrator
    {
        public VariantMigrator(string projectId, string apiKey) : base(projectId, apiKey)
        {
        }

        public async Task Migrate()
        {
            LanguageVariants languageVariants = GetLanguageVariants();
            await SetLanguageVariants(languageVariants);
        }

        public LanguageVariants GetLanguageVariants()
        {
            string variantsJson = GetJsonResource("Jsons.Variants.json");
            LanguageVariants languageVariants = JsonConvert.DeserializeObject<LanguageVariants>(variantsJson);

            return languageVariants;
        }

        public async Task SetLanguageVariants(LanguageVariants languageVariants)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Authorization", "Bearer " + ApiKey);
                client.Headers.Add("Content-type", "application/json");

                foreach (Variant variant in languageVariants.Variants)
                {
                    Uri endpoint = new Uri(BaseEndpoint + "/items/codename/" + variant.Item.Codename + "/variants/codename/default");

                    try
                    {
                        await PublishLanguageVariant(variant.Item.Codename, false);
                        string jsonBody = JsonConvert.SerializeObject(variant);
                        string response = await client.UploadStringTaskAsync(endpoint, "PUT", jsonBody);
                        Console.WriteLine("Language variant for '" + variant.Item.Codename + "' upserted.");
                        await PublishLanguageVariant(variant.Item.Codename, true);
                    }
                    catch (WebException ex)
                    {
                        using (var stream = ex.Response.GetResponseStream())
                        using (var reader = new StreamReader(stream))
                        {
                            string errorStream = reader.ReadToEnd();
                            Error error = JsonConvert.DeserializeObject<Error>(errorStream);

                            if (error.ErrorCode == 103 || error.ErrorCode == 213)
                            {
                                try
                                {
                                    string jsonBody = JsonConvert.SerializeObject(variant);
                                    string response = await client.UploadStringTaskAsync(endpoint, "PUT", jsonBody);
                                    Console.WriteLine("Language variant for '" + variant.Item.Codename + "' upserted.");
                                    await PublishLanguageVariant(variant.Item.Codename, true);
                                    continue;
                                }

                                catch (WebException wex)
                                {
                                    using (var stream2 = wex.Response.GetResponseStream())
                                    using (var reader2 = new StreamReader(stream))
                                    {
                                        errorStream = reader2.ReadToEnd();
                                        error = JsonConvert.DeserializeObject<Error>(errorStream);
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
        }

        public async Task<string> PublishLanguageVariant(string codename, bool publish)
        {
            string changeState = publish ? "publish" : "unpublish";
            using (WebClient client = new WebClient())
            {
                Uri endpoint = new Uri(BaseEndpoint + "/items/codename/" + codename + "/variants/codename/default/" + changeState);
                client.Headers.Add("Authorization", "Bearer " + ApiKey);
                return await client.UploadStringTaskAsync(endpoint, "PUT", "");
            }
        }
    }
}
