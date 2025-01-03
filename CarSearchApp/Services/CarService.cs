using CarSearchApp.Data;
using CarSearchApp.Models;
using System.Xml.Serialization;

namespace CarSearchApp.Services;

public class CarService : ICarService
{
    private readonly AppDbContext _context;

    public CarService(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<Car> SearchCars(CarSearchCriteria criteria)
    {
        var query = _context.Cars.AsQueryable();

        if (criteria.Length.HasValue)
            query = query.Where(c => c.Length == criteria.Length);
        if (criteria.Weight.HasValue)
            query = query.Where(c => c.Weight == criteria.Weight);
        if (criteria.Velocity.HasValue)
            query = query.Where(c => c.Velocity == criteria.Velocity);
        if (!string.IsNullOrEmpty(criteria.Colour))
            query = query.Where(c => c.Colour.ToLower() == criteria.Colour.ToLower());

        return query;
    }

    public string ExportToXml(List<Car> cars)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<Car>));
        using (StringWriter writer = new StringWriter())
        {
            serializer.Serialize(writer, cars);
            return writer.ToString();
        }
    }

    public string ConstructFileName(CarSearchCriteria criteria)
    {
        string lengthPart = criteria.Length.HasValue ? $"length{criteria.Length}" : "nolength";
        string weightPart = criteria.Weight.HasValue ? $"weight{criteria.Weight}" : "noweight";
        string velocityPart = criteria.Velocity.HasValue ? $"velocity{criteria.Velocity}" : "novelocity";
        string ColourPart = !string.IsNullOrEmpty(criteria.Colour) ? criteria.Colour : "allColours";
        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

        return $"cars_{timestamp}_{lengthPart}_{weightPart}_{velocityPart}_{ColourPart}.xml";
    }
}