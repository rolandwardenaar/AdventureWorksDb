using Microsoft.AspNetCore.Components;
using MudBlazor;
using AdventureWorks.Shared.DTOs;

namespace AdventureWorks.Web.Pages.People;

public partial class Edit
{
    [Parameter] public int Id { get; set; }

    private PersonDetailDto? person;
    private PersonUpdateDto personUpdateDto = new();
    private bool isLoading = true;
    private bool isSubmitting = false;

    private List<BreadcrumbItem> _breadcrumbItems = new()
    {
        new BreadcrumbItem("Home", href: "/"),
        new BreadcrumbItem("People", href: "/people"),
        new BreadcrumbItem("Edit", href: null, disabled: true)
    };

    protected override async Task OnInitializedAsync()
    {
        await LoadPerson();
    }

    private async Task LoadPerson()
    {
        isLoading = true;

        try
        {
            person = await PersonService.GetPersonByIdAsync(Id);

            if (person != null)
            {
                // Update breadcrumb with person name
                _breadcrumbItems[2] = new BreadcrumbItem($"Edit {person.FullName}", href: null, disabled: true);

                // Map to update DTO
                personUpdateDto = new PersonUpdateDto
                {
                    PersonType = person.PersonType,
                    Title = person.Title,
                    FirstName = person.FirstName,
                    MiddleName = person.MiddleName,
                    LastName = person.LastName,
                    Suffix = person.Suffix,
                    EmailPromotion = person.EmailPromotion,
                    Demographics = person.Demographics
                };
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading person: {ex.Message}", Severity.Error);
        }
        finally
        {
            isLoading = false;
        }
    }

    private async Task HandleValidSubmit()
    {
        isSubmitting = true;

        try
        {
            var result = await PersonService.UpdatePersonAsync(Id, personUpdateDto);

            if (result != null)
            {
                Snackbar.Add($"Person '{result.FullName}' updated successfully!", Severity.Success);
                Navigation.NavigateTo($"/people/{result.BusinessEntityId}");
            }
            else
            {
                Snackbar.Add("Failed to update person. Please try again.", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error updating person: {ex.Message}", Severity.Error);
        }
        finally
        {
            isSubmitting = false;
        }
    }

    private void Cancel()
    {
        Navigation.NavigateTo($"/people/{Id}");
    }

    private async Task ResetForm()
    {
        await LoadPerson(); // Reload original data
        Snackbar.Add("Form reset to original values", Severity.Info);
    }
}