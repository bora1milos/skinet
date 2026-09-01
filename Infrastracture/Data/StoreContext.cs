using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.Data;

public class StoreContext(DbContextOptions<StoreContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new Infrastracture.Config.ProductConfiguration());
    }
}
