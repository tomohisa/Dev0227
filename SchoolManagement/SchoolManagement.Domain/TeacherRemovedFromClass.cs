using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record TeacherRemovedFromClass(
    Guid ClassId
) : IEventPayload;