using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record ClassTeacherAssigned(
    Guid TeacherId
) : IEventPayload;