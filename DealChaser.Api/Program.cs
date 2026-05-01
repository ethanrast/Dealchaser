using DealChaser.Api.Models;
using DealChaser.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Bind OpenAI options from appsettings.json / appsettings.Development.json
builder.Services.Configure<OpenAiOptions>(
    builder.Configuration.GetSection("OpenAi"));

// Register services
builder.Services.AddHttpClient<OpenAiDealGenerator>();
builder.Services.AddScoped<IDealGenerator, OpenAiDealGenerator>();

// CORS (mainly useful when testing with separate frontend, harmless in prod)
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

// app.UseHttpsRedirection();
app.UseCors();

// Serve React build from wwwroot
app.UseDefaultFiles();  // looks for index.html by default
app.UseStaticFiles();

// 🔹 API endpoint: POST /api/deals
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

// 🔹 Fallback: any unknown route => index.html (React app)
app.MapFallbackToFile("index.html");

app.Run();
