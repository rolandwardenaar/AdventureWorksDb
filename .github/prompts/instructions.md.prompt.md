---
mode: beastmode
---
# AdventureWorks Blazor Server Project - Copilot Instructions

## Project Overview
This is a production-ready Blazor Server application using MudBlazor for UI components, implementing CRUD operations for the AdventureWorks 2022 database using Clean Architecture pattern. The People module is fully implemented and serves as the reference pattern for all other modules.

## Technology Stack
- **Framework**: .NET 8, Blazor Server
- **UI**: MudBlazor 6.11.2 with mobile-first responsive design
- **Database**: SQL Server with Entity Framework Core 8.0
- **Architecture**: Clean Architecture with Repository Pattern
- **Validation**: FluentValidation (ready for implementation)
- **Mapping**: Custom extension methods with ToDto patterns
- **State Management**: Component-level state with service injection
- **Error Handling**: Structured exception handling with logging

## Architecture Layers

### 1. Web Layer (`AdventureWorks.Web`)
- Blazor Server pages and components
- Razor components in `/Pages/` folder
- Shared components in `/Shared/` folder
- Uses dependency injection for services

### 2. Core Layer (`AdventureWorks.Core`)
- Business entities in `/Entities/`
- Service interfaces in `/Interfaces/`
- Business logic and domain rules

### 3. Infrastructure Layer (`AdventureWorks.Infrastructure`)
- Entity Framework DbContext in `/Data/`
- Repository implementations in `/Repositories/`
- Service implementations in `/Services/`
- DTO mappings in `/Mappings/`

### 4. Shared Layer (`AdventureWorks.Shared`)
- DTOs for data transfer between layers
- Query parameters and paged results

## Coding Standards & Patterns

### Naming Conventions
- **Entities**: PascalCase (e.g., `Person`, `Product`)
- **DTOs**: EntityNameDto (e.g., `PersonDto`, `PersonDetailDto`, `PersonListDto`)
- **Services**: IEntityService (e.g., `IPersonService`)
- **Repositories**: IEntityRepository (e.g., `IPersonRepository`)
- **Pages**: EntityAction.razor (e.g., `Index.razor`, `Details.razor`, `Create.razor`, `Edit.razor`)

### File Structure for CRUD Modules
```
Pages/
└── EntityName/
    ├── Index.razor          # List view with grid
    ├── Details.razor        # Read-only detail view
    ├── Create.razor         # Add new entity
    └── Edit.razor           # Edit existing entity
```

### DTO Patterns
Create specific DTOs for different operations (follow established People module pattern):
- `EntityDto`: Basic entity data for general use
- `EntityDetailDto`: Extended data with related entities for detail views  
- `EntityListDto`: Compact data optimized for grid listings and performance
- `EntityCreateDto`: Data required for creating new entities (no ID)
- `EntityUpdateDto`: Data for updating entities (may exclude certain fields)
- `PagedResult<T>`: Wrapper for paged data with total count and metadata

#### Example Implementation (from PersonDto)
```csharp
public class PersonListDto
{
    public int BusinessEntityId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string PersonType { get; set; } = string.Empty;
    public DateTime ModifiedDate { get; set; }
}

public class PersonDetailDto : PersonListDto
{
    public string? MiddleName { get; set; }
    public string? Suffix { get; set; }
    public string? Title { get; set; }
    public bool EmailPromotion { get; set; }
    public string? AdditionalContactInfo { get; set; }
    public string? Demographics { get; set; }
    public Guid Rowguid { get; set; }
}
```

### Repository Pattern
```csharp
// Generic repository interface (implemented and tested)
public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
}

// Specific repository extends generic (PersonRepository is fully implemented)
public interface IPersonRepository : IGenericRepository<Person>
{
    Task<Person?> GetByIdWithDetailsAsync(int businessEntityId);
    Task<IEnumerable<Person>> SearchPersonsAsync(String searchTerm);
    Task<PagedResult<Person>> GetPagedAsync(PersonQueryParameters parameters);
}

// Implementation example with EF Core optimizations
public class PersonRepository : GenericRepository<Person>, IPersonRepository
{
    public PersonRepository(AdventureWorksDbContext context) : base(context) { }

    public async Task<Person?> GetByIdWithDetailsAsync(int businessEntityId)
    {
        return await _context.People
            .Include(p => p.BusinessEntity)
            .FirstOrDefaultAsync(p => p.BusinessEntityId == businessEntityId);
    }
}
```

