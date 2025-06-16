using FakeStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FakeStore.Infrastructure.DataAccess;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=tcp:srv-marchioro.database.windows.net,1433;Initial Catalog=testeFakeStore;User ID=Marchioro;Password=Cri$1984;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
    }
}
