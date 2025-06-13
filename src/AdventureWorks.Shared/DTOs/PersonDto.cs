namespace AdventureWorks.Shared.DTOs;

public class PersonDto
{
    public int BusinessEntityId { get; set; }
    public string PersonType { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string? Suffix { get; set; }
    public int EmailPromotion { get; set; }
    public string? Demographics { get; set; }
    public Guid Rowguid { get; set; }
    public DateTime ModifiedDate { get; set; }
    
    public string FullName => $"{Title} {FirstName} {MiddleName} {LastName} {Suffix}".Replace("  ", " ").Trim();
    public List<string> EmailAddresses { get; set; } = new();
    public List<PersonPhoneDto> Phones { get; set; } = new();
    public List<AddressDto> Addresses { get; set; } = new();
}

public class PersonDetailDto
{
    public int BusinessEntityId { get; set; }
    public string PersonType { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string? Suffix { get; set; }
    public int EmailPromotion { get; set; }
    public string? Demographics { get; set; }
    public Guid Rowguid { get; set; }
    public DateTime ModifiedDate { get; set; }
    
    public string FullName => $"{Title} {FirstName} {MiddleName} {LastName} {Suffix}".Replace("  ", " ").Trim();
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
    public string? Title { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string? Suffix { get; set; }
    public int EmailPromotion { get; set; } = 0;
    public string? Demographics { get; set; }
}

public class PersonUpdateDto
{
    public string PersonType { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string? Suffix { get; set; }
    public int EmailPromotion { get; set; }
    public string? Demographics { get; set; }
}
