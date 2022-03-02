using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using ApacheTech.Common.Extensions.Harmony;
using ApacheTech.VintageMods.Core.Hosting.Configuration.ObservableFeatures;
using HarmonyLib;
using SmartAssembly.Attributes;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local

namespace ApacheTech.VintageMods.Core.Hosting.Configuration.DynamicNotifyPropertyChanged
{
    /// <summary>
    ///     Notifies observers that a property value has changed within a wrapped POCO class.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of object to watch.</typeparam>
    /// <seealso cref="IDisposable" />
    [DoNotPruneType]
    public class DynamicNotifyPropertyChanged<T> : IDisposable where T : class
    {
        private static DynamicNotifyPropertyChanged<T> _instance;
        private static T _observedInstance;

        private readonly Harmony _harmony;

        /// <summary>
        /// 	Initialises a new instance of the <see cref="DynamicNotifyPropertyChanged{T}" /> class.
        /// </summary>
        /// <param name="instance">The instance to watch.</param>
        private DynamicNotifyPropertyChanged(T instance)
        {
            _observedInstance = instance;
            var objectType = instance.GetType();
            _harmony = new Harmony(objectType.FullName);
            var postfix = this.GetMethod("Patch_PropertySetMethod_Postfix");
            foreach (var propertyInfo in objectType.GetProperties())
            {
                _harmony.Patch(propertyInfo.SetMethod, postfix: new HarmonyMethod(postfix));
            }
        }

        /// <summary>
        ///     Binds the specified feature to a POCO class object; dynamically adding an implementation of <see cref="INotifyPropertyChanged"/>, 
        ///     raising an event every time a property within the POCO class, is set.
        /// </summary>
        /// <param name="instance">The instance of the POCO class that manages the feature settings.</param>
        /// <returns>An instance of <see cref="ObservableFeature{T}"/>, which exposes the <c>PropertyChanged</c> event.</returns>
        public static DynamicNotifyPropertyChanged<T> Bind(T instance)
        {
            return _instance ??= new DynamicNotifyPropertyChanged<T>(instance);
        }

        /// <summary>
        ///     Occurs when a property value is changed, within the observed POCO class.
        /// </summary>
        public event DynamicPropertyChangedEventHandler<T> PropertyChanged;

        /// <summary>
        ///     Un-patches the dynamic property watch on the POCO class. 
        /// </summary>
        public void Dispose()
        {
            _harmony.UnpatchAll();
            _instance = null;
            _observedInstance = null;
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Harmony Injected Method")]
        private static void Patch_PropertySetMethod_Postfix(MemberInfo __originalMethod)
        {
            var propertyName = __originalMethod.Name.Remove(0, 4);
            var args = new DynamicPropertyChangedEventArgs<T>(_observedInstance, propertyName);
            _instance.PropertyChanged?.Invoke(args);
        }
    }
}