# Cloned from: [Teddy Smith](https://www.youtube.com/playlist?list=PL82C6-O4XrHdiS10BLh23x71ve9mQCln0)

# How to run
1. Run SQL Server database instance :
`docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Password123#" -e "MSSQL_PID=Evaluation" -p 1433:1433  --name sqlpreview --hostname sqlpreview -d mcr.microsoft.com/mssql/server:2022-preview-ubuntu-22.04`
2. Install EF CLI tool: `dotnet tool install --global dotnet-ef`
3. Restore NuGet's dependencies and tools: `dotnet restore`
4. cd to project directory: `cd WebApplication1`
5. Run migration: `dotnet ef database update`
6. Run seeding and start server: `dotnet run seeddata`

# Packages used:
- AutoMapper: Mapping between DTO and Class (Domain Entities)
- Microsoft.AspNetCore: ASP.NET Core framework components
- Microsoft.EntityFrameworkCore: ORM library for interacting with database

<h3>Notes:</h3> 
- Check what routes are available in `Controllers` directory of a project. Check `Route()` and `Http{Method}` <br>
-`Migrations` directory is generated from `Models`: Run `dotnet ef migrations add {className}` <br>
- Uses Repository Pattern and Dependency Injection <br>
- Define mapping between DTO and Domain Entity in `Helper/MappingProfiles.cs` <br>
- Add an entity: Add Interface -> Add Repository -> Add scope in `Program.cs` -> Modify Mapping -> Add controller <br>
- Add an endpoint: Modify Interface -> Modify Repository -> Modify Controller 
