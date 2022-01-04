using System;
using ApacheTech.Common.DependencyInjection.Annotation;
using ApacheTech.VintageMods.Core.Abstractions.Contracts;
using Vintagestory.API.Common;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Annotation
{
    /// <summary>
    /// Marks the constructor to be used when activating type using <see cref="ActivatorEx" />.
    /// </summary>
    // Token: 0x02000019 RID: 25
    [AttributeUsage(AttributeTargets.Constructor)]
    public class SidedConstructorAttribute : ServiceProviderConstructorAttribute, IApiSided
    {
        /// <summary>
        ///     The app-side that this instance was instantiated on.
        /// </summary>
        /// <value>The side.</value>
        public EnumAppSide Side { get; }

        /// <summary>
        /// 	Initialises a new instance of the <see cref="SidedConstructorAttribute"/> class.
        /// </summary>
        public SidedConstructorAttribute() : this(EnumAppSide.Universal)
        {
        }

        /// <summary>
        /// 	Initialises a new instance of the <see cref="SidedConstructorAttribute"/> class.
        /// </summary>
        /// <param name="side">The side.</param>
        public SidedConstructorAttribute(EnumAppSide side)
        {
            Side = side;
        }
    }
}