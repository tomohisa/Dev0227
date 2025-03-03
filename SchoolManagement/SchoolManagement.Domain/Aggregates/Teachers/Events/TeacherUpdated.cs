using Sekiban.Pure.Events;

namespace SchoolManagement.Domain.Aggregates.Teachers.Events;

[GenerateSerializer]
public record TeacherUpdated(
    string Name = null,
    string Email = null,
    string PhoneNumber = null,
    string Address = null,
    string Subject = null
) : IEventPayload;