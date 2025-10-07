# Project.HR  

**Project.HR** is a demo Human Resources management system currently under development.  
It follows a layered architecture with clear separation of concerns:  

- **Data Layer**  
- **Domain Layer**  
- **UI Layer**  
- **Web API Layer**  

---

## 🚀 Tech Stack  
- **.NET 9**  
- **Entity Framework Core**  
- **SQLite** (lightweight relational database)  
- **Blazor** (UI)  
- **Minimal APIs** (Web API)  
- **NLog** (logging)  

---

## 📂 Project Structure  

### 1. Data Layer  
Responsible for persistence and database operations.  
- Uses **SQLite** with **Entity Framework Core**.  
- Provides CRUD operations for:  
  - Department  
  - Employee  
  - Position  
  - Roles  

---

### 2. Domain Layer  
Contains the core business logic and abstractions.  

Includes:  
- **DTOs**  
- **Enums**  
- **Helpers**  
- **Interfaces**  
- **Models**  

#### 🔹 Logging  
A static helper class `LogErrorHelper` wraps **NLog** for structured error logging:  

```csharp
using NLog;
using System.Runtime.CompilerServices;

namespace Project.HR.Domain.Helpers
{
    public static class LogErrorHelper
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public enum ErrorLevel { Info, Warn, Error, Fatal, Trace }

        public static void LogError(
            string? message,
            Exception? ex,
            ErrorLevel level = ErrorLevel.Error,
            [CallerMemberName] string callerName = "")
        {
            string logMessage = $"[{callerName}] {message}";

            switch (level)
            {
                case ErrorLevel.Info:  _logger.Info(ex, logMessage); break;
                case ErrorLevel.Warn:  _logger.Warn(ex, logMessage); break;
                case ErrorLevel.Error: _logger.Error(ex, logMessage); break;
                case ErrorLevel.Fatal: _logger.Fatal(ex, logMessage); break;
                case ErrorLevel.Trace: _logger.Trace(ex, logMessage); break;
                default:               _logger.Error(ex, logMessage); break;
            }
        }
    }
}
```
### Interfaces
Defines contracts for the Data Access Layer. Example:

```csharp
using Project.HR.Domain.DTOs;
using Project.HR.Domain.Models;

namespace Project.HR.Domain.Interfaces
{
    public interface IDepartmentService
    {
        Task<List<DepartmentDTO?>> GetAllDepartmentsAsync();
        Task<Department> CreateDepartmentAsync(Department department);
        Task<Department?> GetDepartmentByNameAsync(string departmentName);
        Task<DepartmentDTO?> GetDepartmentByIdAsync(int id);
        Task<bool> DeleteDepartmentAsync(int id);
        Task<Department?> UpdateDepartmentAsync(int id, Department department);
    }
}


```
## UI Layer
* Built with Blazor.
* Provides a user-friendly interface to manage CRUD operations.
* Connects to the Web API hosted in Azure.

## WebAPI Layer
* Built with .NET 9 Minimal APIs.
* Exposes endpoints for all CRUD operations.
* Acts as the bridge between the UI and Data Layer.

  📌 Roadmap
* [ ] Add authentication & authorization

* [ ] Expand domain models (e.g., Payroll, Benefits)

* [ ] Improve error handling and logging

* [ ] Add unit/integration tests


