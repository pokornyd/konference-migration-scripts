using System.IO;
using System.Reflection;

namespace Konference
{
    class Migrator
    {
        public Migrator(MigrationClient client) 
        {
            MigrationClient = client;
        }

        protected MigrationClient MigrationClient { get; private set; }

        protected bool ErrorFlag { get; set; } = false;

        protected string GetJsonResource(string resource)
        {
            var textStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Konference." + resource);
            string json = new StreamReader(textStream).ReadToEnd();

            return json;
        }
    }
}
 