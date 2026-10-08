using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
namespace PersonalFinanceMng.Infrastructure.Database.Context
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Mapeamentos de configurações fluentes (Fluent API)
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