### Service Pattern
```csharp
// Standard service interface pattern (implemented in PersonService)
public interface IPersonService
{
    // Read operations with paging and search
    Task<PagedResult<PersonListDto>> GetPersonsPagedAsync(PersonQueryParameters queryParams);
    Task<PersonDetailDto?> GetPersonByIdAsync(int businessEntityId);
    Task<IEnumerable<PersonListDto>> SearchPersonsAsync(string searchTerm);
    
    // Write operations with proper DTOs
    Task<PersonDetailDto> CreatePersonAsync(PersonCreateDto personDto);
    Task<PersonDetailDto> UpdatePersonAsync(int businessEntityId, PersonUpdateDto personDto);
    Task<bool> DeletePersonAsync(int businessEntityId);
}

// Service implementation pattern
public class PersonService : IPersonService
{
    private readonly IPersonRepository _repository;
    private readonly ILogger<PersonService> _logger;

    public PersonService(IPersonRepository repository, ILogger<PersonService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<PersonDetailDto?> GetPersonByIdAsync(int businessEntityId)
    {
        try
        {
            var person = await _repository.GetByIdWithDetailsAsync(businessEntityId);
            return person?.ToDetailDto();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving person with ID {Id}", businessEntityId);
            throw;
        }
    }
}
```

## Blazor Component Patterns

### Page Structure Template
```razor
@page "/entity/{id:int}"
@inject IEntityService EntityService
@inject NavigationManager Navigation
@inject ISnackbar Snackbar

<PageTitle>Entity Name - AdventureWorks</PageTitle>

<MudContainer MaxWidth="MaxWidth.Small" Class="mt-1 mobile-first-container">
    <MudGrid Spacing="1">
        <!-- Header Section with Breadcrumbs -->
        <MudItem xs="12">
            <MudPaper Class="pa-2 pa-sm-3" Elevation="1">
                <MudBreadcrumbs Items="breadcrumbItems" Separator=">">
                    <ItemTemplate Context="item">
                        <MudLink Href="@item.Href" Class="@(item.Disabled ? "mud-text-disabled" : "")">
                            @item.Text
                        </MudLink>
                    </ItemTemplate>
                </MudBreadcrumbs>
                <MudText Typo="Typo.h6" Class="mt-2">Entity Details</MudText>
            </MudPaper>
        </MudItem>

        <!-- Content Section with proper state handling -->
        @if (loading)
        {
            <MudItem xs="12" Class="d-flex justify-center pa-4">
                <MudProgressCircular Indeterminate="true" />
            </MudItem>
        }
        else if (entity == null)
        {
            <MudItem xs="12">
                <MudAlert Severity="Severity.Error">Entity not found</MudAlert>
            </MudItem>
        }
        else
        {
            <!-- Main content with responsive layout -->
            <MudItem xs="12">
                <MudPaper Class="pa-2 pa-sm-3" Elevation="1">
                    <!-- Entity details here -->
                </MudPaper>
            </MudItem>
            
            <!-- Action buttons section -->
            <MudItem xs="12">
                <MudPaper Class="pa-2" Elevation="1">
                    <MudStack Row Spacing="1" Justify="Justify.Center" Class="action-buttons">
                        <!-- Responsive action buttons -->
                    </MudStack>
                </MudPaper>
            </MudItem>
        }
    </MudGrid>
</MudContainer>

@code {
    [Parameter] public int Id { get; set; }
    private EntityDetailDto? entity;
    private bool loading = false;
    
    private List<BreadcrumbItem> breadcrumbItems = new()
    {
        new BreadcrumbItem("Home", href: "/"),
        new BreadcrumbItem("Entities", href: "/entities"),
        new BreadcrumbItem("Details", href: null, disabled: true)
    };

    protected override async Task OnParametersSetAsync()
    {
        await LoadEntityAsync();
    }

    private async Task LoadEntityAsync()
    {
        loading = true;
        try
        {
            entity = await EntityService.GetEntityByIdAsync(Id);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading entity: {ex.Message}", Severity.Error);
            entity = null;
        }
        finally
        {
            loading = false;
        }
    }
}
```

