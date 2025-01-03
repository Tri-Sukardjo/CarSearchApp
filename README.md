# CarSearchApp

## Project Overview
CarSearchApp is a simple .NET Core web application that allows users to search for cars based on specific criteria such as length, weight, velocity, and color. The application provides the ability to export search results to XML format.

### Key Features
- Search for cars by multiple criteria.
- Export search results to XML.
- ASP.NET Core MVC architecture.
- Entity Framework Core for data access.
- Unit Test using xUnit, Moq, and InMemoryDatabase


---

## Project Structure
```
CarSearchApp/
|-- CarSearchApp.sln
|-- Controllers/
|   |-- CarController.cs
|-- Data/
|   |-- AppDbContext.cs
|-- Models/
|   |-- Car.cs
|   |-- CarSearchCriteria.cs
|   |-- ErrorViewModel.cs
|-- Services/
|   |-- CarService.cs
|   |-- ICarService.cs
|-- Views/
|   |-- Car/
|   |   |-- Search.cshtml
|-- Program.cs
|-- appsettings.json
CarSearchApp.Tests/
|-- Controllers/
|   |-- CarControllerTests.cs
|-- Data/
|   |-- AppDbContextTests.cs
|-- Services/
|   |-- CarServiceTests.cs

```

---
## Testing
Unit tests are available in the `CarSearchApp.Tests/` directory. The tests use `xUnit`, `Moq`, and `InMemoryDatabase` to prove the implementation meet the requirement and to ensure we can prove software quality and execution.

---
## Screenshoot Examples
### Search Screen without any filters:

![image](https://github.com/user-attachments/assets/53c0e9be-d608-4451-b93a-748df69ba98a)

### Search Screen with filtered result:

![image](https://github.com/user-attachments/assets/fef301f2-dd83-4982-b841-6023fe6934f4)

---

## Prerequisites
- .NET 6.0 SDK or later
- SQL Server (local or remote instance)
- Visual Studio (or any preferred IDE)
- Git (optional, for version control)

---

## Setting Up the Project

### 1. Clone the Repository
```bash
git clone https://github.com/your-username/CarSearchApp.git
cd CarSearchApp
```

### 2. Configure the Database
1. Rename the `appsettings.json-sample` file to the `appsettings.json`
1. Open the `appsettings.json` file and configure the database connection string:

```json
{
  "ConnectionStrings": {
    "CarConnectionString": "Server=localhost;Database=CarDb;Trusted_Connection=True;"
  }
}
```

- Replace `localhost` with your SQL Server instance.
- Set the desired database name (e.g., `CarDb`).

2. Create the database:
```bash
dotnet ef database update
```

Ensure `dotnet-ef` is installed by running:
```bash
dotnet tool install --global dotnet-ef
```

### 3. Build and Run the Application
```bash
dotnet build
```

To run the application:
```bash
dotnet run --project CarSearchApp.csproj
```

Alternatively, if using Visual Studio:
1. Open the solution (`CarSearchApp.sln`).
2. Set `CarSearchApp` as the startup project.
3. Press `F5` to build and run.

---

## Using the Application

1. **Search for Cars:**
   - Navigate to `https://localhost:5001/`.
   - Enter search criteria such as length, weight, velocity, or color.
   - Click `Search` to filter results.

2. **Export to XML:**
   - After searching, click `Export to XML` to download the search results as an XML file.

---

## Database Migrations
In case of model changes, generate new migrations and apply them to the database:

```bash
dotnet ef migrations add [MigrationName]
dotnet ef database update
```

---

## Troubleshooting

- **Database Connection Issues:**
  - Ensure SQL Server is running and accessible.
  - Verify the connection string in `appsettings.json`.

- **EF Core Tools Not Found:**
  - Install EF Core CLI tools:
    ```bash
    dotnet tool install --global dotnet-ef
    ```

- **Port Conflicts:**
  - Modify the `launchSettings.json` or pass `--urls` to specify a different port:
    ```bash
    dotnet run --urls=http://localhost:5002
    ```

