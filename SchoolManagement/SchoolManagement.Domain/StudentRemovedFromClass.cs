using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record StudentRemovedFromClass() : IEventPayload;