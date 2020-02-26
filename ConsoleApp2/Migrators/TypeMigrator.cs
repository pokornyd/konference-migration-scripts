using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Konference.Interfaces;
using Konference.Models;
using Newtonsoft.Json;

namespace Konference
{
    class TypeMigrator : Migrator, IMigrator
    {
        public TypeMigrator(string projectId, string apiKey) : base(projectId, apiKey)
        {
        }

        public async Task Migrate()
        {
            ContentTypes contentTypes = GetContentTypes();
            await SetContentTypes(contentTypes);
        }

        public ContentTypes GetContentTypes()
        {
            var typesJson = GetJsonResource("Jsons.Types.json");
            ContentTypes contentTypes = JsonConvert.DeserializeObject<ContentTypes>(typesJson);

            return contentTypes;
        }

        public async Task SetContentTypes(ContentTypes contentTypes)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Authorization", "Bearer " + ApiKey);
                client.Headers.Add("Content-type", "application/json");
                Uri endpoint = new Uri(BaseEndpoint + "/types");

                foreach (ContentType contentType in contentTypes.Types)
                {
                    try
                    {
                        string jsonBody = JsonConvert.SerializeObject(contentType);
                        string response = await client.UploadStringTaskAsync(endpoint, "POST", jsonBody);
                        Console.WriteLine("Type \"" + contentType.Name + "\" migrated successfully");
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
                                    Console.WriteLine("Type \"" + contentType.Name + "\" not migrated, error: " + validationError.Message);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Type: " + contentType.Name + " Error: " + error.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorFlag = true;
                        Console.WriteLine("Type \"" + contentType.Name + "\" not migrated, error: " + ex.Message);
                    }
                }

                if (ErrorFlag)
                {
                    Console.WriteLine("\nErrors were encountered, some types may not have been properly created.\n");
                }
                else
                {
                    Console.WriteLine("\nAll types were created successfully.\n");
                }
            }
        }
    }
}
