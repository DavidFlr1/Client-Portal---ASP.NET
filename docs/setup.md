# Setup .net project

## Structure. setup
  ```bash
  # 1. Create solution folder
  mkdir ClientServicePortal && cd ClientServicePortal
  
  # 2. Create an empty solution
  dotnet new sln -n ClientServicePortal

  # 3. Create projects
  dotnet new webapi   -n ClientServicePortal.API
  dotnet new classlib -n ClientServicePortal.Core
  dotnet new classlib -n ClientServicePortal.Infrastructure

  # 4. Add projects to solution
  dotnet sln add ClientServicePortal.API ClientServicePortal.Core ClientServicePortal.Infrastructure

  # 5. Add project references
  dotnet add ClientServicePortal.API           reference ClientServicePortal.Core ClientServicePortal.Infrastructure
  dotnet add ClientServicePortal.Infrastructure reference ClientServicePortal.Core
  ```

## Install packages
  ```bash
  # EF Core + SQL Server
  dotnet add ClientServicePortal.API            package Microsoft.EntityFrameworkCore.SqlServer
  dotnet add ClientServicePortal.API            package Microsoft.EntityFrameworkCore.Tools
  dotnet add ClientServicePortal.API            package Swashbuckle.AspNetCore
  dotnet add ClientServicePortal.API            package Microsoft.AspNetCore.Authentication.JwtBearer
  dotnet add ClientServicePortal.Infrastructure package Microsoft.EntityFrameworkCore.SqlServer
  dotnet add ClientServicePortal.Infrastructure package Microsoft.EntityFrameworkCore.Design
  dotnet add ClientServicePortal.Infrastructure package Microsoft.Extensions.Configuration
  dotnet add ClientServicePortal.Infrastructure package Microsoft.AspNetCore.Identity.EntityFrameworkCore
  dotnet add ClientServicePortal.Infrastructure System.IdentityModel.Tokens.Jwt
  dotnet add ClientServicePortal.Core           package Microsoft.AspNetCore.Identity.EntityFrameworkCore
  ```

## Setup SQL server docker container
  ```bash
  # 1. Download image
  sudo docker pull mcr.microsoft.com/mssql/server:2022-latest

  # 2. Run container
  docker run -d --name SQL_Server_Docker -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=<YourStrongPassword>' -p 1433:1433 mcr.microsoft.com/mssql/server:2022-latest
  ```
  * Remove -d to not run on the background

## Create DB and setup API, Core and Infrastructure files.
```bash
# Install entity framework and initial migration
  # 1. Install EF tool
  dotnet tool install --global dotnet-ef

  # 2. Database
  # Add initial migration (points at Infrastructure where AppDbContext lives)
  dotnet ef migrations add InitialCreate \
    --project ClientServicePortal.Infrastructure \
    --startup-project ClientServicePortal.API

  # Apply migration to the Docker SQL Server
  dotnet ef database update \
    --project ClientServicePortal.Infrastructure \
    --startup-project ClientServicePortal.API
  ```

  ## Alternative: Recreate db for sensitive updates
  ```bash
  # Remove all migrations
  dotnet ef migrations remove \
    --project ClientServicePortal.Infrastructure \
    --startup-project ClientServicePortal.API

  # *Drop the database
  dotnet ef database drop \
    --project ClientServicePortal.Infrastructure \
    --startup-project ClientServicePortal.API

  rm -rf ClientServicePortal.Infrastructure/Migrations/*

  # Create fresh migration
  dotnet ef migrations add InitialCreate \
    --project ClientServicePortal.Infrastructure \
    --startup-project ClientServicePortal.API

  # Apply migration
  dotnet ef database update \
    --project ClientServicePortal.Infrastructure \
    --startup-project ClientServicePortal.API
  ```
# Run the API
```bash
dotnet run --project ClientServicePortal.API
```

# Perform Check
```bash
# Health endpoint: 
GET http://localhost:5264/health or https://localhost:7129/health
```

# Miscellaneous
```bash
# Swagger connection
http://localhost:5264/swagger
```