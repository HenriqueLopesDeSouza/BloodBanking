using BloodBanking.Application.EventHandlers;
using BloodBanking.Application.IService;
using BloodBanking.Application.Service;
using BloodBanking.Core.DomainEvents;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BloodBanking.Application.Config
{
    public static class ApplicationDependency
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IDonorService, DonorService>();
            services.AddScoped<IDonationService, DonationService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IReportService, ReportService>();
            return services;
        }

        public static IServiceCollection AddTransient(this IServiceCollection services)
        {
            services.AddTransient<INotificationHandler<LowBloodStockEvent>, LowBloodStockEventHandler>();
            return services;
        }
    }
}
