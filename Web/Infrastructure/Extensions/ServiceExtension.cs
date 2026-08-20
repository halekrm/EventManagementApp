using Microsoft.EntityFrameworkCore;
using Repositories;

namespace Web.Infrastructure.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(options=>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")
            ));
        }
    }
}