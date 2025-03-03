using Sekiban.Pure.Events;

namespace SchoolManagement.Domain.Aggregates.Students.Events;

[GenerateSerializer]
public record StudentAssignedToClass(
    Guid ClassId
) : IEventPayload;
