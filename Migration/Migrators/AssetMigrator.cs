using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Resources;
using Konference.Models;
using Newtonsoft.Json;
using Konference.Interfaces;
using System.Threading.Tasks;

namespace Konference
{
    class AssetMigrator : Migrator, IMigrator
    {
        public AssetMigrator(MigrationClient client) : base(client)
        {
        }

        public async Task Migrate()
        {
            Folders folders = GetFolders();
            List <AssetBinary> assetBinaries = GetAssetBinaries();

            await SetFolders(folders);
            await SetAssets(assetBinaries);
        }

        private Folders GetFolders()
        {
            string foldersJson = GetJsonResource("Jsons.Folders.json");
            Folders folders = JsonConvert.DeserializeObject<Folders>(foldersJson);

            return folders;
        }

        private async Task SetFolders(Folders folders)
        {
            try
            {
                string jsonBody = JsonConvert.SerializeObject(folders);
                await MigrationClient.SendRequestToEndpoint("/folders", "POST", jsonBody);
                Console.WriteLine("Folders created successfully\n");
            }
            catch (WebException ex)
            {
                string errorStream = ex.Message;
                Error error = JsonConvert.DeserializeObject<Error>(errorStream);
                if (error.ValidationErrors != null)
                {
                    foreach (ValidationError validationError in error.ValidationErrors)
                    {
                        Console.WriteLine("Folders not migrated, error: " + validationError.Message + "\n");
                    }
                }
                else
                {
                    Console.WriteLine("Folders not migrated, error: " + error.Message + "\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private List<AssetBinary> GetAssetBinaries()
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
                    ContentType = "image/jpeg",
                    Binary = binary
                };
                assetBinaries.Add(assetBinary);
            }
            return assetBinaries;
        }

        private async Task SetAssets(List<AssetBinary> assetBinaries)
        {
            foreach (AssetBinary assetBinary in assetBinaries)
            {
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
                    case "spe":
                        folderExternalId = "speakers_folder";
                        break;
                    case "spo":
                        folderExternalId = "sponsors_folder";
                        break;
                }


                try
                {
                    var response = await MigrationClient.SendDataToAssetEndpoint(assetBinary, "POST");
                    FileReference reference = JsonConvert.DeserializeObject<FileReference>(response);

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

                    await MigrationClient.SendRequestToEndpoint("/assets", "POST", jsonBody);
                    Console.WriteLine("Asset object " + assetBinary.FileName + " successfully paired with binary");
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
                            Console.WriteLine("Assets not created, error: " + validationError.Message);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Assets not created, error: " + error.Message);
                    }
                }
                catch (Exception ex)
                {
                    ErrorFlag = true;
                    Console.WriteLine("Assets not created, error: " + ex.Message);
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
