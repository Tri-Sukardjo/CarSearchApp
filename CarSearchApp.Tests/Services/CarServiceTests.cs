using CarSearchApp.Models;
using CarSearchApp.Data;
using CarSearchApp.Services;
using Microsoft.EntityFrameworkCore;

namespace CarSearchApp.Tests.Services;
public class CarServiceTests
{
    private DbContextOptions<AppDbContext> GetInMemoryDbOptions()
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "CarDbTest")
            .Options;
    }
    private void SeedTestData(AppDbContext context)
    {
        context.Cars.AddRange(new List<Car>
        {
            new Car { Length = 5.0, Weight = 1500, Velocity = 120, Colour = "Red" },
            new Car { Length = 4.5, Weight = 1200, Velocity = 100, Colour = "Blue" },
            new Car { Length = 5.0, Weight = 1600, Velocity = 130, Colour = "Green" },
            new Car { Length = 3.5, Weight = 1000, Velocity = 80,  Colour = "Yellow" },
            new Car { Length = 3.5, Weight = 1500, Velocity = 130, Colour = "Red" },
        });
        context.SaveChanges();
    }

    private void CleanUpTestData(AppDbContext context)
    {
        context.Cars.RemoveRange(context.Cars);
        context.SaveChanges();
    }

    [Fact]
    public void SearchCars_WithValidColour_ReturnsCorrectCars()
    {
        var dbContextOptions = GetInMemoryDbOptions();

        using (var context = new AppDbContext(dbContextOptions))
        {
            SeedTestData(context);

            var criteria = new CarSearchCriteria { Colour = "Red" };

            CarService _carService = new CarService(context);
            var result = _carService.SearchCars(criteria).ToList();

            Assert.Equal(2, result.Count);
            Assert.All(result, car => Assert.Equal("Red", car.Colour));

            CleanUpTestData(context);
        }
    }
    
    [Fact]
    public void SearchCars_WithMultipleCriteria_ReturnsCorrectCars()
    {
        var dbContextOptions = GetInMemoryDbOptions();

        using (var context = new AppDbContext(dbContextOptions))
        {
            SeedTestData(context);

            var service = new CarService(context);
            var criteria = new CarSearchCriteria { Length = 3.5, Colour = "Red" };

            var result = service.SearchCars(criteria).ToList();

            Assert.Single(result);
            Assert.Equal(3.5, result[0].Length);
            Assert.Equal("Red", result[0].Colour);

            CleanUpTestData(context);
        }
    }

    [Fact]
    public void SearchCars_WithAllCriteria_ReturnsCorrectCars()
    {
        var dbContextOptions = GetInMemoryDbOptions();

        using (var context = new AppDbContext(dbContextOptions))
        {
            SeedTestData(context);

            var service = new CarService(context);
            var criteria = new CarSearchCriteria { Length = 3.5, Weight = 1000, Velocity = 80, Colour = "Yellow" };

            var result = service.SearchCars(criteria).ToList();

            Assert.Single(result);
            Assert.Equal(3.5, result[0].Length);
            Assert.Equal(1000, result[0].Weight);
            Assert.Equal(80, result[0].Velocity);
            Assert.Equal("Yellow", result[0].Colour);

            CleanUpTestData(context);
        }
    }

    [Fact]
    public void SearchCars_WithNoMatch_ReturnsEmptyList()
    {
        var dbContextOptions = GetInMemoryDbOptions();

        using (var context = new AppDbContext(dbContextOptions))
        {
            SeedTestData(context);

            var service = new CarService(context);
            var criteria = new CarSearchCriteria { Length = 10.0 };  // No car with this length

            var result = service.SearchCars(criteria).ToList();

            Assert.Empty(result);

            CleanUpTestData(context);
        }
    }

    [Fact]
    public void SearchCars_WithNullCriteria_ReturnsAllCars()
    {
        var dbContextOptions = GetInMemoryDbOptions();

        using (var context = new AppDbContext(dbContextOptions))
        {
            SeedTestData(context);

            var service = new CarService(context);
            var criteria = new CarSearchCriteria();  // No criteria set

            var result = service.SearchCars(criteria).ToList();

            Assert.Equal(5, result.Count);  // All seeded cars should be returned

            CleanUpTestData(context);
        }
    }

    [Fact]
    public void SearchCars_WithCaseInsensitiveColor_ReturnsCorrectCars()
    {
        var dbContextOptions = GetInMemoryDbOptions();

        using (var context = new AppDbContext(dbContextOptions))
        {
            SeedTestData(context);

            var service = new CarService(context);
            var criteria = new CarSearchCriteria { Colour = "rEd" };  // Different case

            var result = service.SearchCars(criteria).ToList();

            Assert.Equal(2, result.Count);
            Assert.All(result, car => Assert.Equal("Red", car.Colour));

            CleanUpTestData(context);
        }
    }

    [Fact]
    public void SearchCars_WithNegativeWeight_ReturnsEmptyList()
    {
        var dbContextOptions = GetInMemoryDbOptions();

        using (var context = new AppDbContext(dbContextOptions))
        {
            SeedTestData(context);

            var service = new CarService(context);
            var criteria = new CarSearchCriteria { Weight = -500 };  // Negative weight

            var result = service.SearchCars(criteria).ToList();

            Assert.Empty(result);

            CleanUpTestData(context);
        }
    }

    [Fact]
    public void SearchCars_WithZeroVelocity_ReturnsNoCars()
    {
        var dbContextOptions = GetInMemoryDbOptions();

        using (var context = new AppDbContext(dbContextOptions))
        {
            SeedTestData(context);

            var service = new CarService(context);
            var criteria = new CarSearchCriteria { Velocity = 0 };

            var result = service.SearchCars(criteria).ToList();

            Assert.Empty(result);

            CleanUpTestData(context);
        }
    }

    [Fact]
    public void SearchCars_WithMultipleMatches_ReturnsAllMatchingCars()
    {
        var dbContextOptions = GetInMemoryDbOptions();


        using (var context = new AppDbContext(dbContextOptions))
        {
            SeedTestData(context);

            var service = new CarService(context);
            var criteria = new CarSearchCriteria { Length = 5.0 };

            var result = service.SearchCars(criteria).ToList();

            Assert.Equal(2, result.Count);
            Assert.All(result, car => Assert.Equal(5.0, car.Length));
        }
    }
    
    [Fact]
    public void ExportToXml_WithCars_ReturnsValidXml()
    {
        var dbContextOptions = GetInMemoryDbOptions();

        var cars = new List<Car>
        {
            new Car { Length = 5.0, Weight = 1500, Velocity = 120, Colour = "Red" },
            new Car { Length = 4.5, Weight = 1200, Velocity = 100, Colour = "Blue" }
        };

        // seed custom test data
        using (var context = new AppDbContext(dbContextOptions))
        {
            context.Cars.AddRange(cars);
            context.SaveChanges();
        }

        using (var context = new AppDbContext(dbContextOptions))
        {
            CarService _carService = new CarService(context);
            var xmlResult = _carService.ExportToXml(cars);

            Assert.NotNull(xmlResult);
            Assert.Contains("<Car>", xmlResult); // Ensure the XML contains <Car> element

            CleanUpTestData(context);
        }
    }

    [Theory]
    [InlineData(5.0d, 1500d, 120d, "Red", "_length5_weight1500_velocity120_Red")]
    [InlineData(null, 1500d, 120d, "Blue", "_nolength_weight1500_velocity120_Blue")]
    [InlineData(4.5d, null, 100d, null, "_length4.5_noweight_velocity100_allColours")]
    [InlineData(null, null, null, "Green", "_nolength_noweight_novelocity_Green")]
    [InlineData(null, null, null, null, "_nolength_noweight_novelocity_allColours")]
    public void ConstructFileName_GeneratesCorrectFileName(
        double? length, 
        double? weight, 
        double? velocity, 
        string colour, 
        string expectedFileNamePostfix)
    {
        var dbContextOptions = GetInMemoryDbOptions();

        using (var context = new AppDbContext(dbContextOptions))
        {
            var criteria = new CarSearchCriteria
            {
                Length = length,
                Weight = weight,
                Velocity = velocity,
                Colour = colour
            };

            CarService _carService = new CarService(context);
            var fileName = _carService.ConstructFileName(criteria);

            Assert.Contains(expectedFileNamePostfix, fileName);

            CleanUpTestData(context);
        }
    }
}
