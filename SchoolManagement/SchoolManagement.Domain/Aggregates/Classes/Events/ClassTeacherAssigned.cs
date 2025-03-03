using Sekiban.Pure.Events;

namespace SchoolManagement.Domain.Aggregates.Classes.Events;

[GenerateSerializer]
public record ClassTeacherAssigned(
    Guid TeacherId
) : IEventPayload;