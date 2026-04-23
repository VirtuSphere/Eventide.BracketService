using Eventide.BracketService.Domain.Interfaces;
using Eventide.BracketService.Infrastructure.Data;
using Eventide.BracketService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eventide.BracketService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<BracketDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("BracketDb")));

        services.AddScoped<IBracketRepository, BracketRepository>();
        return services;
    }
}