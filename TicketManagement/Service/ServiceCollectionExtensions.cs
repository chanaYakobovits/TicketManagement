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
            serviceCollection.AddScoped<JwtService>();
            serviceCollection.AddScoped<ITicketService, TicketService>();
            serviceCollection.AddScoped<IAIService, AIService>();
            serviceCollection.AddHttpClient<AIService>();

        }
    }
}
