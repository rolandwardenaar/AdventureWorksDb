using Microsoft.AspNetCore.Components;
using MudBlazor;
using AdventureWorks.Shared.DTOs;

namespace AdventureWorks.Web.Pages.People;

public partial class Details
{
    [Parameter] public int Id { get; set; }

    private PersonDetailDto? person;
    private bool loading = false;

    private List<BreadcrumbItem> breadcrumbItems = new()
    {
        new BreadcrumbItem("Home", href: "/"),
        new BreadcrumbItem("People", href: "/people"),
        new BreadcrumbItem("Details", href: null, disabled: true)
    };

    protected override async Task OnParametersSetAsync()
    {
        loading = true;
        try
        {
            person = await PersonService.GetPersonByIdAsync(Id);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading person: {ex.Message}", Severity.Error);
            person = null;
        }
        finally
        {
            loading = false;
        }
    }

    private string GetPersonTypeDisplay(string personType)
    {
        return personType switch
        {
            "IN" => "Individual Customer",
            "EM" => "Employee",
            "SP" => "Sales Person",
            "SC" => "Store Contact",
            "VC" => "Vendor Contact",
            _ => personType
        };
    }

    private string GetEmailPromotionDisplay(int emailPromotion)
    {
        return emailPromotion switch
        {
            0 => "No email promotions",
            1 => "AdventureWorks only",
            2 => "AdventureWorks and partners",
            _ => emailPromotion.ToString()
        };
    }
}