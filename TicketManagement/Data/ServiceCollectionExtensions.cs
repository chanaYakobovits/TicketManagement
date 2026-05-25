
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;

namespace Data
{
    public static class ServiceCollectionExtensions
    {


        public static void RegisterDB(this IServiceCollection serviceCollection)

        {

            serviceCollection.AddDbContext<AppDbContext>();

            serviceCollection.AddScoped<BaseRepository<Aianalysis>>();

            serviceCollection.AddScoped<BaseRepository<Attachment>>();

            serviceCollection.AddScoped<BaseRepository<Department>>();

            serviceCollection.AddScoped<BaseRepository<Message>>();

            serviceCollection.AddScoped<BaseRepository<Sla>>();

            serviceCollection.AddScoped<BaseRepository<Ticket>>();

            serviceCollection.AddScoped<BaseRepository<Category>>();

            serviceCollection.AddScoped<BaseRepository<TicketAssignment>>();

            serviceCollection.AddScoped<BaseRepository<TicketClosure>>();

            serviceCollection.AddScoped<BaseRepository<TicketStatusHistory>>();

            serviceCollection.AddScoped<BaseRepository<User>>();

            serviceCollection.AddScoped<BaseRepository<UserType>>();
            serviceCollection.AddScoped<BaseRepository<TicketStatusHistory>>();
            serviceCollection.AddScoped<BaseRepository<Message>>();

        }
    }
}
