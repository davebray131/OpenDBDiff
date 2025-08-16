using System;
using Microsoft.Extensions.Configuration;

namespace OpenDBDiff.CLI
{
    /// <summary>
    /// Creates populated configuration settings instances.
    /// </summary>
    internal static class ConfigurationFactory
    {
        private static readonly Lazy<IConfigurationRoot> _factory = new Lazy<IConfigurationRoot>(() => ConfigurationFactory.GetConfiguration());

        /// <summary>
        /// Singleton instance of the <see cref="IConfigurationRoot"/>.
        /// </summary>
        internal static IConfigurationRoot Instance => _factory.Value;

        /// <summary>
        /// Reads the configuration from appsettings.json.
        /// </summary>
        /// <returns></returns>
        internal static IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
