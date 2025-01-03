using CarSearchApp.Models;

namespace CarSearchApp.Services;

public interface ICarService
{
    public IQueryable<Car> SearchCars(CarSearchCriteria criteria);
    public string ExportToXml(List<Car> cars);
    public string ConstructFileName(CarSearchCriteria criteria);

}