### Mobile-First CSS Pattern (Production-Ready)
```css
/* Mobile-first base styles - Always start here */
.mobile-first-container {
    max-width: 100%;
    margin: 0;
    padding: 0 4px;
}

.action-buttons .mud-button {
    min-width: 44px; /* Touch target minimum */
    min-height: 44px;
}

.responsive-grid .mud-table-cell {
    padding: 4px 8px;
    font-size: 0.875rem;
}

/* Small Screens (≥480px) - Enhanced mobile */
@@media (min-width: 480px) {
    .mobile-first-container {
        padding: 0 8px;
    }
    
    .responsive-grid .mud-table-cell {
        padding: 6px 12px;
    }
}

/* Medium Screens (≥768px) - Tablet */
@@media (min-width: 768px) {
    .mobile-first-container {
        max-width: 720px;
        margin: 0 auto;
        padding: 0 12px;
    }
    
    .responsive-grid .mud-table-cell {
        padding: 8px 16px;
        font-size: 1rem;
    }
    
    .action-buttons {
        gap: 8px;
    }
}

/* Large Screens (≥1024px) - Desktop */
@@media (min-width: 1024px) {
    .mobile-first-container {
        max-width: 960px;
    }
    
    .action-buttons {
        gap: 12px;
    }
}

/* Extra Large Screens (≥1200px) - Wide desktop */
@@media (min-width: 1200px) {
    .mobile-first-container {
        max-width: 1140px;
    }
}

/* Responsive button text patterns */
.responsive-btn .d-none {
    display: none !important;
}

.responsive-btn .d-inline {
    display: inline !important;
}

@@media (min-width: 576px) {
    .responsive-btn .d-sm-inline {
        display: inline !important;
    }
    
    .responsive-btn .d-sm-none {
        display: none !important;
    }
}
```

### Data Grid Configuration (Production Implementation)
```razor
<MudDataGrid T="PersonListDto" 
             Items="@people" 
             ServerData="LoadServerData"
             Sortable="true" 
             Filterable="true"
             FilterMode="DataGridFilterMode.ColumnFilterMenu"
             SortMode="SortMode.Multiple"
             Pageable="true"
             PageSize="25"
             RowsPerPageOptions="@(new int[] { 10, 25, 50, 100 })"
             Dense="true"
             Striped="true"
             Hover="true"
             Loading="@loading"
             Class="responsive-grid">
    
    <ToolBarContent>
        <MudText Typo="Typo.h6">People Management</MudText>
        <MudSpacer />
        <MudTextField @bind-Value="searchString" 
                      Placeholder="Search..." 
                      Adornment="Adornment.Start" 
                      AdornmentIcon="@Icons.Material.Filled.Search" 
                      IconSize="Size.Medium" 
                      Class="mt-0 d-none d-sm-flex" 
                      Style="max-width: 300px;" />
        <MudButton Variant="Variant.Filled"
                   Color="Color.Primary"
                   StartIcon="@Icons.Material.Filled.Add"
                   Href="/people/create"
                   Class="ml-2">
            <span class="d-none d-sm-inline">Add New Person</span>
            <span class="d-inline d-sm-none">Add</span>
        </MudButton>
    </ToolBarContent>

    <Columns>
        <PropertyColumn Property="x => x.FirstName" Title="First Name" Sortable="true" Filterable="true" />
        <PropertyColumn Property="x => x.LastName" Title="Last Name" Sortable="true" Filterable="true" />
        <PropertyColumn Property="x => x.PersonType" Title="Type" Sortable="true" Filterable="true" />
        <PropertyColumn Property="x => x.ModifiedDate" Title="Modified" Sortable="true" Format="yyyy-MM-dd" Class="d-none d-md-table-cell" />
        
        <TemplateColumn CellClass="d-flex justify-end" Sortable="false" Filterable="false">
            <CellTemplate>
                <MudStack Row Spacing="1">
                    <MudIconButton Icon="@Icons.Material.Filled.Visibility"
                                   Size="Size.Small"
                                   Color="Color.Primary"
                                   Href="@($"/people/details/{context.Item.BusinessEntityId}")"
                                   Title="View Details" />
                    <MudIconButton Icon="@Icons.Material.Filled.Edit"
                                   Size="Size.Small"
                                   Color="Color.Secondary"
                                   Href="@($"/people/edit/{context.Item.BusinessEntityId}")"
                                   Title="Edit" />
                    <MudIconButton Icon="@Icons.Material.Filled.Delete"
                                   Size="Size.Small"
                                   Color="Color.Error"
                                   OnClick="@(() => ConfirmDeletePerson(context.Item))"
                                   Title="Delete" />
                </MudStack>
            </CellTemplate>
        </TemplateColumn>
    </Columns>
    
    <PagerContent>
        <MudDataGridPager T="PersonListDto" />
    </PagerContent>
</MudDataGrid>
```

