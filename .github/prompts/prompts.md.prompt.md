---
mode: beastmode
---
# AdventureWorks Copilot Code Generation Prompts

## Quick Generation Prompts

### Generate Complete CRUD Module
```
Create a complete CRUD module for [EntityName] following the People module pattern:
- Include all DTOs (Dto, DetailDto, ListDto, CreateDto, UpdateDto)
- Implement service interface and implementation with error handling
- Create repository with advanced queries and paging
- Build all 4 Blazor pages (Index, Details, Create, Edit) with mobile-first responsive design
- Include proper DTO mapping extensions
- Register services in DI container
- Follow exact naming conventions and patterns from People module
```

### Generate Data Grid Component
```
Create a MudDataGrid component for [EntityName] with:
- Server-side paging, sorting, and filtering
- Responsive design with mobile-friendly columns
- Search functionality in toolbar
- Action buttons (View, Edit, Delete) with proper touch targets
- Loading states and error handling
- Follow the People/Index.razor pattern exactly
```

### Generate Detail View Page
```
Create a Details.razor page for [EntityName] with:
- Mobile-first responsive layout using MudContainer and MudGrid
- Breadcrumb navigation
- Responsive field display using MudField components
- Loading and error states
- Touch-friendly action buttons with dual navigation
- Follow the People/Details.razor pattern exactly
```

### Generate Create/Edit Forms
```
Create Create.razor and Edit.razor pages for [EntityName] with:
- Mobile-optimized form layout using MudGrid
- Proper validation and error handling
- Loading states during submission
- Responsive button layout with proper touch targets
- Dual navigation options (Edit page only)
- Follow the People module form patterns exactly
```

### Generate Service Layer
```
Create service interface and implementation for [EntityName] with:
- Complete CRUD operations using proper DTOs
- Paging and search functionality
- Repository pattern integration
- Error handling with logging
- Follow IPersonService and PersonService patterns exactly
```

### Generate Repository Layer
```
Create repository interface and implementation for [EntityName] with:
- Extend GenericRepository pattern
- Include advanced query methods (GetByIdWithDetails, Search, Paged)
- EF Core optimizations with Include statements
- Follow IPersonRepository and PersonRepository patterns exactly
```

## Code Patterns for Quick Reference

### Standard DTO Hierarchy
```csharp
// Basic DTO
public class {Entity}Dto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime ModifiedDate { get; set; }
}

// Detail DTO inherits from basic
public class {Entity}DetailDto : {Entity}Dto
{
    // Additional properties for detail view
}

// List DTO - optimized for grid display
public class {Entity}ListDto
{
    // Minimal properties for performance
}

// Create DTO - no ID
public class {Entity}CreateDto
{
    // Properties needed for creation
}

// Update DTO - selective properties
public class {Entity}UpdateDto
{
    // Properties that can be updated
}
```

### Standard Service Pattern
```csharp
public interface I{Entity}Service
{
    Task<PagedResult<{Entity}ListDto>> Get{Entity}sPagedAsync({Entity}QueryParameters queryParams);
    Task<{Entity}DetailDto?> Get{Entity}ByIdAsync(int id);
    Task<IEnumerable<{Entity}ListDto>> Search{Entity}sAsync(string searchTerm);
    Task<{Entity}DetailDto> Create{Entity}Async({Entity}CreateDto dto);
    Task<{Entity}DetailDto> Update{Entity}Async(int id, {Entity}UpdateDto dto);
    Task<bool> Delete{Entity}Async(int id);
}
```

### Standard Repository Pattern
```csharp
public interface I{Entity}Repository : IGenericRepository<{Entity}>
{
    Task<{Entity}?> GetByIdWithDetailsAsync(int id);
    Task<PagedResult<{Entity}>> GetPagedAsync({Entity}QueryParameters parameters);
    Task<IEnumerable<{Entity}>> Search{Entity}sAsync(string searchTerm);
}
```

### Mobile-First Page Structure
```razor
@page "/{entity}"
@inject I{Entity}Service {Entity}Service
@inject NavigationManager Navigation
@inject ISnackbar Snackbar

<PageTitle>{Entity} - AdventureWorks</PageTitle>

<MudContainer MaxWidth="MaxWidth.Small" Class="mt-1 mobile-first-container">
    <MudGrid Spacing="1">
        <!-- Header with breadcrumbs -->
        <MudItem xs="12">
            <MudPaper Class="pa-2 pa-sm-3" Elevation="1">
                <MudBreadcrumbs Items="breadcrumbItems" Separator=">">
                    <ItemTemplate Context="item">
                        <MudLink Href="@item.Href">@item.Text</MudLink>
                    </ItemTemplate>
                </MudBreadcrumbs>
                <MudText Typo="Typo.h6" Class="mt-2">{Entity} Management</MudText>
            </MudPaper>
        </MudItem>

        <!-- Content with loading/error states -->
        @if (loading)
        {
            <MudItem xs="12" Class="d-flex justify-center pa-4">
                <MudProgressCircular Indeterminate="true" />
            </MudItem>
        }
        else if (hasError)
        {
            <MudItem xs="12">
                <MudAlert Severity="Severity.Error">Error loading data</MudAlert>
            </MudItem>
        }
        else
        {
            <!-- Main content here -->
        }
    </MudGrid>
</MudContainer>

@code {
    private bool loading = false;
    private bool hasError = false;
    
    private List<BreadcrumbItem> breadcrumbItems = new()
    {
        new BreadcrumbItem("Home", href: "/"),
        new BreadcrumbItem("{Entity}", href: "/{entity}"),
        new BreadcrumbItem("Current Page", href: null, disabled: true)
    };
}
```

### Responsive Action Buttons
```razor
<MudStack Row Spacing="1" Justify="Justify.Center" Class="action-buttons">
    <MudButton Variant="Variant.Outlined" 
               Color="Color.Secondary" 
               Size="Size.Small"
               StartIcon="@Icons.Material.Filled.ArrowBack"
               Href="/back-url"
               Class="responsive-btn">
        <MudText Class="d-none d-sm-inline">Back to List</MudText>
        <MudText Class="d-inline d-sm-none">Back</MudText>
    </MudButton>
    
    <MudButton Variant="Variant.Filled" 
               Color="Color.Primary" 
               Size="Size.Small"
               StartIcon="@Icons.Material.Filled.Action"
               OnClick="HandleAction"
               Class="responsive-btn">
        <MudText Class="d-none d-sm-inline">Action Name</MudText>
        <MudText Class="d-inline d-sm-none">Action</MudText>
    </MudButton>
</MudStack>

<style>
    .action-buttons .mud-button {
        min-width: 44px;
        min-height: 44px;
    }
</style>
```

## Quality Checklist

Before submitting generated code, ensure:
- [ ] Mobile-first responsive design implemented
- [ ] Loading states included for all async operations  
- [ ] Error handling with user-friendly messages
- [ ] Touch targets minimum 44px
- [ ] Consistent naming following People module
- [ ] Proper DTO hierarchy and mapping
- [ ] Repository pattern with EF Core optimizations
- [ ] Service layer with error handling and logging
- [ ] Dependency injection properly configured
- [ ] Breadcrumb navigation included
- [ ] Action buttons with responsive text
- [ ] Build successfully without warnings

## Common Entities Available
- Person (✅ Complete implementation)
- Product (🔄 Ready for implementation)
- Customer (🔄 Ready for implementation) 
- Employee (🔄 Ready for implementation)
- SalesOrder (🔄 Ready for implementation)
- PurchaseOrder (🔄 Ready for implementation)

Always reference the People module as the gold standard for all new implementations.
