namespace ApacheTech.VintageMods.Core.DependencyInjection.Abstractions
{
    public interface IDependencyResolver
    {
        public TService Resolve<TService>();
        public TService CreateInstance<TService>(params  object[] args) where TService : class;
    }
}