## Error Handling Patterns

### Service Error Handling
```csharp
public async Task<EntityDetailDto?> GetEntityByIdAsync(int id)
{
    try
    {
        var entity = await _repository.GetByIdWithDetailsAsync(id);
        return entity?.ToDetailDto();
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error retrieving entity with ID {Id}", id);
        throw;
    }
}
```

### Component Error Handling
```csharp
protected override async Task OnParametersSetAsync()
{
    loading = true;
    try
    {
        entity = await EntityService.GetEntityByIdAsync(Id);
    }
    catch (Exception ex)
    {
        Snackbar.Add($"Error loading entity: {ex.Message}", Severity.Error);
        entity = null;
    }
    finally
    {
        loading = false;
    }
}
```

## Dependency Injection Setup (Production Configuration)

### Service Registration Pattern
```csharp
// ServiceCollectionExtensions.cs - Infrastructure Layer
public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
{
    // Database Context with optimized settings
    services.AddDbContext<AdventureWorksDbContext>(options =>
    {
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        options.EnableSensitiveDataLogging(false); // Disable in production
        options.EnableDetailedErrors(false); // Disable in production
    });

    // Generic Repository Pattern
    services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

    // Specific Repositories (all implemented and tested)
    services.AddScoped<IPersonRepository, PersonRepository>();
    // Add more repositories as they are implemented

    // Business Services (all implemented and tested)
    services.AddScoped<IPersonService, PersonService>();
    // Add more services as they are implemented

    return services;
}

// Program.cs - Web Layer
public static void Main(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();
    
    // MudBlazor configuration
    builder.Services.AddMudServices(config =>
    {
        config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
        config.SnackbarConfiguration.PreventDuplicates = false;
        config.SnackbarConfiguration.NewestOnTop = false;
        config.SnackbarConfiguration.ShowCloseIcon = true;
        config.SnackbarConfiguration.VisibleStateDuration = 10000;
        config.SnackbarConfiguration.HideTransitionDuration = 500;
        config.SnackbarConfiguration.ShowTransitionDuration = 500;
        config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
    });

    // Infrastructure services registration
    builder.Services.AddInfrastructureServices(builder.Configuration);

    var app = builder.Build();

    // Configure pipeline
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();

    app.MapRazorPages();
    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");

    app.Run();
}
```

## Validation Patterns

### FluentValidation Setup
```csharp
public class PersonCreateDtoValidator : AbstractValidator<PersonCreateDto>
{
    public PersonCreateDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");

        RuleFor(x => x.PersonType)
            .NotEmpty().WithMessage("Person type is required")
            .Must(BeValidPersonType).WithMessage("Invalid person type");
    }

    private bool BeValidPersonType(string personType)
    {
        return new[] { "IN", "EM", "SP", "SC", "VC" }.Contains(personType);
    }
}
```

