namespace AdventureWorks.Shared.DTOs
{
    /// <summary>
    /// Generic paged result for server-side paging
    /// </summary>
    /// <typeparam name="T">Type of items in the result</typeparam>
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNextPage => Page < TotalPages;
        public bool HasPreviousPage => Page > 1;
    }

    /// <summary>
    /// Query parameters for paging, sorting and filtering
    /// </summary>
    public class QueryParameters
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;
        public string? SearchTerm { get; set; }
        public Dictionary<string, object?> Filters { get; set; } = new();
    }

    /// <summary>
    /// Specific query parameters for Person search
    /// </summary>
    public class PersonQueryParameters : QueryParameters
    {
        public string? PersonType { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? City { get; set; }
        public string? StateProvince { get; set; }
        public string? CountryRegion { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }
    }
}
