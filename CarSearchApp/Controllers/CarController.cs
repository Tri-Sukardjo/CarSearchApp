using CarSearchApp.Models;
using CarSearchApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarSearchApp.Controllers;

public class CarController : Controller
{
    private readonly ICarService _carService;

    public CarController(ICarService carService)
    {
        _carService = carService;
    }

    public IActionResult Search(CarSearchCriteria criteria)
    {
        var cars = _carService.SearchCars(criteria).ToList();
        return View(cars);
    }

    public IActionResult Export(CarSearchCriteria criteria)
    {
        var cars = _carService.SearchCars(criteria).ToList();
        var xml = _carService.ExportToXml(cars);
        var fileName = _carService.ConstructFileName(criteria);
        return File(new System.Text.UTF8Encoding().GetBytes(xml), "application/xml", fileName);
    }
}

