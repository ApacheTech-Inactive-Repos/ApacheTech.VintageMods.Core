using System;
using ApacheTech.VintageMods.Core.Hosting.Configuration;
using ApacheTech.VintageMods.Core.Services;

// ReSharper disable UnusedMember.Global
// ReSharper disable StaticMemberInGenericType
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Abstractions.Features
{
    /// <summary>
    ///     Represents a class that affects, or is affected by specific feature settings.
    /// </summary>
    /// <typeparam name="T">The settings file to use within the patches in this class.</typeparam>
    public abstract class WorldSettingsConsumer<T> : IDisposable where T : class, new()
    {
        protected static T Settings { get; set; }

        protected static string FeatureName { get; set; }

        /// <summary>
        /// 	Initialises static members of the <see cref="WorldSettingsConsumer{T}"/> class.
        /// </summary>
        static WorldSettingsConsumer()
        {
            FeatureName = (typeof(T).Name).Replace("Settings", "");
            Settings = ModServices.IOC.Resolve<T>();
        }

        /// <summary>
        /// 	Initialises a new instance of the <see cref="WorldSettingsConsumer{T}" /> class.
        /// </summary>
        protected WorldSettingsConsumer()
        {
            FeatureName = (typeof(T).Name).Replace("Settings", "");
            Settings = ModServices.IOC.Resolve<T>();
        }

        /// <summary>
        ///     Saves any changes to the mod settings file.
        /// </summary>
        protected void SaveChanges()
        {
            ModSettings.World.Save(Settings);
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            FeatureName = null;
            Settings = null;
        }
    }
}