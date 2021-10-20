using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Reflection;
using JetBrains.Annotations;

namespace ApacheTech.VintageMods.Core.Services.MefLab
{
    /// <summary>
    ///     Provides MEF Composition Functionality to mods.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public interface IMefLabService
    {
        /// <summary>
        ///     Adds a directory to the aggregate Catalogue.
        /// </summary>
        /// <param name="path">The absolute path to the directory to add.</param>
        /// <returns>The <see cref="DirectoryCatalog"/> instance that was added to the aggregate, so that it can be removed later.</returns>
        DirectoryCatalog AddDirectory(string path);

        /// <summary>
        ///     Removes a directory from the aggregate Catalogue.
        /// </summary>
        /// <param name="path">The absolute path to the directory to remove.</param>
        void RemoveDirectory(string path);

        /// <summary>
        ///     Adds an assembly to the aggregate Catalogue.
        /// </summary>
        /// <param name="assembly">The assembly to add.</param>
        /// <returns>The <see cref="AssemblyCatalog"/> instance that was added to the aggregate, so that it can be removed later.</returns>
        AssemblyCatalog AddAssembly(Assembly assembly);

        /// <summary>
        ///     Removes an assembly from the aggregate Catalogue.
        /// </summary>
        /// <param name="assembly">The assembly to remove.</param>
        void RemoveAssembly(Assembly assembly);

        /// <summary>
        ///     Removes the specified constituent catalogue from the aggregate Catalogue.
        /// </summary>
        /// <typeparam name="TCatalogue">The type of the catalogue to remove.</typeparam>
        /// <param name="catalogue">The constituent catalogue instance that currently resides in the aggregate Catalogue.</param>
        void Remove<TCatalogue>(TCatalogue catalogue) where TCatalogue : ComposablePartCatalog;

        /// <summary>
        ///     Creates composable parts from an array of attributed objects.
        /// </summary>
        /// <param name="attributedParts">An array of attributed parts to compose.</param>
        void ComposeParts(params object[] attributedParts);
    }
}