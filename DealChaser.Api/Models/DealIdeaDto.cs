namespace DealChaser.Api.Models;

public class DealIdeaDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PriceEstimate { get; set; } = string.Empty;  // "US$40–$60 Black Friday price"
    public string Category { get; set; } = string.Empty;       // "Tech", "Gaming", ...
    public string SearchKeywords { get; set; } = string.Empty; // For Amazon/Bol search
}
