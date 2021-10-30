using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.NewtonsoftJson;

namespace ApacheTech.VintageMods.Core.Services.Configuration.WritableJsonFile
{
    /// <summary>
    ///     Represents a JSON file as an <see cref="IConfigurationSource"/>.
    /// </summary>
    /// <seealso cref="NewtonsoftJsonConfigurationSource" />
    public class WritableJsonConfigurationSource : NewtonsoftJsonConfigurationSource
    {
        /// <summary>
        ///     Builds the <see cref="WritableJsonConfigurationProvider" /> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder" />.</param>
        /// <returns>An instance of type <see cref="WritableJsonConfigurationProvider" /></returns>
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            EnsureDefaults(builder);
            return new WritableJsonConfigurationProvider(this);
        }
    }
}