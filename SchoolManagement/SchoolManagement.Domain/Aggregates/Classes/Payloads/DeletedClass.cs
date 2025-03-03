using Sekiban.Pure.Aggregates;

namespace SchoolManagement.Domain.Aggregates.Classes.Payloads;

[GenerateSerializer]
public record DeletedClass(
    string Name,
    string ClassCode,
    string Description,
    Guid? TeacherId = null,
    List<Guid> StudentIds = null
) : IAggregatePayload
{
    public List<Guid> StudentIds { get; init; } = StudentIds ?? new List<Guid>();
}