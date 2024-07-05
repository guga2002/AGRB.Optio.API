using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace AGRB.Optio.Persistance.Reflections
{
    public static class ReflectionRepositories
    {
        public static void AddInjectRepositories(this IServiceCollection collection, Assembly interfaceAssembly, Assembly implementationAssembly, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            if (interfaceAssembly is null)
            {
                throw new ArgumentNullException(nameof(interfaceAssembly), "Interface assembly cannot be null");
            }

            if (implementationAssembly is null)
            {
                throw new ArgumentNullException(nameof(implementationAssembly), "Implementation assembly cannot be null");
            }

            var interfaceTypes = interfaceAssembly.GetTypes().Where(i =>
                i.IsInterface && i.Name.Contains("Repos", StringComparison.OrdinalIgnoreCase)
            ).ToList();

            var implementationTypes = implementationAssembly.GetTypes().Where(i =>
                !i.IsInterface && !i.IsAbstract && !i.IsGenericTypeDefinition && i.Name.Contains("Repos", StringComparison.OrdinalIgnoreCase)
            ).ToList();

            foreach (var interfaceType in interfaceTypes)
            {
                var implementationType = implementationTypes.FirstOrDefault(impl => interfaceType.IsAssignableFrom(impl));
                if (implementationType != null)
                {
                    collection.Add(new ServiceDescriptor(interfaceType, implementationType, serviceLifetime));
                }
            }
        }
    }
}