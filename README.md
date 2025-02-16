# Wine API
## Overview:
The API implements full CRUD operations, robust request validation, comprehensive error handling, and JWT-based authentication. The project is built using ASP.NET Core with Entity Framework Core for data access, leveraging an in-memory database. It follows clean architecture principles, ensuring maintainability, scalability, and separation of concerns throughout the application.
## Features:
- ASP.NET Core Web API project.
- Entity Framework Core and in-memory database.
- DTOs.
- FluentValidation.
- Global exception handler.
- AutoMapper.
- Built-in ASP.NET Core logging.
- JWT authentication.
- xUnit.
- Swagger.
## API Endpoints:
| HTTP Method | Endpoint         | Description |
|------------|----------------|-------------|
| GET        | /api/wines      | Get all wines |
| GET        | /api/wines/{id} | Get a wine by ID |
| POST       | /api/wines      | Add a new wine |
| PUT        | /api/wines/{id} | Update a wine |
| DELETE     | /api/wines/{id} | Delete a wine (secured by JWT) |
## Setup Instructions
1. Clone the repository.
2. Open the project in your preferred IDE (e.g., Visual Studio).
3. Run the application.
