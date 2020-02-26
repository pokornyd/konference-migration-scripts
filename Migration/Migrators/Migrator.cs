using System.IO;
using System.Reflection;

namespace Konference
{
    class Migrator
    {
        public string ProjectId { get; set; }
        public string ApiKey { get; set; }
        public string BaseEndpoint 
        { 
            get
            {
                return "https://manage.kontent.ai/v2/projects/" + ProjectId;
            }
        }
        public bool ErrorFlag { get; set; } = false;
        public Migrator(string projectId, string apiKey)
        {
            ProjectId = projectId;
            ApiKey = apiKey;
        }

        public string GetJsonResource(string resource)
        {
            var textStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Konference." + resource);
            string json = new StreamReader(textStream).ReadToEnd();

            return json;
        }
    }
}
