# StajBlogSitesi (ASP.NET Core MVC Blog Application)

This is a multi-layered blog platform built with ASP.NET Core MVC. It supports writer registration with email confirmation, blog posting, category management, and secure login.

## Features

- Writer registration with email confirmation (via Mailtrap)
- FluentValidation for input validation
- Layered architecture (Entity, DataAccess, Business, UI)
- SweetAlert2 for dynamic alerts
- Writer profile editing and password changing
- Entity Framework Core with code-first migrations

## Project Structure

- `EntityLayer/` – Entity models
- `DataAccessLayer/` – EF Core repositories
- `BusinessLayer/` – Business logic and validators
- `StajBlogSitesi/` – ASP.NET Core MVC frontend (UI + Controllers)
- `docs/` – Project documentation (`DOCUMENTATION.pdf`)

## Requirements

- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- Visual Studio 2022 (or `dotnet CLI`)
- SQL Server (or SQLite / LocalDB depending on config)

## Getting Started

1. **Clone the Repository**
   git clone https://github.com/your-username/StajBlogSitesi.git
   cd StajBlogSitesi
   

2. **Open in Visual Studio**
   - Open the solution file: `StajBlogSitesi/StajBlogSitesi.sln`

3. **Database Configuration**
   - In `appsettings.json`, update the `DefaultConnection` string with your SQL Server credentials.

4. **Apply Migrations**
   If migrations already exist:
   dotnet ef database update

   If no migration exists yet:
   dotnet ef migrations add InitialCreate
   dotnet ef database update

5. **Mailtrap Setup**
   - In `RegisterController.cs`, update the SMTP username and password with your [Mailtrap](https://mailtrap.io) credentials.

6. **Run the Project**
   - From Visual Studio: Press `F5` or `Ctrl + F5`
   - From CLI:
     dotnet run --project StajBlogSitesi

## Contribution

Feel free to contribute! Submit a pull request or open an issue to collaborate.

## License

[MIT License](LICENSE)

## Author

**Anýl Cem Elemir**