﻿using System;

namespace ApacheTech.VintageMods.Core.Hosting.Configuration.DynamicNotifyPropertyChanged
{
    /// <summary>
    ///     Represents a method that will handle an event being raised, when feature settings are changed within a settings file.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of feature object being observed.</typeparam>
    /// <param name="args">The arguments being sent to the handler.</param>
    public delegate void DynamicPropertyChangedEventHandler<T>(DynamicPropertyChangedEventArgs<T> args) where T : class;
}