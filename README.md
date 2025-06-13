# AdventureWorks Blazor Server Application

Een moderne, mobile-first Blazor Server applicatie voor CRUD operaties op de AdventureWorks 2022 database, gebouwd met Clean Architecture principes en MudBlazor UI componenten.

## 🚀 Project Overview

Dit project implementeert een complete CRUD interface voor de AdventureWorks database met focus op:
- **Mobile-first responsive design**
- **Clean Architecture** met Repository en Service patterns
- **MudBlazor** UI componenten voor moderne, toegankelijke interface
- **GitHub Copilot** integration voor consistente code generatie

## 📋 Current Implementation Status

### ✅ Completed Modules
- **People Module**: Complete CRUD implementatie
  - Advanced data grid met paging, sorting, filtering
  - Mobile-optimized detail views
  - Touch-friendly create/edit forms
  - Dual navigation options
  - Complete DTO hierarchy
  - Service en Repository layers
  - Error handling en loading states

### 🔄 Ready for Expansion
- Product Module (entities beschikbaar)
- Customer Module (entities beschikbaar)
- Sales Module (entities beschikbaar)
- Employee Module (entities beschikbaar)

## 🛠️ Technology Stack

- **.NET 8** - Latest LTS framework
- **Blazor Server** - Interactive web applications
- **MudBlazor 6.11.2** - Material Design UI components
- **Entity Framework Core 8.0** - Database ORM
- **SQL Server** - AdventureWorks 2022 database
- **Clean Architecture** - Separation of concerns
- **Repository Pattern** - Data access abstraction

## 📱 Mobile-First Design

Alle UI componenten zijn ontworpen met mobile-first principes:
- **Responsive breakpoints**: 480px, 768px, 1024px, 1200px
- **Touch targets**: Minimum 44px voor optimale touch experience
- **Progressive enhancement**: Start mobile, enhance voor desktop
- **Adaptive layouts**: Content past zich aan aan schermgrootte

## 🏗️ Architecture

```
AdventureWorks.Web/          # Blazor Server presentation layer
├── Pages/People/            # CRUD pages (Index, Details, Create, Edit)
├── Shared/                  # Shared components
└── wwwroot/                 # Static assets

AdventureWorks.Core/         # Domain layer
├── Entities/                # Domain entities
└── Interfaces/              # Service abstractions

AdventureWorks.Infrastructure/ # Infrastructure layer
├── Data/                    # DbContext
├── Repositories/            # Data access implementations
├── Services/                # Business logic implementations
└── Mappings/                # DTO mapping extensions

AdventureWorks.Shared/       # Shared contracts
└── DTOs/                    # Data transfer objects

tests/                       # Test projects
├── AdventureWorks.Tests.Unit/
└── AdventureWorks.Tests.Integration/
```

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB of volledige installatie)
- Visual Studio 2022 of VS Code met C# extension

### Installation

1. **Clone de repository**
   ```bash
   git clone https://github.com/[username]/AdventureWorksDb.git
   cd AdventureWorksDb
   ```

2. **Restore packages**
   ```bash
   dotnet restore
   ```

3. **Update database connection string**
   Bewerk `src/AdventureWorks.Web/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AdventureWorks2022;Trusted_Connection=true;"
     }
   }
   ```

4. **Build en run**
   ```bash
   dotnet build
   dotnet run --project src/AdventureWorks.Web
   ```

5. **Open browser**
   Navigate naar `https://localhost:7001`

## 🔧 Development

### VS Code Setup
Het project bevat complete VS Code configuratie in `.vscode/`:
- **settings.json**: Optimized development settings
- **launch.json**: Debug configurations
- **tasks.json**: Build, run, test tasks
- **extensions.json**: Recommended extensions

### GitHub Copilot Integration
Complete Copilot configuratie voor consistente code generatie:
- **`.copilot-instructions.md`**: Coding standards en patterns
- **`.copilot-prompts.md`**: Ready-to-use prompts
- **`.copilot-chat-participant.md`**: Chat context guidelines

### Adding New Modules
Gebruik het People module als template:

1. **Generate with Copilot**:
   ```
   Create a complete CRUD module for [EntityName] following the People module pattern
   ```

2. **Implementation checklist**:
   - [ ] Create DTOs in Shared project
   - [ ] Implement service interface in Core
   - [ ] Create repository in Infrastructure
   - [ ] Implement service in Infrastructure
   - [ ] Create Blazor pages in Web
   - [ ] Register services in DI
   - [ ] Test all CRUD operations

## 📊 Features

### Data Grid (Advanced)
- **Server-side paging** voor performance
- **Multi-column sorting** 
- **Column filtering** met dropdown menus
- **Search functionality** in toolbar
- **Responsive columns** die verbergen op kleinere schermen
- **Action buttons** met proper touch targets

### Forms (Mobile-Optimized)
- **Responsive layouts** met MudGrid
- **Touch-friendly controls** 
- **Validation feedback** 
- **Loading states** tijdens submission
- **Error handling** met user-friendly messages

### Navigation
- **Breadcrumb navigation** 
- **Multi-option navigation** (bijv. "Back to List" + "Back to Details")
- **Responsive button text** (volledig op desktop, compact op mobile)

## 🧪 Testing

```bash
# Run alle tests
dotnet test

# Run specifieke test project
dotnet test tests/AdventureWorks.Tests.Unit/

# Run met coverage
dotnet test --collect:"XPlat Code Coverage"
```

## 📝 Documentation

- **Plan van Aanpak**: `Plan_van_Aanpak.md` - Complete project planning en progress
- **ERD**: `AdventureWorks_ERD.md` - Database entity relationships
- **Copilot Guide**: `.copilot-readme.md` - GitHub Copilot usage instructions

## 🤝 Contributing

1. Fork het project
2. Create feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add AmazingFeature'`)
4. Push naar branch (`git push origin feature/AmazingFeature`)
5. Open Pull Request

### Code Standards
- Follow established patterns in People module
- Implement mobile-first responsive design
- Include proper error handling en loading states
- Add unit tests voor nieuwe functionality
- Update documentation

## 📄 License

Dit project is gelicenseerd onder de MIT License - zie `LICENSE` file voor details.

## 🙏 Acknowledgments

- **AdventureWorks Database** - Microsoft sample database
- **MudBlazor** - Material Design Blazor components
- **Entity Framework Core** - ORM framework
- **GitHub Copilot** - AI-powered code assistance

## 📞 Support

Voor vragen of ondersteuning:
- Open een GitHub Issue
- Check de documentatie in `/docs/`
- Review de Copilot instruction files voor development guidance

---

**Built with ❤️ using .NET 8, Blazor Server, and MudBlazor**
