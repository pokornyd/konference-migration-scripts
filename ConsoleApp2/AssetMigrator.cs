using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Text;
using Konference.Models;
using Newtonsoft.Json;
using Resources;

namespace Konference
{
    class AssetMigrator : Migrator
    {
        public AssetMigrator(string projectId, string apiKey) : base(projectId, apiKey)
        {
        }

        public Folders GetFolders()
        {
            using (StreamReader reader = new StreamReader("C:\\Users\\DanielP\\source\\repos\\konference-migration-scripts\\ConsoleApp2\\Jsons\\Folders.json"))
            {
                string jsonBody = reader.ReadToEnd();
                Folders folders = JsonConvert.DeserializeObject<Folders>(jsonBody);
                return folders;
            }
        }

        public async void SetFolders(Folders folders)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Authorization", "Bearer " + ApiKey);
                client.Headers.Add("Content-type", "application/json");
                Uri endpoint = new Uri("https://manage.kontent.ai/v2/projects/" + ProjectId + "/folders");

                //foreach (Folder folder in folders.AssetFolders)
                //{
                //    try
                //    {
                //        string jsonBody = JsonConvert.SerializeObject(folder);
                //        string response = await client.UploadStringTaskAsync(endpoint, "POST", jsonBody);
                //        Console.Write("Folder \"" + folder.Name + "\" created successfully\n");
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.Write(ex.Message);
                //    }
                //}

                try
                {
                    string jsonBody = JsonConvert.SerializeObject(folders);
                    string response = await client.UploadStringTaskAsync(endpoint, "POST", jsonBody);
                    Console.Write("Folders created successfully\n");
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
        }

        public List<AssetBinary> GetAssetBinaries()
        {
            ResourceManager resourceManager = new ResourceManager(typeof(Images));
            ResourceSet resourceSet = resourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            List<AssetBinary> assetBinaries = new List<AssetBinary>();

            foreach (DictionaryEntry entry in resourceSet)
            {
                string resourceKey = entry.Key.ToString();
                byte[] binary = (byte[])entry.Value;
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
            //System.Type t = typeof(Resource);
            //var assembly = Assembly.GetAssembly(t);
            //foreach (var resourcename in assembly.GetManifestResourceNames())
            //    Console.WriteLine(resourcename);
        }

        public async void SetAssets(List<AssetBinary> assetBinaries)
        {
            foreach (AssetBinary assetBinary in assetBinaries)
            {
                using (WebClient client = new WebClient())
                {
                    Uri endpoint = new Uri("https://manage.kontent.ai/v2/projects/" + ProjectId + "/files/" + assetBinary.FileName);
                    client.Headers.Add("Authorization", "Bearer " + ApiKey);
                    client.Headers.Add("Content-type", "image/jpeg");
                    client.Headers.Add("Content-length", assetBinary.ContentLength.ToString());

                    var response = await client.UploadDataTaskAsync(endpoint, "POST", assetBinary.Binary);
                    string referenceResponse = Encoding.UTF8.GetString(response);
                    FileReference reference = JsonConvert.DeserializeObject<FileReference>(referenceResponse);
                    Console.WriteLine("Binaries for " + assetBinary.FileName + " created successfully");

                    endpoint = new Uri("https://manage.kontent.ai/v2/projects/" + ProjectId + "/assets");
                    string folderExternalId = "";
                    //string assetNameFolderReference = assetBinary.FileName.Substring(0,3);
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

                    string jsonBody = JsonConvert.SerializeObject(asset);
                    var response2 = await client.UploadStringTaskAsync(endpoint, "POST", jsonBody);
                    Console.WriteLine("Asset object successfully paired with binary");
                }
            }
        }

        //private async void UploadDataCallback(Object sender, UploadDataCompletedEventArgs e)
        //{
        //    string referenceResponse = Encoding.UTF8.GetString(e.Result);
        //    FileReference reference = JsonConvert.DeserializeObject<FileReference>(referenceResponse);

        //    Uri endpoint = new Uri("https://manage.kontent.ai/v2/projects/" + ProjectId + "/assets");
        //    Asset asset = new Asset
        //    {
        //        FileReference = reference,
        //        Descriptions = new Description[0],
        //        Folder = 
        //    };

        //    using (WebClient client = new WebClient())
        //    {
        //        client.Headers.Add("Authorization", "Bearer " + ApiKey);

        //        string jsonBody = JsonConvert.SerializeObject(asset);
        //        var response = await client.UploadStringTaskAsync(endpoint, "POST", jsonBody);
        //        Console.WriteLine("Asset object successfully paired with binary");
        //    }
        //}
    }
}
