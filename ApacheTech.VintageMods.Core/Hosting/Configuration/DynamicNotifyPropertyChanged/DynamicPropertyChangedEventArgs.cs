﻿using System;
using ApacheTech.VintageMods.Core.Hosting.Configuration.ObservableFeatures;

namespace ApacheTech.VintageMods.Core.Hosting.Configuration.DynamicNotifyPropertyChanged
{
    /// <summary>
    ///     Contains data pertaining to an <see cref="ObservableFeature{T}"/>, as an observed property changes.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of the strongly-typed POCO settings class.</typeparam>
    /// <seealso cref="EventArgs" />
    public class DynamicPropertyChangedEventArgs<T> : EventArgs
    {
        /// <summary>
        /// 	Initialises a new instance of the <see cref="FeatureSettingsChangedEventArgs{T}"/> class.
        /// </summary>
        /// <param name="propertyName">The name of the property being changed.</param>
        /// <param name="instance">The instance of the <see cref="T"/> object being observed.</param>
        public DynamicPropertyChangedEventArgs(T instance, string propertyName)
        {
            Instance = instance;
            PropertyName = propertyName;
        }

        /// <summary>
        ///     Gets the name of the property being changed.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName { get; }

        /// <summary>
        ///     Gets the instance of the <see cref="T"/> object being observed.
        /// </summary>
        /// <value>An instance of type <see cref="T"/>.</value>
        public T Instance { get; }
    }
}