using Sekiban.Pure.Events;

namespace SchoolManagement.Domain.Aggregates.Students.Events;

[GenerateSerializer]
public record StudentRegistered(
    string Name,
    string StudentId,
    DateTime DateOfBirth,
    string Email,
    string PhoneNumber,
    string Address
) : IEventPayload;
