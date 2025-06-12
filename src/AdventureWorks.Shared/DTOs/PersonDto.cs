namespace AdventureWorks.Shared.DTOs;

public class PersonDto
{
    public int BusinessEntityId { get; set; }
    public string PersonType { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {MiddleName} {LastName}".Replace("  ", " ").Trim();
    public List<string> EmailAddresses { get; set; } = new();
    public List<PersonPhoneDto> Phones { get; set; } = new();
    public List<AddressDto> Addresses { get; set; } = new();
    public DateTime ModifiedDate { get; set; }
}

public class PersonListDto
{
    public int BusinessEntityId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string? PrimaryEmail { get; set; }
    public string? PrimaryPhone { get; set; }
    public string PersonType { get; set; } = string.Empty;
}

public class PersonCreateDto
{
    public string PersonType { get; set; } = "IN"; // Individual
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
}

public class PersonUpdateDto : PersonCreateDto
{
    public int BusinessEntityId { get; set; }
}
