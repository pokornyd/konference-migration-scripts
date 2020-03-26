using Konference.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Konference
{
    public class MigrationClient
    {
        private static string BaseEndpoint { get; set; }

        private static HttpClient Client { get; set; }
        public MigrationClient(string apiKey, string projectId)
        {
            BaseEndpoint = "https://manage.kontent.ai/v2/projects/" + projectId;
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        }


        public async Task SendRequestToEndpoint(string endpoint, string method, string data)
        {
            var request = new HttpRequestMessage
            {
                Content = new StringContent(data, Encoding.UTF8, "application/json"),
                RequestUri = new Uri(BaseEndpoint + endpoint),
                Method = new HttpMethod(method),               
            };
            await CheckRequestSucceeded(await Client.SendAsync(request, HttpCompletionOption.ResponseContentRead));
        }

        public async Task<string> SendDataToAssetEndpoint(AssetBinary assetBinary, string method)
        {
            var request = new HttpRequestMessage
            {
                Content = new ByteArrayContent(assetBinary.Binary),
                RequestUri = new Uri(BaseEndpoint + "/files/" + assetBinary.FileName),
                Method = new HttpMethod(method)
            };
            request.Content.Headers.Add("Content-type", assetBinary.ContentType);
            request.Content.Headers.Add("Content-length", assetBinary.Binary.Length.ToString());

            var result = await Client.SendAsync(request);

            return result.Content.ReadAsStringAsync().Result;
        }

        private async Task CheckRequestSucceeded(HttpResponseMessage message)
        {
            if (!message.IsSuccessStatusCode)
            {
                string exMessage = await message.Content.ReadAsStringAsync();
                throw new WebException(exMessage);
            }
        }
    }
}
