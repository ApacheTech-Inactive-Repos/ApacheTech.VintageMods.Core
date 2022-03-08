using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// ReSharper disable UnusedMember.Global

namespace ApacheTech.VintageMods.Core.Extensions.DotNet
{
    public static class AssemblyExtensions
    {
        /// <summary>
        ///     Nullifies any orphaned static members within a given assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public static void NullifyOrphanedStaticMembers(this Assembly assembly)
        {
            if (assembly is null) return;
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
        }

        public static IEnumerable<Type> GetAllTypesImplementingOpenGenericType(this Assembly assembly, Type openGenericType)
        {
            return 
                from x in assembly.GetTypes() 
                from z in x.GetInterfaces() 
                let y = x.BaseType 
                where y is { IsGenericType: true } 
                    && openGenericType.IsAssignableFrom(y.GetGenericTypeDefinition()) || z.IsGenericType 
                    && openGenericType.IsAssignableFrom(z.GetGenericTypeDefinition()) 
                select x;
        }
    }
}