## Best Practices

### 1. Always Use DTOs
- Never expose entities directly to the UI
- Create specific DTOs for each operation
- Use mapping extensions for conversions

### 2. Mobile-First Design
- Start with mobile styles, enhance for larger screens
- Use responsive breakpoints: 480px, 768px, 1024px, 1200px
- Ensure touch targets are minimum 44px

### 3. Loading States
- Always show loading indicators for async operations
- Handle both loading and error states
- Provide meaningful error messages

### 4. Navigation
- Use consistent navigation patterns
- Provide multiple navigation options
- Include breadcrumbs for complex flows

### 5. Performance
- Use paging for large datasets
- Implement efficient database queries
- Minimize state updates in components

## Common Components

### Breadcrumb Setup
```csharp
private List<BreadcrumbItem> breadcrumbItems = new()
{
    new BreadcrumbItem("Home", href: "/"),
    new BreadcrumbItem("Entities", href: "/entities"),
    new BreadcrumbItem("Details", href: null, disabled: true)
};
```

### Action Buttons (Production Responsive Pattern)
```razor
<!-- Dual navigation pattern (from Edit.razor) -->
<MudStack Row Spacing="1" Justify="Justify.Center" Class="action-buttons">
    <MudButton Variant="Variant.Outlined" 
               Color="Color.Secondary" 
               Size="Size.Small"
               StartIcon="@Icons.Material.Filled.ArrowBack"
               Href="@($"/people/details/{person?.BusinessEntityId}")"
               Class="responsive-btn">
        <MudText Class="d-none d-sm-inline">Back to Details</MudText>
        <MudText Class="d-inline d-sm-none">Details</MudText>
    </MudButton>
    
    <MudButton Variant="Variant.Outlined" 
               Color="Color.Primary" 
               Size="Size.Small"
               StartIcon="@Icons.Material.Filled.List"
               Href="/people"
               Class="responsive-btn">
        <MudText Class="d-none d-sm-inline">Back to List</MudText>
        <MudText Class="d-inline d-sm-none">List</MudText>
    </MudButton>
    
    <MudButton Variant="Variant.Filled" 
               Color="Color.Success" 
               Size="Size.Small"
               StartIcon="@Icons.Material.Filled.Save"
               ButtonType="ButtonType.Submit"
               Disabled="@(!editForm.IsValid || isSubmitting)"
               Class="responsive-btn">
        @if (isSubmitting)
        {
            <MudProgressCircular Size="Size.Small" Indeterminate="true" />
        }
        else
        {
            <MudText Class="d-none d-sm-inline">Save Changes</MudText>
            <MudText Class="d-inline d-sm-none">Save</MudText>
        }
    </MudButton>
</MudStack>

<!-- Mobile action buttons with proper touch targets -->
<style>
    .action-buttons .mud-button {
        min-width: 44px;
        min-height: 44px;
    }
    
    .responsive-btn {
        padding: 8px 12px;
    }
    
    @@media (min-width: 576px) {
        .responsive-btn {
            padding: 8px 16px;
        }
    }
</style>
```

## Current Implementation Status

### ✅ Completed Modules
- **People Module**: Full CRUD implementation with mobile-first responsive design
  - Index.razor: Advanced data grid with paging, sorting, filtering
  - Details.razor: Mobile-optimized detail view with responsive layout
  - Create.razor: Touch-friendly create form with validation
  - Edit.razor: Enhanced edit form with dual navigation options
  - Complete DTO architecture: PersonDto, PersonDetailDto, PersonListDto, PersonCreateDto, PersonUpdateDto
  - Service layer: IPersonService with full CRUD operations
  - Repository layer: PersonRepository with advanced queries and paging
  - Dependency injection: Fully configured and tested

### 🔄 Ready for Expansion
- Product Module (entities available, needs CRUD implementation)
- Customer Module (entities available, needs CRUD implementation) 
- Sales Module (entities available, needs CRUD implementation)

### 📋 Reference Implementation
Use the People module as the gold standard for implementing new modules. All patterns, naming conventions, and architectural decisions should follow the established People module implementation.

