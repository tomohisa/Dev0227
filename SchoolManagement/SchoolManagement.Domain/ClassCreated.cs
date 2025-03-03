using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record ClassCreated(
    string Name,
    string ClassCode,
    string Description
) : IEventPayload;