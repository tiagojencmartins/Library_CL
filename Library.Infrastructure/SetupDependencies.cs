using Library.Infrastructure.Crosscutting.Abstract;
using Library.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure
{
    public static class SetupDependencies
    {
        public static void AddInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddRepository();
            services.AddScoped<IRepositoryService, RepositoryService>();
        }

        private static void AddRepository(this IServiceCollection services)
        {
            services.AddDbContext<LibraryContext>(options =>
            {
                options.UseInMemoryDatabase("Library");
            });

            using var repository = services.BuildServiceProvider().GetService<LibraryContext>();
            repository!.Database.EnsureCreated();
        }
    }
}
