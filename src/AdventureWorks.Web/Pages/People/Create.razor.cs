using Microsoft.AspNetCore.Components;
using MudBlazor;
using AdventureWorks.Shared.DTOs;

namespace AdventureWorks.Web.Pages.People;

public partial class Create
{
    private PersonCreateDto createDto = new();
    private bool isSubmitting = false;

    private List<BreadcrumbItem> breadcrumbItems = new()
    {
        new BreadcrumbItem("Home", href: "/"),
        new BreadcrumbItem("People", href: "/people"),
        new BreadcrumbItem("Add New", href: null, disabled: true)
    };

    protected override void OnInitialized()
    {
        // Set default values
        createDto.PersonType = "IN"; // Individual by default
        createDto.EmailPromotion = 0; // No promotions by default
    }

    private async Task HandleValidSubmit()
    {
        isSubmitting = true;

        try
        {
            var result = await PersonService.CreatePersonAsync(createDto);

            if (result != null)
            {
                Snackbar.Add($"Person '{result.FullName}' created successfully!", Severity.Success);
                Navigation.NavigateTo($"/people/{result.BusinessEntityId}");
            }
            else
            {
                Snackbar.Add("Failed to create person. Please try again.", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error creating person: {ex.Message}", Severity.Error);
        }
        finally
        {
            isSubmitting = false;
        }
    }

    private void Cancel()
    {
        Navigation.NavigateTo("/people");
    }
}