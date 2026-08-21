using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Contracts;

namespace Web.Infrastructure.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureDbContext(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")
                ));
        }

        public static void ConfigureRepositoryRegistration(
            this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }
    }
}