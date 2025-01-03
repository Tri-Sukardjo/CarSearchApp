using CarSearchApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CarSearchApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Car> Cars { get; set; }
}
