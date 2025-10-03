# ClientServicePortal

A modern ASP.NET Core Web API for managing client services with Entity Framework Core and SQL Server integration.

## Project Structure

```
ClientServicePortal/
│── ClientServicePortal.API/             # ASP.NET Core Web API (controllers, startup, etc.)
│   ├── Controllers/                     # HTTP Request
│   ├── Program.cs                       # Provider
│   ├── appsettings.json
│
│── ClientServicePortal.Core/            # Domain models + business logic
│   ├── Entities/                        # C# classes = DB tables structure (Client.cs, Project.cs, etc.) 
│   ├── DTOs/                            # Plain request/response shape
│   ├── Interfaces/                      # Available operations 
│   ├── *Services/                       # Optional: To store interfaces and repositories together.
│
│── ClientServicePortal.Infrastructure/  # Data access (EF Core DbContext, Repos, SQL integration)
│   ├── Data/
│   │   ├── AppDbContext.cs              # EF Core context mapping tables
│   │   ├── Configurations/              # Fluent API configs per entity
│   ├── Repositories/                    # Operation access logic
│
│── sql/                                 # Raw SQL scripts
│   ├── 01_create_database.sql
│   ├── 02_tables.sql
│   ├── 03_stored_procedures.sql
│   ├── 04_seed_data.sql
│
│── .gitignore
│── ClientServicePortal.sln
└── README.md
```

## Requirements

- .NET 9.0 SDK
- Docker (for SQL Server)
- Entity Framework Core Tools

## Development Setup

### 1. Clone and Setup
```bash
git clone <repository-url>
cd ClientServicePortal
```

### 2. Start SQL Server (Docker)
```bash
docker run -d --name SQL_Server_Docker \
  -e 'ACCEPT_EULA=Y' \
  -e 'SA_PASSWORD=admin123!' \
  -p 1433:1433 \
  mcr.microsoft.com/mssql/server:2022-latest
```

### 3. Install EF Tools
```bash
dotnet tool install --global dotnet-ef
```

### 4. Configure User Secrets (Secure Configuration)
```bash
# Initialize user secrets
dotnet user-secrets init --project ClientServicePortal.API

# Set connection string securely
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=ClientServicePortal;User Id=sa;Password=admin123!;TrustServerCertificate=True;" --project ClientServicePortal.API
```

### 5. Run Migrations
```bash
dotnet ef database update \
  --project ClientServicePortal.Infrastructure \
  --startup-project ClientServicePortal.API
```

### 6. Run the API
```bash
dotnet run --project ClientServicePortal.API
```

## Endpoints

- Health Check: `GET /health`
- Swagger UI: `http://localhost:5264/swagger`
- API Base: `http://localhost:5264` or `https://localhost:7129`

## Configuration

### Development
- Uses **User Secrets** for sensitive data (connection strings, API keys)
- User secrets are stored outside the project directory and never committed to git
- Configuration files: `appsettings.json`, `appsettings.Development.json`

### Production
Set environment variables:
```bash
export ConnectionStrings__DefaultConnection="your-production-connection-string"
export ASPNETCORE_ENVIRONMENT="Production"
```

## Deployment

### 1. Build the Solution
```bash
dotnet build --configuration Release
```

### 2. Publish the API
```bash
dotnet publish ClientServicePortal.API --configuration Release --output ./publish
```

### 3. Set Production Environment Variables
```bash
# Connection string
export ConnectionStrings__DefaultConnection="Server=prod-server;Database=ClientServicePortal;User Id=prod-user;Password=secure-password;TrustServerCertificate=True;"

# Environment
export ASPNETCORE_ENVIRONMENT="Production"
export ASPNETCORE_URLS="http://+:80"
```

### 4. Run Database Migrations in Production
```bash
dotnet ef database update \
  --project ClientServicePortal.Infrastructure \
  --startup-project ClientServicePortal.API \
  --configuration Release
```

### 5. Start the Application
```bash
dotnet ./publish/ClientServicePortal.API.dll
```

## Security Notes

- **Never commit sensitive data** to git
- Use **User Secrets** for development
- Use **Environment Variables** or **Azure Key Vault** for production
- Connection strings and API keys are excluded from source control
