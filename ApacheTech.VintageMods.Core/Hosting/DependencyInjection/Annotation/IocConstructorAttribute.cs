using System;
using Vintagestory.API.Common;

namespace ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Annotation
{
    /// <summary>
    /// Marks the constructor to be used when activating type using <see cref="ActivatorUtilities" />.
    /// </summary>
    // Token: 0x02000019 RID: 25
    [AttributeUsage(AttributeTargets.All)]
    public class IocConstructorAttribute : Attribute
    {
        public EnumAppSide Side { get; }

        public IocConstructorAttribute() : this(EnumAppSide.Universal)
        {
        }

        public IocConstructorAttribute(EnumAppSide side)
        {
            Side = side;
        }
    }
}