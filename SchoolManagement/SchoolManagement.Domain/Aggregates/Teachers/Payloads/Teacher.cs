using Sekiban.Pure.Aggregates;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record Teacher(
    string Name,
    string TeacherId,
    string Email,
    string PhoneNumber,
    string Address,
    string Subject,
    List<Guid> ClassIds = null
) : IAggregatePayload
{
    public Teacher() : this("", "", "", "", "", "", new List<Guid>()) { }
    
    public List<Guid> ClassIds { get; init; } = ClassIds ?? new List<Guid>();
}