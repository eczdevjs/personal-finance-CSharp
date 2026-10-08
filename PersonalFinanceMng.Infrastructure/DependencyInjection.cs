using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using PersonalFinanceMng.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;


namespace PersonalFinanceMng.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // aqui sera registrado o entity framework/dapper repositorios e servicos externos 
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("A string de conexão 'DefaultConnection' não foi encontrada no appsettings.json!");
            }
            //services.AddDbContext<AppDbContext>();
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

            return services;
        }
    }
}
