using ApacheTech.VintageMods.Core.Services;

// ReSharper disable UnusedMember.Global
// ReSharper disable StaticMemberInGenericType
// ReSharper disable MemberCanBePrivate.Global

namespace ApacheTech.VintageMods.Core.Abstractions.Features
{
    public abstract class SettingsConsumer<T> where T : FeatureSettings
    {
        public static T Settings { get; set; } = ModServices.IOC.Resolve<T>();

        public static string FeatureName { get; set; } = (typeof(T).Name).Replace("Settings", "");

        /// <summary>
        /// 	Initialises static members of the <see cref="SettingsConsumer{T}"/> class.
        /// </summary>
        static SettingsConsumer()
        {
            Initialise();
        }

        /// <summary>
        /// 	Initialises settings for the <see cref="SettingsConsumer{T}"/> class.
        /// </summary>
        public static void Initialise()
        {
            FeatureName = (typeof(T).Name).Replace("Settings", "");
            Settings = ModServices.IOC.Resolve<T>();
        }

        /// <summary>
        ///     Saves any changes to the mod settings file.
        /// </summary>
        protected abstract void SaveChanges();

        /// <summary>
        ///     Disposes this instance.
        /// </summary>
        public static void Dispose()
        {
            Settings = null;
            FeatureName = null;
        }
    }
}