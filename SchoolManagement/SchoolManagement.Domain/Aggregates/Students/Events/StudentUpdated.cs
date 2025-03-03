using Sekiban.Pure.Events;

namespace SchoolManagement.Domain.Aggregates.Students.Events;

[GenerateSerializer]
public record StudentUpdated(
    string Name = null,
    string Email = null,
    string PhoneNumber = null,
    string Address = null
) : IEventPayload;
