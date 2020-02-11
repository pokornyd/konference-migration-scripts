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
    class TypeMigrator : Migrator
    {
        public TypeMigrator(string projectId, string apiKey) : base(projectId, apiKey)
        {
        }

        public ContentTypes GetContentTypes()
        {
            using (StreamReader reader = new StreamReader("C:\\Users\\DanielP\\source\\repos\\konference-migration-scripts\\ConsoleApp2\\Jsons\\Types.json"))
            {
                string jsonBody = reader.ReadToEnd();
                ContentTypes contentTypes = JsonConvert.DeserializeObject<ContentTypes>(jsonBody);
                return contentTypes;
            }
        }

        public async void SetContentTypes(ContentTypes contentTypes)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Authorization", "Bearer " + ApiKey);
                client.Headers.Add("Content-type", "application/json");
                Uri uri = new Uri("https://manage.kontent.ai/v2/projects/" + ProjectId + "/types");
                bool errorFlag = false;

                foreach (ContentType contentType in contentTypes.Types)
                {
                    try
                    {
                        string jsonBody = JsonConvert.SerializeObject(contentType);
                        string response = await client.UploadStringTaskAsync(uri, "POST", jsonBody);
                        Console.WriteLine("Type \"" + contentType.Name + "\" migrated successfully");
                    }
                    catch (WebException ex)
                    {
                        errorFlag = true;
                        using (var stream = ex.Response.GetResponseStream())
                        using (var reader = new StreamReader(stream))
                        {
                            string errorStream = reader.ReadToEnd();
                            Error error = JsonConvert.DeserializeObject<Error>(errorStream);
                            foreach (ValidationError validationError in error.ValidationErrors)
                            {
                                Console.WriteLine("Type \"" + contentType.Name + "\" not migrated, error: " + validationError.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errorFlag = true;
                        Console.WriteLine("Type \"" + contentType.Name + "\" not migrated, error: " + ex.Message);
                    }
                }

                if (errorFlag)
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
