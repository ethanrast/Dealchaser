using DealChaser.Api.Models;
using DealChaser.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Bind OpenAI options from appsettings.json
builder.Services.Configure<OpenAiOptions>(
    builder.Configuration.GetSection("OpenAi"));

// Register services
builder.Services.AddHttpClient<OpenAiDealGenerator>();
builder.Services.AddScoped<IDealGenerator, OpenAiDealGenerator>();

// CORS for your React dev server (for now: allow all)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

var app = builder.Build();

app.UseCors();

app.MapGet("/", () => "DealChaser.ai – Black Friday Deal API");

// POST /api/deals
app.MapPost("/api/deals", async (
    DealRequestDto request,
    IDealGenerator generator,
    CancellationToken ct) =>
{
    if (request.NumberOfIdeas <= 0 || request.NumberOfIdeas > 20)
    {
        request.NumberOfIdeas = 5;
    }

    var ideas = await generator.GenerateDealsAsync(request, ct);
    return Results.Ok(ideas);
});

app.Run();
