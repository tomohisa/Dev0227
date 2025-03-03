using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record TeacherAssignedToClass(
    Guid ClassId
) : IEventPayload;