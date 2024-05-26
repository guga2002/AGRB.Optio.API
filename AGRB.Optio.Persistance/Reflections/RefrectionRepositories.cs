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
            var assemb = assembly.GetTypes().Where(i =>
            !i.IsInterface&&
            !i.IsAbstract &&
            !i.IsGenericTypeDefinition &&
            i.Name.Contains("Repos",StringComparison.OrdinalIgnoreCase)
            );

            foreach( var type in assemb )
            {
                var interfaceRepository = type.GetInterfaces();
                if(interfaceRepository.Count()==0)
                {
                    continue;
                }
                foreach (var item in interfaceRepository)
                {
                    collection.AddScoped(item, type);
                }
            }
        }
    }
}
