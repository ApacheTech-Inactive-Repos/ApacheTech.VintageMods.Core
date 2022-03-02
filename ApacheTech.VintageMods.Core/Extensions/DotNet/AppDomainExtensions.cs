using System;
using System.Linq;
using System.Reflection;

namespace ApacheTech.VintageMods.Core.Extensions.DotNet
{
    public static class AppDomainExtensions
    {
        public static Assembly GetAssemblyByName(this AppDomain domain, string assemblyName)
        {
            var assemblies = domain.GetAssemblies();
            return assemblies.FirstOrDefault(a => a.GetName().Name == assemblyName);
        }
    }
}