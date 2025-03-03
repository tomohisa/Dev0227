using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record StudentAssignedToClass(
    Guid ClassId
) : IEventPayload;