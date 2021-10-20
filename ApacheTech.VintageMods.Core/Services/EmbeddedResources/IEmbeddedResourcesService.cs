using System.IO;
using System.Reflection;
using JetBrains.Annotations;

namespace ApacheTech.VintageMods.Core.Services.EmbeddedResources
{
    /// <summary>
    ///     Provides a means for interacting with embedded resources within mod assemblies.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public interface IEmbeddedResourcesService
    {
        /// <summary>
        ///     Reads the resource, and passes back the output as a string
        /// </summary>
        /// <param name="assembly">The assembly to load the resource from.</param>
        /// <param name="fileName">Name of the file, embedded within the assembly.</param>
        /// <returns>The contents of the file, as a string.</returns>
        /// <exception cref="FileNotFoundException">Embedded data file not found.</exception>
        public string GetResourceContent(Assembly assembly, string fileName);

        /// <summary>
        ///     Determines whether an embedded resource exists within an assembly.
        /// </summary>
        /// <param name="assembly">The assembly to find the resource in.</param>
        /// <param name="fileName">The name of the file to find.</param>
        /// <returns><c>true</c> if the embedded resource is found, <c>false</c> otherwise.</returns>
        public bool ResourceExists(Assembly assembly, string fileName);

        /// <summary>
        ///     Reads the resource, and passes back the output as a raw stream.
        /// </summary>
        /// <param name="assembly">The assembly to load the resource from.</param>
        /// <param name="fileName">Name of the file, embedded within the assembly.</param>
        /// <returns>The contents of the file, as a raw stream.</returns>
        /// <exception cref="FileNotFoundException">Embedded data file not found.</exception>
        public Stream GetResourceStream(Assembly assembly, string fileName);

        /// <summary>
        ///     Disembeds an embedded resource to specified location.
        /// </summary>
        /// <param name="assembly">The assembly to load the resource from.</param>
        /// <param name="resourceName">The manifest name of the resource.</param>
        /// <param name="fileName">The full path to where the file should be copied to.</param>
        public void DisembedResource(Assembly assembly, string resourceName, string fileName);

        /// <summary>
        ///     Disembeds embedded assets from a mod library, into the current mod's assets folder.
        /// </summary>
        /// <param name="assembly">The assembly to load the embedded assets from.</param>
        public void DisembedAssets(Assembly assembly);
    }
}