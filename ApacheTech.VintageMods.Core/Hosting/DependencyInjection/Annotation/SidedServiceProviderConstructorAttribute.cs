using System;
using ApacheTech.Common.DependencyInjection.Annotation;
using ApacheTech.VintageMods.Core.Abstractions.Contracts;
using JetBrains.Annotations;
using Vintagestory.API.Common;

namespace ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Annotation
{
    /// <summary>
    /// Marks the constructor to be used when activating type using <see cref="ActivatorEx" />.
    /// </summary>
    // Token: 0x02000019 RID: 25
    [AttributeUsage(AttributeTargets.Constructor)]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class SidedServiceProviderConstructorAttribute : ServiceProviderConstructorAttribute, IApiSided
    {
        /// <summary>
        ///     The app-side that this instance was instantiated on.
        /// </summary>
        /// <value>The side.</value>
        public EnumAppSide Side { get; }

        /// <summary>
        /// 	Initialises a new instance of the <see cref="SidedServiceProviderConstructorAttribute"/> class.
        /// </summary>
        public SidedServiceProviderConstructorAttribute() : this(EnumAppSide.Universal)
        {
        }

        /// <summary>
        /// 	Initialises a new instance of the <see cref="SidedServiceProviderConstructorAttribute"/> class.
        /// </summary>
        /// <param name="side">The side.</param>
        public SidedServiceProviderConstructorAttribute(EnumAppSide side)
        {
            Side = side;
        }
    }
}