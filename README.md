# AdventureWorks CRUD Application

Een moderne CRUD-applicatie voor de AdventureWorks 2022 database gebouwd met C#, Blazor Server en MudBlazor.

## Technologie Stack
- **Backend**: C# .NET 8
- **Frontend**: Blazor Server
- **UI Framework**: MudBlazor
- **Database**: SQL Server (AdventureWorks 2022)
- **ORM**: Entity Framework Core

## Project Structuur
```
AdventureWorksApp/
├── src/
│   ├── AdventureWorks.Web/              # Blazor Server App
│   ├── AdventureWorks.Core/             # Business Logic & Entities
│   ├── AdventureWorks.Infrastructure/   # Data Access & External Services
│   └── AdventureWorks.Shared/           # Shared Models & DTOs
├── tests/
│   ├── AdventureWorks.Tests.Unit/
│   └── AdventureWorks.Tests.Integration/
└── docs/
```

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server with AdventureWorks 2022 database
- VS Code

### Running the Application
```bash
cd src/AdventureWorks.Web
dotnet run
```

## Documentation
- [Plan van Aanpak](Plan_van_Aanpak.md)
- [Database ERD](AdventureWorks_ERD.md)
