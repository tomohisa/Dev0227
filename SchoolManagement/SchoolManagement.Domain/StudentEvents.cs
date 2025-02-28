using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record StudentRegistered(
    string Name,
    string StudentId,
    DateTime DateOfBirth,
    string Email,
    string PhoneNumber,
    string Address
) : IEventPayload;

[GenerateSerializer]
public record StudentDeleted() : IEventPayload;

[GenerateSerializer]
public record StudentUpdated(
    string Name = null,
    string Email = null,
    string PhoneNumber = null,
    string Address = null
) : IEventPayload;

[GenerateSerializer]
public record StudentAssignedToClass(
    Guid ClassId
) : IEventPayload;

[GenerateSerializer]
public record StudentRemovedFromClass() : IEventPayload;
