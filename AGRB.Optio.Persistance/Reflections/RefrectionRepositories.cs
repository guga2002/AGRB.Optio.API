using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace RGBA.Optio.UI.Reflections
{
    public static  class RefrectionRepositories
    {
        public static void AddInjectRepositories(this IServiceCollection collection, Assembly assembly, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            if(assembly is null )
            {
                throw new ArgumentNullException(nameof(assembly), "Assembly cannot be null");
            }
            var types = assembly.GetTypes().Where(i =>
            i is { IsInterface: false, IsAbstract: false, IsGenericTypeDefinition: false } &&
            i.Name.Contains("Repos",StringComparison.OrdinalIgnoreCase)
            );

            foreach( var type in types)
            {
                var interfaces = type.GetInterfaces().ToList();
                if(interfaces.Count == 0)
                {
                    continue;
                }
                foreach (var item in interfaces)
                {
                    collection.AddScoped(item, type);
                }
            }
        }
    }
}
