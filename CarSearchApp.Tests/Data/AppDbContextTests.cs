using CarSearchApp.Data;
using Microsoft.EntityFrameworkCore;
using CarSearchApp.Models;

namespace CarSearchApp.Tests.Data;

public class AppDbContextTests
{
    private readonly AppDbContext _context;

    public AppDbContextTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "CarSearchTestDb")
            .Options;

        _context = new AppDbContext(options);
    }

    [Fact]
    public async Task CanAddAndRetrieveCar()
    {
        // Arrange
        var car = new Car { Length = 5.0, Weight = 1500, Velocity = 120, Colour = "Red" };

        // Act
        await _context.Cars.AddAsync(car);
        await _context.SaveChangesAsync();

        var retrievedCar = await _context.Cars.FirstOrDefaultAsync(c => c.Colour == "Red");

        // Assert
        Assert.NotNull(retrievedCar);
        Assert.Equal("Red", retrievedCar.Colour);
    }
}

