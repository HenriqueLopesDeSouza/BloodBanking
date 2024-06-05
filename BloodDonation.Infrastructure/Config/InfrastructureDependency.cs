using BloodBanking.Core.Repositories;
using BloodBanking.Infrastructure.Persistence.Repositories;
using BloodBanking.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BloodBanking.Core.DomainEvents;
using MediatR;


namespace BloodBanking.Infrastructure.Config
{
    public static class InfrastructureDependency
    {
        public static IServiceCollection AddContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BloodDonationDbContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("BloodDonationDb"));
            });
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {

            services.AddScoped<IDonationRepository, DonationRepository>();
            services.AddScoped<IDonorRepository, DonorRepository>();
            services.AddScoped<IBloodStockRepository, BloodStockRepository>();
            return services;
        }
    }
}
