namespace Contextool.Editor.Common
{
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.Threading;

    [DataContract]
    internal sealed class Configuration
    {
        public static async Task<Configuration> ReadFromFileAsync(string configurationFile)
        {
            // Ensure we're off the UI thread;
            await TaskScheduler.Default;

            using (var file = File.OpenRead(configurationFile))
            {
                return new DataContractJsonSerializer(typeof(Configuration)).ReadObject(file) as Configuration;
            }
        }
    }
}
