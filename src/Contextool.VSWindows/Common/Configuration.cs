namespace Contextool.Editor.Common
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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
                var configuration = new DataContractJsonSerializer(typeof(Configuration)).ReadObject(file) as Configuration;

                // Pretty-up values for the callers.
                configuration.Projects = configuration.Projects ?? Enumerable.Empty<string>();

                return configuration;
            }
        }

        [DataMember(Name = "projects")]
        public IEnumerable<string> Projects { get; private set; }
    }
}
