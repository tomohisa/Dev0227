using Sekiban.Pure.Aggregates;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record DeletedTeacher(
    string Name,
    string TeacherId,
    string Email,
    string PhoneNumber,
    string Address,
    string Subject,
    List<Guid> ClassIds = null
) : IAggregatePayload
{
    public List<Guid> ClassIds { get; init; } = ClassIds ?? new List<Guid>();
}