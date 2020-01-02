using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace DDD.School.API.Extensions
{

    public static class ServiceCollectionExtensions
    {
        // https://stackoverflow.com/questions/49087739/net-core-register-raw-generic-with-different-number-of-parameters
        public static IServiceCollection RegisterAllTypes(this IServiceCollection serviceCollection, 
            Type interfaceType,
            Assembly[] assemblies,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            var scannedTypes = assemblies.SelectMany(x => x.GetTypes().Where(t => t.IsClass && !t.IsAbstract))
                                         .ToArray();
            foreach (var type in scannedTypes)
            {
                foreach (var i in type.GetInterfaces())
                {
                    // Check for generic
                    if (i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType)
                    {
                        var genericInterfaceType = interfaceType.MakeGenericType(i.GetGenericArguments());
                        serviceCollection.Add(new ServiceDescriptor(genericInterfaceType, type, lifetime));
                    }
                    // Check for non-generic
                    else if (!i.IsGenericType && i == interfaceType)
                    {
                        serviceCollection.Add(new ServiceDescriptor(interfaceType, type, lifetime));
                    }
                }
            }

            return serviceCollection;
        }
    }
}
