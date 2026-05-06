# Midterm Banking System - API Documentation

Welcome to the API documentation for the Midterm Banking System.

## Overview

This banking system provides a comprehensive REST API for managing user accounts, including operations such as:

- User authentication and account creation
- Deposit and withdrawal operations
- Account management and updates
- Account balance inquiries

## Getting Started

### Key Components

1. **API Layer** - RESTful endpoints for account management
2. **Business Logic Layer** - Service layer with validation and business rules
3. **Data Access Layer** - Database operations and user repository

### Architecture

The system follows a layered architecture with clear separation of concerns:

- **Controllers** - HTTP endpoint handlers
- **Services** - Business logic and validation
- **Repository** - Data access and database operations
- **Models** - Domain entities and DTOs

## API Endpoints

### Account Management

- `POST /account/create` - Create a new account
- `GET /account/{id}` - Get account details
- `POST /account/update` - Update account information
- `DELETE /account/{id}` - Delete an account

### Transactions

- `POST /account/deposit` - Deposit funds
- `POST /account/withdraw` - Withdraw funds

## Error Handling

All API endpoints return appropriate HTTP status codes:

- `200 OK` - Request successful
- `201 Created` - Resource created successfully
- `400 Bad Request` - Invalid request parameters
- `404 Not Found` - Resource not found
- `409 Conflict` - Resource conflict (e.g., duplicate login)
- `500 Internal Server Error` - Server error

## Code Quality

This project maintains high code quality standards:

- **StyleCop Analyzers** - Enforces C# coding standards
- **Unit Tests** - Comprehensive test coverage (90% threshold)
- **Documentation** - XML documentation for all public APIs
- **SOLID Principles** - Dependency injection and layered architecture

## Building and Testing

### Prerequisites

- .NET 10.0 SDK or later
- MySQL Server

### Build

```bash
./build.sh          # Linux/macOS
.\build.ps1         # Windows
```

### Running Tests

```bash
cd backend
dotnet test tests/tests.csproj -c Release
```

### Generate Documentation

```bash
# Install DocFX
dotnet tool install -g docfx

# Generate documentation
docfx docs/docfx_project/docfx.json
```

## Documentation Structure

- **[API Reference](api/index.md)** - Detailed API documentation
- **[User](user.md)** - User entity documentation
- **[IAccountService](services.md)** - Account service interface
- **[IUserRepository](repository.md)** - User repository interface

## Contributing

When contributing to this project:

1. Follow the EditorConfig standards (`.editorconfig`)
2. Ensure StyleCop compliance
3. Add XML documentation for public members
4. Write comprehensive unit tests
5. Build and test locally before submitting

## License

See LICENSE file for details.
