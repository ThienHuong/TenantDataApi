using Microsoft.EntityFrameworkCore;
using SolverApi.Models;

namespace SolverApi.DataAccess
{
    public class GlobalDbContext : DbContext
    {
        public DbSet<Tenant> Tenants { get; set; }

        public GlobalDbContext(DbContextOptions<GlobalDbContext> options) : base(options)
        {
        }
    }
}