### DTO Mapping Extensions (Production Pattern)
```csharp
public static class PersonMappingExtensions
{
    public static PersonListDto ToListDto(this Person person)
    {
        return new PersonListDto
        {
            BusinessEntityId = person.BusinessEntityId,
            FirstName = person.FirstName,
            LastName = person.LastName,
            PersonType = person.PersonType,
            ModifiedDate = person.ModifiedDate
        };
    }

    public static PersonDetailDto ToDetailDto(this Person person)
    {
        return new PersonDetailDto
        {
            BusinessEntityId = person.BusinessEntityId,
            FirstName = person.FirstName,
            LastName = person.LastName,
            MiddleName = person.MiddleName,
            PersonType = person.PersonType,
            Suffix = person.Suffix,
            Title = person.Title,
            EmailPromotion = person.EmailPromotion,
            AdditionalContactInfo = person.AdditionalContactInfo,
            Demographics = person.Demographics,
            Rowguid = person.Rowguid,
            ModifiedDate = person.ModifiedDate
        };
    }

    public static Person ToEntity(this PersonCreateDto dto)
    {
        return new Person
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            MiddleName = dto.MiddleName,
            PersonType = dto.PersonType,
            Suffix = dto.Suffix,
            Title = dto.Title,
            EmailPromotion = dto.EmailPromotion,
            AdditionalContactInfo = dto.AdditionalContactInfo,
            Demographics = dto.Demographics,
            Rowguid = Guid.NewGuid(),
            ModifiedDate = DateTime.UtcNow
        };
    }
}
```

## Module Implementation Checklist

When creating a new CRUD module, follow this production checklist based on the People module:

### 1. ✅ Core Layer (AdventureWorks.Core)
- [ ] Verify entity exists in `/Entities/` folder
- [ ] Create service interface in `/Interfaces/I{Entity}Service.cs`

### 2. ✅ Shared Layer (AdventureWorks.Shared)
- [ ] Create `{Entity}Dto.cs` - Basic entity data
- [ ] Create `{Entity}DetailDto.cs` - Extended data for details view
- [ ] Create `{Entity}ListDto.cs` - Compact data for grid listing
- [ ] Create `{Entity}CreateDto.cs` - Data for creation (no ID)
- [ ] Create `{Entity}UpdateDto.cs` - Data for updates
- [ ] Create `{Entity}QueryParameters.cs` - Search and filter parameters

### 3. ✅ Infrastructure Layer (AdventureWorks.Infrastructure)
- [ ] Create `/Repositories/I{Entity}Repository.cs` interface
- [ ] Implement `/Repositories/{Entity}Repository.cs` class extending GenericRepository
- [ ] Create `/Services/{Entity}Service.cs` implementing I{Entity}Service
- [ ] Create `/Mappings/{Entity}MappingExtensions.cs` with ToDto methods
- [ ] Register services in `/Extensions/ServiceCollectionExtensions.cs`

### 4. ✅ Web Layer (AdventureWorks.Web)
- [ ] Create `/Pages/{Entity}/Index.razor` - Data grid with paging, sorting, filtering
- [ ] Create `/Pages/{Entity}/Details.razor` - Read-only detail view with responsive layout
- [ ] Create `/Pages/{Entity}/Create.razor` - Create form with validation
- [ ] Create `/Pages/{Entity}/Edit.razor` - Edit form with dual navigation
- [ ] Add navigation menu items in `/Shared/NavMenu.razor`

### 5. ✅ Testing & Validation
- [ ] Build project successfully (`dotnet build`)
- [ ] Test all CRUD operations in browser
- [ ] Verify responsive design on mobile/tablet/desktop
- [ ] Test error handling and loading states
- [ ] Verify navigation flows work correctly

### 6. ✅ Documentation Updates
- [ ] Update Plan_van_Aanpak.md with completed module
- [ ] Update README.md if needed
- [ ] Document any special considerations

Follow the People module as the gold standard - all patterns, naming conventions, and implementations should match exactly.
