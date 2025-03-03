using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record ClassStudentRemoved(
    Guid StudentId
) : IEventPayload;