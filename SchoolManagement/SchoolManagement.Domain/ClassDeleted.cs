using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record ClassDeleted() : IEventPayload;