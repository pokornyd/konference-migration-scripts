using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Resources;
using System.Text;
using Konference.Models;
using Newtonsoft.Json;
using Konference.Interfaces;
using System.Threading.Tasks;

namespace Konference
{
    class AssetMigrator : Migrator, IMigrator
    {
        public AssetMigrator(string projectId, string apiKey) : base(projectId, apiKey)
        {
        }

        public async Task Migrate()
        {
            Folders folders = GetFolders();
            List <AssetBinary> assetBinaries = GetAssetBinaries();

            await SetFolders(folders);
            await SetAssets(assetBinaries);
        }

        public Folders GetFolders()
        {
            string foldersJson = GetJsonResource("Jsons.Folders.json");
            Folders folders = JsonConvert.DeserializeObject<Folders>(foldersJson);

            return folders;
        }

        public async Task SetFolders(Folders folders)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Authorization", "Bearer " + ApiKey);
                client.Headers.Add("Content-type", "application/json");
                Uri endpoint = new Uri(BaseEndpoint + "/folders");

                try
                {
                    string jsonBody = JsonConvert.SerializeObject(folders);
                    string response = await client.UploadStringTaskAsync(endpoint, "POST", jsonBody);
                    Console.WriteLine("Folders created successfully");
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        string errorStream = reader.ReadToEnd();
                        Error error = JsonConvert.DeserializeObject<Error>(errorStream);
                        if (error.ValidationErrors != null)
                        {
                            foreach (ValidationError validationError in error.ValidationErrors)
                            {
                                Console.WriteLine("Folders not migrated, error: " + validationError.Message);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Folders not migrated, error:" + error.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public List<AssetBinary> GetAssetBinaries()
        {
            ResourceSet resourceSet = new ResourceManager(typeof(Images)).GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            List<AssetBinary> assetBinaries = new List<AssetBinary>();           

            foreach (DictionaryEntry entry in resourceSet)
            {
                string resourceKey = entry.Key.ToString();
                byte[] binary = entry.Value as byte[];
                int length = binary.Length;

                AssetBinary assetBinary = new AssetBinary
                {
                    FileName = resourceKey.ToLower().Replace(" ", "-"),
                    ContentLength = length,
                    ContentType = "application/octet-stream",
                    Binary = binary
                };
                assetBinaries.Add(assetBinary);
            }
            return assetBinaries;
        }

        public async Task SetAssets(List<AssetBinary> assetBinaries)
        {
            foreach (AssetBinary assetBinary in assetBinaries)
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("Authorization", "Bearer " + ApiKey);
                    client.Headers.Add("Content-type", "image/jpeg");
                    client.Headers.Add("Content-length", assetBinary.ContentLength.ToString());

                    string folderExternalId = "";

                    switch (assetBinary.FileName.Substring(0, 3))
                    {
                        case "brn":
                            folderExternalId = "brno_folder";
                            break;
                        case "mel":
                            folderExternalId = "melbourne_folder";
                            break;
                        case "den":
                            folderExternalId = "denver_folder";
                            break;
                    }


                    try
                    {
                        Uri endpoint = new Uri(BaseEndpoint + "/files/" + assetBinary.FileName);

                        var response = await client.UploadDataTaskAsync(endpoint, "POST", assetBinary.Binary);
                        FileReference reference = JsonConvert.DeserializeObject<FileReference>(Encoding.UTF8.GetString(response));

                        Asset asset = new Asset
                        {
                            FileReference = reference,
                            Descriptions = new Description[0],
                            Folder = new Folder()
                            {
                                ExternalId = folderExternalId
                            },
                            ExternalId = "asset_" + assetBinary.FileName.ToLower()
                        };

                        endpoint = new Uri(BaseEndpoint + "/assets");
                        string jsonBody = JsonConvert.SerializeObject(asset);

                        await client.UploadStringTaskAsync(endpoint, "POST", jsonBody);
                        Console.WriteLine("Asset object successfully paired with binary");
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
                                    Console.WriteLine("Assets not created, error: " + validationError.Message);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Assets not created, error: " + error.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorFlag = true;
                        Console.WriteLine("Assets not created, error: " + ex.Message);
                    }
                }
            }
            if (ErrorFlag)
            {
                Console.WriteLine("\nErrors encountered during asset migration, some assets may not have been created.\n");
            }
            else
            {
                Console.WriteLine("\nAssets created successfully.\n");
            }
        }
    }
}
