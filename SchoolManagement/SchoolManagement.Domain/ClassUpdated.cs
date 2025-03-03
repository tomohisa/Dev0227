using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record ClassUpdated(
    string Name = null,
    string ClassCode = null,
    string Description = null
) : IEventPayload;