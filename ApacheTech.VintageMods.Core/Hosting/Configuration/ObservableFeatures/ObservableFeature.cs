using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ApacheTech.VintageMods.Core.Common.Extensions.System;
using HarmonyLib;

#region Analyser Cleanup

#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable IDE0060 // Remove unused parameter

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local

#endregion

namespace ApacheTech.VintageMods.Core.Hosting.Configuration.ObservableFeatures
{
    /// <summary>
    ///     Notifies observers that a property value has changed within a wrapped POCO class.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of object to watch.</typeparam>
    /// <seealso cref="IDisposable" />
    public class ObservableFeature<T> : IDisposable
    {
        private static ObservableFeature<T> _instance;

        private readonly Harmony _harmony;
        private readonly string _featureName;
        private readonly T _featureInstance;

        /// <summary>
        /// 	Initialises a new instance of the <see cref="ObservableFeature{T}" /> class.
        /// </summary>
        /// <param name="featureName">The name of the feature to be observed</param>
        /// <param name="instance">The instance to watch.</param>
        private ObservableFeature(string featureName, T instance)
        {
            _featureInstance = instance;
            _featureName = featureName;
            var objectType = instance.GetType();
            _harmony = new Harmony(objectType.FullName);
            foreach (var propertyInfo in objectType.GetProperties())
            {
                var original = propertyInfo.SetMethod;
                var postfix = this.GetMethod("Patch_PropertySetMethod_Postfix");
                _harmony.Patch(original, postfix: new HarmonyMethod(postfix));
            }
        }

        /// <summary>
        ///     Binds the specified feature to a POCO class object; dynamically adding an implementation of <see cref="INotifyPropertyChanged"/>, 
        ///     raising an event every time a property within the POCO class, is set.
        /// </summary>
        /// <param name="featureName">The name of the feature being observed.</param>
        /// <param name="instance">The instance of the POCO class that manages the feature settings.</param>
        /// <returns>An instance of <see cref="ObservableFeature{T}"/>, which exposes the <c>PropertyChanged</c> event.</returns>
        public static ObservableFeature<T> Bind(string featureName, T instance)
        {
            return _instance ??= new ObservableFeature<T>(featureName, instance);
        }

        /// <summary>
        ///     Occurs when a property value is changed, within the observed POCO class.
        /// </summary>
        public event FeatureSettingsChangedEventHandler<T> PropertyChanged;

        /// <summary>
        ///     Un-patches the dynamic property watch on the POCO class. 
        /// </summary>
        public void Dispose()
        {
            _harmony.UnpatchAll();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Patch_PropertySetMethod_Postfix(object __instance)
        {
            var args = new FeatureSettingsChangedEventArgs<T>(
                _instance._featureName, 
                _instance._featureInstance);

            _instance.PropertyChanged?.Invoke(args);
        }
    }
}