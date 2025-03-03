using Sekiban.Pure.Aggregates;

namespace SchoolManagement.Domain.Aggregates.Classes.Payloads;

[GenerateSerializer]
public record Class(
    string Name,
    string ClassCode,
    string Description,
    Guid? TeacherId = null,
    List<Guid> StudentIds = null
) : IAggregatePayload
{
    public Class() : this("", "", "", null, new List<Guid>()) { }
    
    public List<Guid> StudentIds { get; init; } = StudentIds ?? new List<Guid>();
}