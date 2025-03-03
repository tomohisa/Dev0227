using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record TeacherRegistered(
    string Name,
    string TeacherId,
    string Email,
    string PhoneNumber,
    string Address,
    string Subject
) : IEventPayload;