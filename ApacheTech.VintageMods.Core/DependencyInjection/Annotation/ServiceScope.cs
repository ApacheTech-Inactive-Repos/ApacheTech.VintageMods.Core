namespace ApacheTech.VintageMods.Core.DependencyInjection.Annotation
{
    /// <summary>
    ///     Determines the scope of the service, when it is registered within the IOC Container.
    /// </summary>
    public enum ServiceScope
    {
        /// <summary>
        ///     Singleton: Single instance, throughout the lifetime of the mod.
        /// </summary>
        Singleton,

        /// <summary>
        ///     Scoped: ModSystems can set their own scope, and derive instances that last as long as is needed.
        /// </summary>
        Scoped,

        /// <summary>
        ///     Transient: A new instance is created whenever the service is called.
        /// </summary>
        Transient
    }
}
