using Sekiban.Pure.Events;

namespace SchoolManagement.Domain.Aggregates.Teachers.Events;

[GenerateSerializer]
public record TeacherRemovedFromClass(
    Guid ClassId
) : IEventPayload;