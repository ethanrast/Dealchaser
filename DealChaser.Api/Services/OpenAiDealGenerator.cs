using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using DealChaser.Api.Models;
using Microsoft.Extensions.Options;

namespace DealChaser.Api.Services;

public class OpenAiDealGenerator : IDealGenerator
{
    private readonly HttpClient _httpClient;
    private readonly OpenAiOptions _options;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public OpenAiDealGenerator(HttpClient httpClient, IOptions<OpenAiOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _options.ApiKey);
    }

    public async Task<IReadOnlyList<DealIdeaDto>> GenerateDealsAsync(
        DealRequestDto request,
        CancellationToken ct = default)
    {
        var systemPrompt = """
        You are an AI assistant that recommends Black Friday shopping deals.

        Users are NOT looking for gifts. They primarily shop for themselves.

        You DO NOT know real-time prices or live discounts, but you DO know:
        - which products are heavily discounted every Black Friday
        - which categories see the biggest price drops
        - typical price ranges of popular items
        - major online markets globally (for example Amazon, Walmart, Target, Best Buy, AliExpress, Bol.com, and other large retailers)

        Return ONLY a JSON array of objects, no extra text or markdown.

        Each object must contain:
        - name
        - description
        - priceEstimate (e.g. "US$40–$60 Black Friday price" or "€40–€60 Black Friday price")
        - category (e.g. "Tech", "Gaming", "Beauty", "Home", "Fitness")
        - searchKeywords (short, in English, optimized for searching on major e-commerce sites like Amazon)

        Focus on items that are:
        - high-demand
        - commonly discounted
        - desirable for personal use
        - realistic in the user’s stated budget and region if provided

        Examples include electronics, skincare bundles, small appliances, gaming accessories,
        smart home devices, fitness gear, phone accessories, etc.
        """;

        var userPrompt = $"""
        I need {request.NumberOfIdeas} Black Friday deal suggestions.

        Region / market: {(string.IsNullOrWhiteSpace(request.Region) ? "Global / any" : request.Region)}
        Product category: {request.Category}
        Budget range: {(request.BudgetMin?.ToString("0.##") ?? "any")} to {(request.BudgetMax?.ToString("0.##") ?? "any")}
        Brand preference: {request.BrandPreference}
        Extra details: {request.SearchTerm}

        Rules:
        - Do NOT suggest gifts.
        - Suggest items people buy for themselves.
        - Prioritize tech, gadgets, electronics, beauty products, small appliances, and accessories that are commonly discounted on Black Friday.
        - Keep items specific and practical.
        - Keep them within the budget when possible.
        - If a region/market is specified, prefer products and price ranges that make sense for that region.
        """;

        var payload = new
        {
            model = _options.Model,
            messages = new[]
            {
                new { role = "system", content = systemPrompt },
                new { role = "user", content = userPrompt }
            }
        };

        var json = JsonSerializer.Serialize(payload, JsonOptions);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(
            "https://api.openai.com/v1/chat/completions",
            content,
            ct);

        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync(ct);
        using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);

        var replyContent = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        if (string.IsNullOrWhiteSpace(replyContent))
        {
            return Array.Empty<DealIdeaDto>();
        }

        // Strip possible ```json fences
        replyContent = replyContent.Trim();
        if (replyContent.StartsWith("```"))
        {
            var firstNewLine = replyContent.IndexOf('\n');
            var lastFence = replyContent.LastIndexOf("```", StringComparison.Ordinal);
            if (firstNewLine >= 0 && lastFence > firstNewLine)
            {
                replyContent = replyContent.Substring(
                    firstNewLine + 1,
                    lastFence - firstNewLine - 1).Trim();
            }
        }

        try
        {
            var ideas = JsonSerializer.Deserialize<List<DealIdeaDto>>(replyContent, JsonOptions);
            return ideas ?? new List<DealIdeaDto>();
        }
        catch
        {
            return Array.Empty<DealIdeaDto>();
        }
    }
}
