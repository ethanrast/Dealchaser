using DealChaser.Api.Models;

namespace DealChaser.Api.Services;

public interface IDealGenerator
{
    Task<IReadOnlyList<DealIdeaDto>> GenerateDealsAsync(
        DealRequestDto request,
        CancellationToken ct = default);
}
