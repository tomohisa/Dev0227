using Sekiban.Pure.Events;

namespace SchoolManagement.Domain.Aggregates.Classes.Events;

[GenerateSerializer]
public record ClassStudentRemoved(
    Guid StudentId
) : IEventPayload;