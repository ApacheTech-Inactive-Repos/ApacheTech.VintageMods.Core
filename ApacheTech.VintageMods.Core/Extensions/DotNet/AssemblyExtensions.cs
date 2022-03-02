using System.Reflection;
using System.Threading.Tasks;

namespace ApacheTech.VintageMods.Core.Extensions.DotNet
{
    public static class AssemblyExtensions
    {
        public static void NullifyOrphanedStaticMembers(this Assembly assembly)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (var type in assembly.GetTypes())
                {

                    foreach (var propertyInfo in type.GetProperties(BindingFlags.Static | BindingFlags.SetProperty))
                    {
                        if (!propertyInfo.CanWrite) continue;
                        propertyInfo.SetMethod.Invoke(null, null);
                    }

                    foreach (var fieldInfo in type.GetFields(BindingFlags.Static | BindingFlags.SetField))
                    {
                        if (fieldInfo.Attributes == FieldAttributes.InitOnly) continue;
                        fieldInfo.SetValue(null, null);
                    }
                }
            });
        }
    }
}