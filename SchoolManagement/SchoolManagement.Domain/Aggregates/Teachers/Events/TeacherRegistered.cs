using Sekiban.Pure.Events;

namespace SchoolManagement.Domain.Aggregates.Teachers.Events;

[GenerateSerializer]
public record TeacherRegistered(
    string Name,
    string TeacherId,
    string Email,
    string PhoneNumber,
    string Address,
    string Subject
) : IEventPayload;