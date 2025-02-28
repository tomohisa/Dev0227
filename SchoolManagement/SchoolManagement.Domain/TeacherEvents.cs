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

[GenerateSerializer]
public record TeacherDeleted() : IEventPayload;

[GenerateSerializer]
public record TeacherUpdated(
    string Name = null,
    string Email = null,
    string PhoneNumber = null,
    string Address = null,
    string Subject = null
) : IEventPayload;

[GenerateSerializer]
public record TeacherAssignedToClass(
    Guid ClassId
) : IEventPayload;

[GenerateSerializer]
public record TeacherRemovedFromClass(
    Guid ClassId
) : IEventPayload;
