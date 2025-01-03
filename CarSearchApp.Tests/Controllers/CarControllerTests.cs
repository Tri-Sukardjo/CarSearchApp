using Moq;
using Microsoft.AspNetCore.Mvc;
using CarSearchApp.Controllers;
using CarSearchApp.Services;
using CarSearchApp.Models;

namespace CarSearchApp.Tests.Controllers;

public class CarControllerTests
{
    private readonly Mock<ICarService> _mockCarService;
    private readonly CarController _controller;

    public CarControllerTests()
    {
        _mockCarService = new Mock<ICarService>();
        _controller = new CarController(_mockCarService.Object);
    }

    [Fact]
    public void Search_WithValidCriteria_ReturnsViewWithCars()
    {
        // Arrange
        var criteria = new CarSearchCriteria { Colour = "Red" };
        var cars = new List<Car>
        {
            new Car { Length = 5.0, Weight = 1500, Velocity = 120, Colour = "Red" }
        };

        _mockCarService.Setup(service => service.SearchCars(criteria)).Returns(cars.AsQueryable());

        // Act
        var result = _controller.Search(criteria) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(cars, result.Model);
    }

    [Fact]
    public void Export_WithValidCriteria_ReturnsXmlFile()
    {
        // Arrange
        var criteria = new CarSearchCriteria { Colour = "Red" };
        var cars = new List<Car>
        {
            new Car { Length = 5.0, Weight = 1500, Velocity = 120, Colour = "Red" }
        };

        var xmlContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Cars><Car><Length>5.0</Length><Weight>1500</Weight><Velocity>120</Velocity><Color>Red</Color></Car></Cars>";

        _mockCarService.Setup(service => service.SearchCars(criteria)).Returns(cars.AsQueryable());
        _mockCarService.Setup(service => service.ExportToXml(cars)).Returns(xmlContent);

        // Act
        var result = _controller.Export(criteria) as FileContentResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("application/xml", result.ContentType);
        Assert.Equal(xmlContent, System.Text.Encoding.UTF8.GetString(result.FileContents));
    }
}
