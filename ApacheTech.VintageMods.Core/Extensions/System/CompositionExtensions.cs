using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.ReflectionModel;
using System.Linq;
using System.Reflection;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;

namespace ApacheTech.VintageMods.Core.Extensions.System
{
    public static class CompositionExtensions
    {
        #region MEF Composition

        /// <summary>
        ///     Gets accessible assemblies with any composable exports.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>A collection of assemblies that contain one or more composable exports.</returns>
        public static IEnumerable<Assembly> GetAssembliesWithExports(this CompositionContainer container)
        {
            return container.Catalog?.Parts
                .Select(part => ReflectionModelServices.GetPartType(part).Value.Assembly)
                .Distinct()
                .ToList();
        }

        /// <summary>
        ///     Composes a batch of attributed parts, separate to to whatever else is in the container at the time.
        /// </summary>
        /// <param name="container">The container used to resolve exported parts.</param>
        /// <param name="parts">The parts required for import.</param>
        public static void ComposeBatch(this CompositionContainer container, params object[] parts)
        {
            try
            {
                var batch = new CompositionBatch();
                foreach (var o in parts) batch.AddPart(o);
                container.Compose(batch);
            }
            catch (ArgumentNullException ex)
            {
                ApiEx.Current.Logger.Error($"{ex}");
                throw;
            }
        }

        #endregion
    }
}