using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace ApacheTech.VintageMods.Core.DependencyInjection.Annotation
{
    /// <summary>
    ///     Denotes that this class should be registered within the IOC container, when the mod is launched.
    /// </summary>
    /// <remarks>
    ///     If no specific type is supplied, the class will be registered via convention:<br/><br/>
    ///
    ///      • If the class implements an interface, the interface will be be used as the representation.<br/>
    ///      • If the class implements more than one interface, the first interface will be be used as the representation.<br/>
    ///      • If the class does not implement an interface, it will be registered as itself.
    /// </remarks>
    /// <seealso cref="Attribute" />
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [AttributeUsage(AttributeTargets.Class)]
    public class RegisteredServiceAttribute : Attribute
    {
        /// <summary>
        ///     Gets the scope of the service, be it Singleton, Scoped, or Transient.
        /// </summary>
        /// <value>The service scope.</value>
        internal ServiceLifetime ServiceScope { get; }

        /// <summary>
        ///     Gets the type of the registered.
        /// </summary>
        /// <value>The type of the registered.</value>
        internal Type ServiceType { get; }

        /// <summary>
        /// 	Initialises a new instance of the <see cref="RegisteredServiceAttribute"/> class.
        /// </summary>
        /// <param name="serviceScope">The service scope.</param>
        /// <param name="registeredType">The type of the representation of the registered class within the IOC Container.</param>
        /// <remarks>
        ///     If no specific type is supplied, the class will be registered via convention:<br/><br/>
        ///
        ///      • If the class implements an interface, the interface will be be used as the representation.<br/>
        ///      • If the class implements more than one interface, the first interface will be be used as the representation.<br/>
        ///      • If the class does not implement an interface, it will be registered as itself.
        /// </remarks>
        public RegisteredServiceAttribute(ServiceLifetime serviceScope, Type serviceType)
        {
            ServiceScope = serviceScope;
            ServiceType = serviceType;
        }
    }
}