using Sekiban.Pure.Events;

namespace SchoolManagement.Domain.Aggregates.Classes.Events;

[GenerateSerializer]
public record ClassUpdated(
    string Name = null,
    string ClassCode = null,
    string Description = null
) : IEventPayload;