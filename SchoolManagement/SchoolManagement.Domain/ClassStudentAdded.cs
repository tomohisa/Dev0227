using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record ClassStudentAdded(
    Guid StudentId
) : IEventPayload;