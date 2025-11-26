namespace DealChaser.Api.Models;

public class DealRequestDto
{
    // e.g. "Tech", "Gaming", "Beauty", "Home", "Fitness"
    public string Category { get; set; } = string.Empty;

    public decimal? BudgetMin { get; set; }
    public decimal? BudgetMax { get; set; }

    // e.g. "Sony", "Apple", "any"
    public string BrandPreference { get; set; } = string.Empty;

    // e.g. "noise cancelling headphones", "27'' 144Hz monitor"
    public string SearchTerm { get; set; } = string.Empty;

    // e.g. "US", "EU", "UK", "Global"
    public string Region { get; set; } = "Global";

    public int NumberOfIdeas { get; set; } = 5;
}
