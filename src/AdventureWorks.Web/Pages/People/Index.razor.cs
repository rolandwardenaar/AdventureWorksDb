using Microsoft.AspNetCore.Components;
using MudBlazor;
using AdventureWorks.Shared.DTOs;

namespace AdventureWorks.Web.Pages.People;

public partial class Index
{
    private MudDataGrid<PersonListDto>? dataGrid;
    private PagedResult<PersonListDto>? pagedResult;
    private PersonQueryParameters queryParams = new();
    private DateRange? dateRange;
    private bool isLoading = false;
    private string _searchString = "";

    // Quick filter function for real-time search
    private Func<PersonListDto, bool> _quickFilterFunc => x =>
    {
        if (string.IsNullOrWhiteSpace(_searchString))
            return true;

        if (x.FullName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            return true;

        if (x.PrimaryEmail?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            return true;

        if (x.PrimaryPhone?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            return true;

        return false;
    };

    protected override async Task OnInitializedAsync()
    {
        // Set default page size
        queryParams.PageSize = 25;
        await base.OnInitializedAsync();
    }

    private async Task<GridData<PersonListDto>> LoadServerData(GridState<PersonListDto> state)
    {
        isLoading = true;

        try
        {
            // Map grid state to query parameters
            queryParams.Page = state.Page + 1; // MudBlazor uses 0-based paging
            queryParams.PageSize = state.PageSize;

            // Handle multi-column sorting
            if (state.SortDefinitions?.Any() == true)
            {
                var primarySort = state.SortDefinitions.First();
                queryParams.SortBy = primarySort.SortBy;
                queryParams.SortDescending = primarySort.Descending;

                // TODO: Implement secondary sorting in PersonQueryParameters
                // For now, we only use the primary sort
            }
            else
            {
                // Default sorting by LastName
                queryParams.SortBy = "LastName";
                queryParams.SortDescending = false;
            }

            // Apply quick search if present
            if (!string.IsNullOrEmpty(_searchString) && string.IsNullOrEmpty(queryParams.SearchTerm))
            {
                queryParams.SearchTerm = _searchString;
            }

            // Apply date range filter
            if (dateRange?.Start != null)
                queryParams.CreatedAfter = dateRange.Start;
            if (dateRange?.End != null)
                queryParams.CreatedBefore = dateRange.End;

            // Load data
            pagedResult = await PersonService.GetPersonsPagedAsync(queryParams);

            return new GridData<PersonListDto>
            {
                Items = pagedResult.Items,
                TotalItems = pagedResult.TotalCount
            };
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading people: {ex.Message}", Severity.Error);
            return new GridData<PersonListDto>
            {
                Items = Enumerable.Empty<PersonListDto>(),
                TotalItems = 0
            };
        }
        finally
        {
            isLoading = false;
        }
    }

    private async Task ApplyFilters()
    {
        // Apply date range
        if (dateRange?.Start != null)
            queryParams.CreatedAfter = dateRange.Start;
        if (dateRange?.End != null)
            queryParams.CreatedBefore = dateRange.End;

        // Reset to first page when applying filters
        queryParams.Page = 1;

        // Reload the grid
        if (dataGrid != null)
        {
            await dataGrid.ReloadServerData();
        }
    }

    private async Task ClearFilters()
    {
        queryParams = new PersonQueryParameters { PageSize = queryParams.PageSize };
        dateRange = null;
        _searchString = "";

        if (dataGrid != null)
        {
            await dataGrid.ReloadServerData();
        }

        Snackbar.Add("All filters cleared", Severity.Info);
    }

    private async Task ExportData()
    {
        try
        {
            isLoading = true;

            // Get all data for export (with current filters but no paging)
            var exportParams = new PersonQueryParameters
            {
                SearchTerm = queryParams.SearchTerm,
                PersonType = queryParams.PersonType,
                FirstName = queryParams.FirstName,
                LastName = queryParams.LastName,
                Email = queryParams.Email,
                CreatedAfter = queryParams.CreatedAfter,
                CreatedBefore = queryParams.CreatedBefore,
                SortBy = queryParams.SortBy,
                SortDescending = queryParams.SortDescending,
                Page = 1,
                PageSize = int.MaxValue // Get all records
            };

            var exportResult = await PersonService.GetPersonsPagedAsync(exportParams);

            // For now, just show success message
            // In a real app, you'd generate CSV/Excel here
            Snackbar.Add($"Export would contain {exportResult.TotalCount} records", Severity.Info);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Export failed: {ex.Message}", Severity.Error);
        }
        finally
        {
            isLoading = false;
        }
    }

    private void ViewPerson(int businessEntityId)
    {
        Navigation.NavigateTo($"/people/{businessEntityId}");
    }

    private void EditPerson(int businessEntityId)
    {
        Navigation.NavigateTo($"/people/{businessEntityId}/edit");
    }

    private async Task ConfirmDelete(PersonListDto person)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Confirm Delete",
            $"Are you sure you want to delete {person.FullName}?",
            yesText: "Delete",
            cancelText: "Cancel");

        if (result == true)
        {
            await DeletePerson(person.BusinessEntityId);
        }
    }

    private async Task DeletePerson(int businessEntityId)
    {
        try
        {
            var success = await PersonService.DeletePersonAsync(businessEntityId);
            if (success)
            {
                Snackbar.Add("Person deleted successfully", Severity.Success);
                if (dataGrid != null)
                {
                    await dataGrid.ReloadServerData();
                }
            }
            else
            {
                Snackbar.Add("Failed to delete person", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error deleting person: {ex.Message}", Severity.Error);
        }
    }

    private Color GetPersonTypeColor(string personType) => personType switch
    {
        "IN" => Color.Primary,
        "EM" => Color.Success,
        "SP" => Color.Warning,
        "SC" => Color.Info,
        "VC" => Color.Secondary,
        _ => Color.Default
    };

    private string GetPersonTypeText(string personType) => personType switch
    {
        "IN" => "Individual",
        "EM" => "Employee",
        "SP" => "Sales Person",
        "SC" => "Store Contact",
        "VC" => "Vendor Contact",
        _ => personType
    };

    // Helper methods for filter status
    private bool HasActiveFilters()
    {
        return !string.IsNullOrEmpty(queryParams.SearchTerm) ||
               !string.IsNullOrEmpty(queryParams.PersonType) ||
               !string.IsNullOrEmpty(queryParams.FirstName) ||
               !string.IsNullOrEmpty(queryParams.LastName) ||
               !string.IsNullOrEmpty(queryParams.Email) ||
               queryParams.CreatedAfter.HasValue ||
               queryParams.CreatedBefore.HasValue ||
               !string.IsNullOrEmpty(_searchString);
    }

    private int GetActiveFiltersCount()
    {
        int count = 0;
        if (!string.IsNullOrEmpty(queryParams.SearchTerm)) count++;
        if (!string.IsNullOrEmpty(queryParams.PersonType)) count++;
        if (!string.IsNullOrEmpty(queryParams.FirstName)) count++;
        if (!string.IsNullOrEmpty(queryParams.LastName)) count++;
        if (!string.IsNullOrEmpty(queryParams.Email)) count++;
        if (queryParams.CreatedAfter.HasValue) count++;
        if (queryParams.CreatedBefore.HasValue) count++;
        if (!string.IsNullOrEmpty(_searchString)) count++;
        return count;
    }
}