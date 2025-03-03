using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record StudentUpdated(
    string Name = null,
    string Email = null,
    string PhoneNumber = null,
    string Address = null
) : IEventPayload;