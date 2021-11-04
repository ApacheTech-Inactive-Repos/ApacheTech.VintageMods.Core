using System;

namespace ApacheTech.VintageMods.Core.Hosting.DependencyInjection
{
    /// <summary>
    /// Marks the constructor to be used when activating type using <see cref="ActivatorUtilities" />.
    /// </summary>
    // Token: 0x02000019 RID: 25
    [AttributeUsage(AttributeTargets.All)]
    public class ActivatorUtilitiesConstructorAttribute : Attribute
    {
    }
}