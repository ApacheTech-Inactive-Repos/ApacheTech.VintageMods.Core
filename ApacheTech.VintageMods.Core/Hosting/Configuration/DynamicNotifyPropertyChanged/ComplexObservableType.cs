using System;
using System.Reflection;
using ApacheTech.Common.Extensions.Harmony;

// ReSharper disable UnusedType.Global
// ReSharper disable PublicConstructorInAbstractClass
// ReSharper disable UnusedAutoPropertyAccessor.Local

#pragma warning disable IDE0052 // Remove unread private members

namespace ApacheTech.VintageMods.Core.Hosting.Configuration.DynamicNotifyPropertyChanged
{
    public abstract class ComplexObservableType<T> where T: class
    {
        private double Check { get; set; }

        public ComplexObservableType()
        {
            // NOTE:    A first attempt to solve the problem of complex types not firing the DynamicNotifyPropertyChanged
            //          Event. With this attempt, we set up a dummy property within the class, then wrap each of the properties
            //          that match the generic constrain, and observe their property changes. When something changes, we set
            //          the value of the Check property, which, in turn, fires the PropertyChanged event for the derived class.
            foreach (var propertyInfo in GetType().GetProperties(BindingFlags.Public))
            {
                if (propertyInfo.PropertyType is not T) return;
                var observedProperty = DynamicNotifyPropertyChanged<T>.Bind(this.GetProperty<T>(propertyInfo.Name));
                observedProperty.PropertyChanged += PropertyChanged;
            }
        }

        private void PropertyChanged(DynamicPropertyChangedEventArgs<T> args)
        {
            Check = new Random().NextDouble();
        }
    }
}