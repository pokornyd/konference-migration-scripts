using System;
using System.Net;
using System.Threading.Tasks;
using Konference.Interfaces;
using Konference.Models;
using Newtonsoft.Json;

namespace Konference
{
    class TypeMigrator : Migrator, IMigrator
    {
        public TypeMigrator(MigrationClient client) : base(client)
        {
        }

        public async Task Migrate()
        {
            ContentTypes contentTypes = GetContentTypes();
            await SetContentTypes(contentTypes);
        }

        private ContentTypes GetContentTypes()
        {
            var typesJson = GetJsonResource("Jsons.Types.json");
            ContentTypes contentTypes = JsonConvert.DeserializeObject<ContentTypes>(typesJson);

            return contentTypes;
        }

        private async Task SetContentTypes(ContentTypes contentTypes)
        {
            foreach (ContentType contentType in contentTypes.Types)
            {
                await Task.Delay(100); //rate limit protection
                try
                {
                    string jsonBody = JsonConvert.SerializeObject(contentType);
                    await MigrationClient.SendRequestToEndpoint("/types", "POST", jsonBody);
                    Console.WriteLine("Type \"" + contentType.Name + "\" migrated successfully");                    
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
                            Console.WriteLine("Type \"" + contentType.Name + "\" not migrated, error: " + validationError.Message);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Type: " + contentType.Name + " Error: " + error.Message);
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
