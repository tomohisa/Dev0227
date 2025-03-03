using Sekiban.Pure.Events;

namespace SchoolManagement.Domain.Aggregates.Classes.Events;

[GenerateSerializer]
public record ClassCreated(
    string Name,
    string ClassCode,
    string Description
) : IEventPayload;