using Microsoft.Extensions.DependencyInjection;
using Service.IServices;
using Service.Services;



namespace Service
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUserService, UserService>();
            

        }
    }
}
