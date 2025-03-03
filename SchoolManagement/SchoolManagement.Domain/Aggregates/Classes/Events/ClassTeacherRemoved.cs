using Sekiban.Pure.Events;

namespace SchoolManagement.Domain.Aggregates.Classes.Events;

[GenerateSerializer]
public record ClassTeacherRemoved() : IEventPayload;